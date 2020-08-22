import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon, Divider } from 'semantic-ui-react'
import AuthHandler from "../../handlers/authHandler";

export class LoginArea extends Component {
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

    accessAccount = () => {
        this.setState({loading: true})
        const { username, email, password } = this.state;
        AuthHandler
            .login(username,email,password)

            .then((res) => {
                this.setState({msg: 
                    <Message positive>
                        <Icon name='check' />
                        <b>Welcome {this.state.username}</b>
                    </Message>
                })
                console.log(res)
            })

            .catch(err => 
                this.setState({msg: 
                    <Message error>
                        <Icon name='warning' />
                        <b>Error: </b>{err}
                    </Message>
                })
            )

        .then(() => {
            this.setState({loading: false})
            setTimeout(window.location.reload.bind(window.location), 2000);
        })
    }

    render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <h1>Log In</h1>
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
                            <Button content='Log In' loading={this.state.loading} primary onClick={this.accessAccount}/>
                            <Divider />
                            <Link to="/forgotpassword"><span className="link">Forgot password?</span></Link>
                            <Message>
                            <p>Don't have an account? <Link to="/signup"><Button compact color='red' size="small">Sign Up</Button></Link></p>
                            </Message>
                            {this.state.msg ? this.state.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}