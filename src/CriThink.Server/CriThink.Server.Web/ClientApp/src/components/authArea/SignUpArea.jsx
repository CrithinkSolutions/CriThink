import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon, Divider } from 'semantic-ui-react'
import AuthHandler from "../../handlers/authHandler";

export class SignUpArea extends Component {
	constructor(props) {
        super(props);
        this.state = {
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

	registerAccount = () => {
        this.setState({loading: true});
        const { username, email, password } = this.state;
        AuthHandler
            .register(username,email,password)

            .then((res) => {
                this.setState({msg: 
                    <Message positive>
                        <Icon name='check' />
                        <b>Check your email {this.state.email}</b>
                    </Message>
                })
                console.log(res)
            })

            .catch(err => {
                this.setState({msg: 
                    <Message error>
                        <Icon name='warning' />
                        <b>Error: </b>{err}
                    </Message>
                })
                console.log(err)
            })

        .then(() => {
            this.setState({loading: false})
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
                            <Button content='Sign Up' loading={this.state.loading} primary onClick={this.registerAccount}/>
                            <Message>
                            <p>Have an account? <Link to="/login"><Button compact color='red' size="small">Log In</Button></Link></p>
                            </Message>
                            {this.state.msg ? this.state.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}
