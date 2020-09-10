import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { closeDialog } from '../../actions/app';
import { Button } from 'semantic-ui-react';
import { Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import './../../custom.css'

class LicenceModal extends Component {

	closeDialog = () => {
        this.props.closeDialog();
    }

	render() {
		return (
			<Modal isOpen={this.props.dialogOpen} centered>
                <ModalHeader>FontAwesome Licence</ModalHeader>
                <ModalBody>
                    <p>Font Awesome Free is free, open source, and GPL friendly. You can use it for
                    commercial projects, open source projects, or really almost whatever you want.
                    Full Font Awesome Free license: <a href='https://fontawesome.com/license/free'>https://fontawesome.com/license/free</a>.</p>
                    <p><b>Icons: CC BY 4.0 License</b> (<a href='https://creativecommons.org/licenses/by/4.0/'>https://creativecommons.org/licenses/by/4.0/</a>)
                    In the Font Awesome Free download, the CC BY 4.0 license applies to all icons
                    packaged as SVG and JS file types.</p>
                </ModalBody>
                <ModalFooter>
                    <Button color='red' className='regular' onClick={this.closeDialog} >CLOSE</Button>
                </ModalFooter>
            </Modal>
		);
	}
}

function mapStateToProps(state) {
    return {
        dialogOpen: state.app.dialogOpen
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        closeDialog,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(LicenceModal);