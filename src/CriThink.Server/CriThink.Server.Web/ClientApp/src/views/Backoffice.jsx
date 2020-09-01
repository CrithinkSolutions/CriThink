import React, { Component } from 'react';
import { Table, Button, Container, Row, Col, ButtonGroup } from 'reactstrap';
import { TableHeader, TableHeaderCell, TableRow, TableBody, TableCell, Icon, Segment, Dimmer, Loader } from 'semantic-ui-react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { openCustomDialog, openConfirmationDialog } from '../actions/app';
import { changeCurrentList, loadAllSources, removeBlacklistedSite, removeWhitelistedSite } from '../actions/backoffice';
import AddSiteModal from '../components/modals/AddSiteModal';

class Backoffice extends Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        this.props.loadAllSources();
    }

    openModal = () => {
        this.props.openCustomDialog(<AddSiteModal />);
    }

    showDeleteConfirm = (data) => {
        const title = `Do you really want to delete "${ data }" from the sources?`
        this.props.openConfirmationDialog(undefined, title, this.removeSite, data);
    }

    removeSite = (site) => {
        if (this.props.currentList === 'blacklist') {
            this.props.removeBlacklistedSite(site);
        }
        else if (this.props.currentList === 'whitelist') {
            this.props.removeWhitelistedSite(site);
        }
    }

    renderSiteRow = (siteItem) => (
        <TableRow key={siteItem.uri}>
            <TableCell>
                {siteItem.uri}
            </TableCell>
            <TableCell>
                {siteItem.classification}
            </TableCell>
            <TableCell>
                {siteItem.notes || '-'}
            </TableCell>
            <TableCell width='2' align='right'>
                <ButtonGroup>
                    {/* <Button color='warning'>
                        <Icon name='edit'></Icon>edit
                    </Button> */}
                    <Button color='danger' onClick={() => this.showDeleteConfirm(siteItem.uri, this.props.currentList)}>
                        <Icon name='remove circle'></Icon>delete
                    </Button>
                </ButtonGroup>
            </TableCell>
        </TableRow>
    );

    render() {
        return <Container>
            <Row>
                <Col>
                    <h1>Sites Manager</h1>
                </Col>
                <Col>
                    <Button color='primary' style={{ float: 'right' }} onClick={this.openModal}>
                        <Icon name='add'></Icon>add</Button>
                </Col>
            </Row>
            <Row>
                <Col>
                    <Button outline style={{ width: '100%' }}
                        active={this.props.currentList === 'blacklist'}
                        onClick={() => this.props.changeCurrentList('blacklist')}>Blacklist</Button>
                </Col>
                <Col>
                    <Button outline style={{ width: '100%' }}
                        active={this.props.currentList === 'whitelist'}
                        onClick={() => this.props.changeCurrentList('whitelist')}>Whitelist</Button>
                </Col>
            </Row>
            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHeaderCell>Domain</TableHeaderCell>
                        <TableHeaderCell>Classification</TableHeaderCell>
                        <TableHeaderCell>Notes</TableHeaderCell>
                        <TableHeaderCell>Actions</TableHeaderCell>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {this.props.sites.map(this.renderSiteRow)}
                </TableBody>
                <Dimmer active={this.props.loading} inverted>
                    <Loader size='huge'>{this.props.loadingMessage}</Loader>
                </Dimmer>
            </Table>
        </Container>;
    };
}

function mapStateToProps(state) {
    return {
        sites: state.backoffice.currentList === 'whitelist'
            ? state.backoffice.whitelist
            : state.backoffice.blacklist,
        currentList: state.backoffice.currentList,
        loading: !!state.app.loading.find(x => x.label === 'getAllSources'),
        loadingMessage: state.app.loadingMessage,
    };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        openCustomDialog,
        openConfirmationDialog,
        changeCurrentList,
        loadAllSources,
        removeBlacklistedSite,
        removeWhitelistedSite,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(Backoffice);