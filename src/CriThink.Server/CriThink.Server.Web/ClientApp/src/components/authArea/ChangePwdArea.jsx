import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon, Divider } from 'semantic-ui-react'
import AuthHandler from "../../handlers/authHandler";

export class ChangePwdArea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            oldpwd: '',
            newpwd: '',
            user: AuthHandler.getCurrentUser(),
            msg: ''
        };
    }

    changeHandler = event => {
        this.setState({
          [event.target.name]: event.target.value
        });
      }

    componentDidMount() {
        if (!this.state.user) {
            this.props.history.push("/profile");
        }
    }

    changePwdAccount = () => {
        this.setState({loading: true,})
        const { oldpwd, newpwd, user } = this.state;
        AuthHandler
            .changePwd(oldpwd, newpwd, user.jwtToken.token)

            .then((res) => {
                this.setState({msg: 
                    <Message positive>
                        <Icon name='check' />
                        <b>{res}</b>
                    </Message>
                })
                setTimeout(() => {this.props.history.push("/profile")}, 3000);
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
        })

    }

    render() {
        return (
            <div id="mainrender">
                <Grid id="input" verticalAlign='middle' textAlign="center" style={{height: '85vh'}}>
                    <Grid.Column width={8}>
                        <h1>Change Password</h1>
                        <br/>
                        <Form>
                            <Form.Input
                                icon='lock'
                                iconPosition='left'
                                label='Old Password'
                                name='oldpwd'
                                type='password'
                                onChange={this.changeHandler}
                                value={this.state.oldpwd}
                            />
                            <Form.Input
                                icon='lock'
                                iconPosition='left'
                                label='New Password'
                                name='newpwd'
                                type='password'
                                onChange={this.changeHandler}
                                value={this.state.newpwd}
                            />
                        </Form>
                        <br/>
                        <div id="options">
                            <Button content='Send' loading={this.state.loading} primary onClick={this.changePwdAccount}/>
                            <Link to="/profile"><Button content='Go Back'/></Link>
                            {this.state.msg ? this.state.msg : null}
                        </div>
                    </Grid.Column>
                </Grid> 
            </div>
        );
    }
}