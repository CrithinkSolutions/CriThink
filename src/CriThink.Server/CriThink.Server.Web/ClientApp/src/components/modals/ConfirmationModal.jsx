import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button as BootstrapButton } from 'reactstrap';
import { Button } from 'semantic-ui-react';
import { closeDialog } from '../../actions/app';

class ConfirmationModal extends Component {
    closeDialog = () => {
        this.props.closeDialog();
    }

    render() {
        const { title, body, data } = this.props;
        return (
            <Modal isOpen={ this.props.dialogOpen }>
                <ModalHeader close={<BootstrapButton close onClick={this.closeDialog} disabled={this.props.loading} />}>
                    {title || 'Are you sure?'}
                </ModalHeader>
                <ModalBody>
                    {body}
                </ModalBody>
                <ModalFooter>
                <Button color='blue'
                        disabled={this.props.loading}
                        loading={this.props.loading}
                        onClick={() => this.props.confirmationHandler(data)}>yes</Button>
                    <Button onClick={this.closeDialog} disabled={this.props.loading}>no</Button>
                </ModalFooter>
            </Modal>
        );
    }
}

function mapStateToProps(state) {
    return {
        dialogOpen: state.app.dialogOpen,
        loading: !!state.app.loading.find(x => x.label === 'confirmationDialog')
    };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        closeDialog,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ConfirmationModal);