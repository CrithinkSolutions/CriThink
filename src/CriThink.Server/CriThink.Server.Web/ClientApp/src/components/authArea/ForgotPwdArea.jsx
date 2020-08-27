import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon } from 'semantic-ui-react'
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
        this.setState({loading: true})
        const { username, email } = this.state;
        AuthHandler
            .forgotPwd(username,email)

            .then((res) => {
                this.setState({msg: 
                    <Message positive>
                        <Icon name='check' />
                        <b>Check your email</b>
                    </Message>
                })
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
                            <Button content='Send' loading={this.state.loading} primary onClick={this.getPwdAccount}/>
                            <Link to="/login"><Button content='Go Back'/></Link>
                            {this.state.msg ? this.state.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}