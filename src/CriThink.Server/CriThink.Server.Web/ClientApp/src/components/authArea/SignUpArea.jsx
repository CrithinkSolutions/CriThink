import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon } from 'semantic-ui-react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { getUserRegister } from '../../actions/auth'

class SignUpArea extends Component {
	constructor(props) {
        super(props);
        this.state = {
            username: '',
            email: '',
            password: ''
        };
    }

    changeHandler = event => {
        this.setState({
          [event.target.name]: event.target.value
        });
    }

	registerAccount = () => {
        const { username, email, password } = this.state;
        this.props.getUserRegister({
            username,
            email,
            password
        })
    }

	render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <h1>Sign Up</h1>
                        <br/>
                        <Form>
                            <Form.Input
                                icon='user'
                                iconPosition='left'
                                label='Username'
                                placeholder='Username'
                                name='username'
                                onChange={this.changeHandler}
                                value={this.state.username}
                            />
                            <Form.Input
                                icon='mail'
                                iconPosition='left'
                                label='Email'
                                placeholder='Email'
                                name='email'
                                onChange={this.changeHandler}
                                value={this.state.email}
                            />
                            <Form.Input
                                icon='lock'
                                iconPosition='left'
                                label='Password'
                                type='password'
                                name='password'
                                onChange={this.changeHandler}
                                value={this.state.password}
                            />
                        </Form>
                        <br/>
                        <div id="options">
                            <Button content='Sign Up' loading={this.props.loading} primary onClick={this.registerAccount}/>
                            <Message>
                            <p>Have an account? <Link to="/login"><Button compact color='red' size="small">Log In</Button></Link></p>
                            </Message>
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
        getUserRegister
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(SignUpArea);