import React, { Component } from 'react'
import { Route, Redirect } from 'react-router'
import { connect } from 'react-redux';
import { Layout } from './components/Layout'
import { Home } from './components/Home'
import LoginArea from './components/authArea/LoginArea'
import SignUpArea from './components/authArea/SignUpArea'
import ForgotPwdArea from './components/authArea/ForgotPwdArea'
import ProfileArea from './components/authArea/ProfileArea'
import ChangePwdArea from './components/authArea/ChangePwdArea'
import NewPwdArea from './components/authArea/NewPwdArea'
import { NoAuthRoute, AuthRoute } from './routers/authRoute'
import Backoffice from './views/Backoffice';

import './custom.css'

class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
      	<Route exact path='/' component={Home} />
        <NoAuthRoute exact path='/login' component={LoginArea} />
        <AuthRoute authed={this.props.jwtToken} exact path='/signup' component={SignUpArea} />
        <AuthRoute authed={this.props.jwtToken} exact path='/forgotpassword' component={ForgotPwdArea} />
        <AuthRoute authed={this.props.jwtToken} exact path='/profile' component={ProfileArea} />
        <AuthRoute authed={this.props.jwtToken} exact path='/profile/changepassword' component={ChangePwdArea} />
        <AuthRoute authed={this.props.jwtToken} path='/api/identity/reset-password' component={NewPwdArea} />
        <AuthRoute authed={this.props.jwtToken} path='/backoffice' component={Backoffice} />
        {this.props.dialog}
        <Redirect from='*' to='/'/>
      </Layout>
    );
  }
}

function mapStateToProps(state) {
  return {
    dialogOpen: state.app.dialogOpen,
    dialog: state.app.dialog,
    jwtToken: state.auth.jwtToken
  };
}

export default connect(mapStateToProps)(App);
