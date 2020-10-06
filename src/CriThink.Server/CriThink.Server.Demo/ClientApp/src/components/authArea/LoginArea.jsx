import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Form, Grid, Message, Divider } from 'semantic-ui-react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getUserLogin } from '../../actions/auth';

class LoginArea extends Component {
    constructor (props) {
        super(props);
        this.state = {
            username: '',
            email: '',
            password: '',
        };
    }

    changeHandler = event => {
        if(event.target.name === 'username' && event.target.value.includes('@')) {
            this.setState({email: event.target.value});
        }
        else if (event.target.name === 'password') {
            this.setState({password: event.target.value});
        }
        else {
            this.setState({
                email: '',
                username: event.target.value,
            });
        }
    }

    accessAccount = () => {
        const { username, email, password } = this.state;
        this.props.getUserLogin({
            username,
            email,
            password,
        });
    }

    render () {
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
                                label='Username or Email'
                                placeholder='Username or Email'
                                name='username'
                                onChange={this.changeHandler}
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
                            <Button content='Log In' loading={this.props.loading} primary onClick={this.accessAccount}/>
                            <Divider />
                            <Link to="/forgotpassword"><span className="link">Forgot password?</span></Link>
                            <Message>
                                <p>Don't have an account? <Link to="/signup"><Button compact color='red' size="small">Sign Up</Button></Link></p>
                            </Message>
                            {this.props.msg ? this.props.msg : null}
                        </div>
                    </Grid.Column>
                </Grid>
            </div>
        );
    }
}

function mapStateToProps (state) {
    return {
        loading: !!state.app.loading.find(x => x.label === 'userLogin'),
        msg: state.app.msg,
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        getUserLogin,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(LoginArea);