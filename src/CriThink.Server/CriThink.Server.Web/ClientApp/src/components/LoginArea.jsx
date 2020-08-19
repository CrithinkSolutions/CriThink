import React, { Component } from 'react';
import { Button, Form, Grid } from 'semantic-ui-react'
import axios from 'axios'

export class LoginArea extends Component {
    constructor(props) {
        super(props);
        this.handleLoginClick = this.handleLoginClick.bind(this);
        this.handleSignUpClick = this.handleSignUpClick.bind(this);
        this.changeHandler = this.changeHandler.bind(this)
        this.state = {
            signuprender: false,
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
    

    handleLoginClick = () => {
        this.setState({signuprender: false});
    }

    handleSignUpClick = () => {
        this.setState({signuprender: true});
    }

    accessAccount = () => {
        const { username, email, password } = this.state;
        axios.post('http://crithink-staging.eba-msmbrpmt.eu-central-1.elasticbeanstalk.com/api/identity/login', {
            "username": username,
            "email": email,
            "password": password
        })
        .then(function (response) {
            console.log(response);
        })
        .catch(function (error) {
            console.log(error);
        })
    }

    render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <div>
                        <h1>Login</h1>
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
                        <Button content='Login' primary onClick={this.accessAccount}/>
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}

