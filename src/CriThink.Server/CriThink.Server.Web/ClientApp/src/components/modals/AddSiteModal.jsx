import React, { Component } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { Button, Form, Radio } from 'semantic-ui-react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { closeDialog } from '../../actions/app';

class AddSiteModal extends Component {
    constructor(props) {
        super(props);

        this.state = {
            domain: '',
            notes: '',
            canPost: false,
            list: undefined,
        };
    }

    changeHandler = (e, { name, value }) => {
        this.setState({
            [name]: value,
        });

        this.validateForm();
    }

    validateForm = () => {
        this.setState((state) => ({
            canPost: !!state.domain && !! state.list,
        }));
    }

    closeDialog = () => {
        this.setState({
            domain: '',
            notes: '',
            list: undefined,
            canPost: false,
        });
        this.props.closeDialog();
    }

    render() {
        return <Modal isOpen={this.props.dialogOpen} centered>
            <ModalHeader close={<Button close onClick={this.closeDialog}/>}>Add new site</ModalHeader>
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
                                name='list' label='Blacklist' value='blackcorrierelist'
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
                </Form>
            </ModalBody>
            <ModalFooter>
                <Button color='primary' 
                    disabled={!this.state.canPost}
                    loading={this.props.loading}
                    onClick={this.addNews}
                    >add</Button>
                <Button onClick={this.closeDialog}>cancel</Button>
            </ModalFooter>
        </Modal>
    }
}

function mapStateToProps(state) {
    return {
        dialogOpen: state.app.dialogOpen,
        loading: !!state.app.loading.find(x => x.label === 'addNews'),
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        closeDialog,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(AddSiteModal);
