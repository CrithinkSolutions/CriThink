import React, { Component } from 'react';
import { Button, Form, Grid, Message, Icon } from 'semantic-ui-react'
import axios from 'axios'

export class LoginArea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            signuprender: false,
            loading: false,
            username: '',
            email: '',
            password: '',
            msg: ''
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
        this.setState({loading: true});
        const { username, email, password } = this.state;
        axios.post('/api/identity/login', {
            "username": username,
            "email": email,
            "password": password
        })
        .then(response => {
            this.setState({msg: 
                <Message positive>
                    <Icon name='check' />
                    <b>Welcome {this.state.username}</b>
                </Message>})
        })
        .catch(error => {
            switch(error.response.status) {
              case 500:
                this.setState({msg: 
                    <Message error>
                        <Icon name='warning' />
                        <b>Error: </b>{error.response.data.error}
                    </Message>})
                break;
              case 400:
                this.setState({msg: 
                    <Message error>
                        <Icon name='warning' />
                        <b>Error: </b>{Object.entries(error.response.data.errors)[0][1][0]}
                    </Message>})
                break;
            default:
                console.log(error.response.data)
            } 
        })
        .then(() => {this.setState({loading: false})})
    }

    registerAccount = () => {
        this.setState({loading: true});
        const { username, email, password } = this.state;
        axios.post('/api/identity/sign-up', {
            "username": username,
            "email": email,
            "password": password
        })
        .then(response => {
            this.setState({msg: 
                <Message positive>
                    <Icon name='check' />
                    <b>Check your email {this.state.email}</b>
                </Message>})
        })
        .catch(error => {
            this.setState({msg: 
                <Message error>
                    <Icon name='warning' />
                    <b>Error: </b>{Object.entries(error.response.data.errors)[0][1][0]}
                </Message>
            })
        })
        .then(() => {this.setState({loading: false})})
    }

    render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <div>
                        {this.state.signuprender ? (<h1>Sign Up</h1>) : (<h1>Log In</h1>)}
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
                        {this.state.signuprender ? (
                            <div>
                                <Button content='Sign Up' loading={this.state.loading} primary onClick={this.registerAccount}/>
                                <Message>
                                <p>Have an account? <Button compact color='red' size="small" onClick={this.handleLoginClick}>Log In</Button></p>
                                </Message>
                                {this.state.msg ? this.state.msg : null}
                            </div>
                            ) : (
                            <div>
                                <Button content='Log In' loading={this.state.loading} primary onClick={this.accessAccount}/>
                                <Message>
                                <p>Don't have an account? <Button compact color='red' size="small" onClick={this.handleSignUpClick}>Sign Up</Button></p>
                                </Message>
                                {this.state.msg ? this.state.msg : null}
                            </div>
                        )}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}