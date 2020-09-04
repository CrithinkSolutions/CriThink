import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon } from 'semantic-ui-react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { getUserForgotPwd } from '../../actions/auth'

class ForgotPwdArea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            email: '',
        };
    }

    changeHandler = event => {
        if(event.target.name === "username" && event.target.value.includes("@")) {
            this.setState({email: event.target.value})
        }
        else {
            this.setState({
                email: '',
                username: event.target.value
            })
        }
    }

    getPwdAccount = () => {
        const { username, email } = this.state;
        this.props.getUserForgotPwd({
            username,
            email
        })
    }

    render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <h1>Forgot Password</h1>
                        <br/>
                        <Form>
                            <Form.Input
                                icon='user'
                                iconPosition='left'
                                label='Username or Email'
                                placeholder='Username or Email'
                                name='username'
                                onChange={this.changeHandler}
                            />
                        </Form>
                        <br/>
                        <div id="options">
                            <Button content='Send' loading={this.props.loading} primary onClick={this.getPwdAccount}/>
                            <Link to="/login"><Button content='Go Back'/></Link>
                            {this.props.msg ? this.props.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}

function mapStateToProps(state) {
    return {
        loading: !!state.app.loading.find(x => x.label === 'userLogin'),
        msg: state.app.msg
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        getUserForgotPwd
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ForgotPwdArea);