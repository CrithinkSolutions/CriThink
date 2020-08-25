import React, { Component } from 'react';
import { Button, Form, Grid, Message, Icon } from 'semantic-ui-react'
import AuthHandler from "../../handlers/authHandler";

const params = (new URL(document.location)).searchParams;

export class NewPwdArea extends Component {

    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            newPwd: '',
            msg: ''
        };
    }

    changeHandler = event => {
        this.setState({
          [event.target.name]: event.target.value
        });
      }

    componentDidMount() { 
        if (!params.get("userId") || !params.get("code") || params.get("code").length !== 320) {
            this.props.history.push("/login");
        }
    }

    newPwdAccount = () => {
        this.setState({loading: true})
        const { newPwd } = this.state;
        AuthHandler
            .newPwd(params.get("userId"), params.get("code"), newPwd)

            .then((res) => {
                this.setState({msg: 
                    <Message positive>
                        <Icon name='check' />
                        <b>Password changed</b>
                    </Message>
                })
            })

            .catch(err => {
                this.setState({msg: 
                    <Message error>
                        <Icon name='warning' />
                        <b>Error: </b>{err}
                    </Message>
                })
            })

        .then(() => {
            this.setState({loading: false})
            setTimeout(() => {
                this.props.history.push("/login")
            }, 3000);
        })

    }

    render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <h1>Set your new Password</h1>
                        <br/>
                        <Form>
                            <Form.Input
                                icon='lock'
                                iconPosition='left'
                                label='New Password'
                                name='newPwd'
                                type='password'
                                onChange={this.changeHandler}
                                value={this.state.newPwd}
                            />
                        </Form>
                        <br/>
                        <div id="options">
                            <Button content='Send' loading={this.state.loading} primary onClick={this.newPwdAccount}/>
                            <Message>
                                <Message.Header>Remember password must be at least one:</Message.Header>
                                <Message.List>
                                    <Message.Item>8 characters</Message.Item>
                                    <Message.Item>non alphanumeric character</Message.Item>
                                    <Message.Item>digit ('0'-'9')</Message.Item>
                                    <Message.Item>uppercase ('A'-'Z')</Message.Item>
                                </Message.List>
                            </Message>
                            {this.state.msg ? this.state.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}