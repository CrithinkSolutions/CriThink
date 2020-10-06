import React, { Component } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button as BootstrapButton } from 'reactstrap';
import { Button, Form, Radio, Dropdown } from 'semantic-ui-react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { closeDialog } from '../../actions/app';
import { addNewsSource } from '../../actions/backoffice';
import { validHostname } from '../../lib/utils';

class AddSiteModal extends Component {
    constructor (props) {
        super(props);

        this.state = {
            domain: '',
            notes: '',
            list: undefined,
            classification: undefined,
            canPost: false,
        };
    }

    changeHandler = (e, sender) => {
        const { name, value } = sender;

        if (name === 'list') {
            this.setState((state) => ({
                classification: state.list === value ? state.classification : '',
            }));
        }

        this.setState({
            [name]: value,
        });

        this.validateForm();
    }

    validateForm = () => {
        this.setState((state) => ({
            canPost: validHostname(state.domain) && !!state.domain && !!state.list && !!state.classification,
        }));
    }

    closeDialog = () => {
        this.setState({
            domain: '',
            notes: '',
            list: undefined,
            classification: undefined,
            canPost: false,
        });
        this.props.closeDialog();
    }

    getOptions = () => {
        if (this.state.list === undefined) return [];

        let values = [];

        // TEMPORARY HARDCODED

        if (this.state.list === 'blacklist') values = ['Conspiracist', 'Fake News'];

        else values = ['Reliable', 'Satirical'];

        return values.map(x => ({ key: x, value: x, text: x }));
    }

    addNews = () => {
        const { domain, classification, notes, list } = this.state;

        this.props.addNewsSource({
            domain,
            classification,
            notes,
            list,
        });
    }

    render () {
        return (
            <Modal isOpen={this.props.dialogOpen} centered>
                <ModalHeader
                    close={<BootstrapButton close onClick={this.closeDialog} disabled={this.props.loading} />}
                >
                    Add new site
                </ModalHeader>
                <ModalBody>
                    <Form>
                        <Form.Input
                            icon='newspaper'
                            iconPosition='left'
                            label='Domain'
                            placeholder='Domain'
                            name='domain'
                            onChange={this.changeHandler}
                            value={this.state.domain}
                        />
                        <Form.Input
                            icon='newspaper'
                            iconPosition='left'
                            label='Notes'
                            placeholder='Notes'
                            name='notes'
                            onChange={this.changeHandler}
                            value={this.state.notes}
                        />
                        <Form.Group grouped>
                            <label>Destination list:</label>
                            <Form.Field>
                                <Radio checked={this.state.list === 'blacklist'}
                                    name='list' label='Blacklist' value='blacklist'
                                    onClick={this.changeHandler}
                                />
                            </Form.Field>
                            <Form.Field>
                                <Radio checked={this.state.list === 'whitelist'}
                                    name='list' label='Whitelist' value='whitelist'
                                    onClick={this.changeHandler}
                                />
                            </Form.Field>
                        </Form.Group>
                        <Form.Group grouped>
                            <label>Classification:</label>
                            <Form.Field>
                                <Dropdown disabled={!this.state.list}
                                    options={this.getOptions()}
                                    placeholder='Choose classification...'
                                    selection

                                    name='classification'
                                    onChange={this.changeHandler}
                                    value={this.state.classification} />
                            </Form.Field>
                        </Form.Group>
                    </Form>
                </ModalBody>
                <ModalFooter>
                    <Button color='blue'
                        disabled={!this.state.canPost || this.props.loading}
                        loading={this.props.loading}
                        onClick={this.addNews}>add</Button>
                    <Button onClick={this.closeDialog} disabled={this.props.loading}>cancel</Button>
                </ModalFooter>
            </Modal>
        );
    }
}

function mapStateToProps (state) {
    return {
        dialogOpen: state.app.dialogOpen,
        loading: !!state.app.loading.find(x => x.label === 'addNewsSource'),
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        closeDialog,
        addNewsSource,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(AddSiteModal);
