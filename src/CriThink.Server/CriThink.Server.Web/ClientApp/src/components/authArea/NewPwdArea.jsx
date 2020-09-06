import React, { Component } from 'react';
import { Button, Form, Grid, Message } from 'semantic-ui-react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { getUserNewPwd } from '../../actions/auth'

const params = (new URL(document.location)).searchParams;

export class NewPwdArea extends Component {

    constructor(props) {
        super(props);
        this.state = {
            newPwd: ''
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
        const { newPwd } = this.state;
        const { userid, jwtToken } = this.props;
        this.props.getUserLogin({
            userid,
            jwtToken,
            newPwd
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
                            <Button content='Send' loading={this.props.loading} primary onClick={this.newPwdAccount}/>
                            <Message>
                                <Message.Header>Remember password must be at least one:</Message.Header>
                                <Message.List>
                                    <Message.Item>8 characters</Message.Item>
                                    <Message.Item>non alphanumeric character</Message.Item>
                                    <Message.Item>digit ('0'-'9')</Message.Item>
                                    <Message.Item>uppercase ('A'-'Z')</Message.Item>
                                </Message.List>
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
        msg: state.app.msg,
        userid: state.auth.userid,
        jwtToken: state.auth.jwtToken
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        getUserNewPwd
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(NewPwdArea);