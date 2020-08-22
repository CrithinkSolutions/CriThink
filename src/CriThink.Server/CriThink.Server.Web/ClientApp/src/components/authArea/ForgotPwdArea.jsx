import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon, Divider } from 'semantic-ui-react'
import AuthHandler from "../../handlers/authHandler";

export class ForgotPwdArea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            username: '',
            email: '',
            msg: ''
        };
    }

    changeHandler = event => {
        this.setState({
          [event.target.name]: event.target.value
        });
      }

    getPwdAccount = () => {
        this.setState({loading: true})
        const { username, email } = this.state;
        AuthHandler
            .forgotPwd(username,email)

            .then((res) => {
                this.setState({msg: 
                    <Message positive>
                        <Icon name='check' />
                        <b>Check your email {this.state.email}</b>
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
            setTimeout(window.location.reload.bind(window.location), 250);
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
                        </Form>
                        <br/>
                        <div id="options">
                            <Button content='Send' loading={this.state.loading} primary onClick={this.getPwdAccount}/>
                            {this.state.msg ? this.state.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}