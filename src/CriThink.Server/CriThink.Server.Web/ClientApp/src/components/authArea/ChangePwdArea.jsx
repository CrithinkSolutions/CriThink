import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Button, Form, Grid, Message, Icon } from 'semantic-ui-react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux';
import { getUserChangePwd } from '../../actions/auth'

class ChangePwdArea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            oldpwd: '',
            newpwd: ''
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
        const { oldpwd, newpwd } = this.state; 
        const { jwtToken } = this.props;
        this.props.getUserChangePwd({
            oldpwd,
            newpwd,
            jwtToken
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
                            <Button content='Send' loading={this.props.loading} primary onClick={this.changePwdAccount}/>
                            <Link to="/profile"><Button content='Go Back'/></Link>
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
        getUserChangePwd
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ChangePwdArea);