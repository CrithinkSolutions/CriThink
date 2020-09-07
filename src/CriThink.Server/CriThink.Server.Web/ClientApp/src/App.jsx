import React, { Component } from 'react'
import { Route, Redirect, Switch } from 'react-router'
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
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
import { getUserLogout } from './actions/auth'

import './custom.css'

class App extends Component {
  static displayName = App.name;

  componentDidMount() {
    if (Date.parse(this.props.jwtExp) < Date.now()) {
      this.props.getUserLogout()
    }
  }

  render () {
    return (
        <Layout>
          <Switch>
          	<Route exact path='/' component={Home} />
            <NoAuthRoute authed={this.props.jwtToken} exact path='/login' component={LoginArea} />
            <NoAuthRoute authed={this.props.jwtToken} exact path='/signup' component={SignUpArea} />
            <AuthRoute authed={this.props.jwtToken} exact path='/forgotpassword' component={ForgotPwdArea} />
            <AuthRoute authed={this.props.jwtToken} exact path='/profile' component={ProfileArea} />
            <AuthRoute authed={this.props.jwtToken} exact path='/profile/changepassword' component={ChangePwdArea} />
            <AuthRoute authed={this.props.jwtToken} path='/api/identity/reset-password' component={NewPwdArea} />
            <AuthRoute authed={this.props.jwtToken} path='/backoffice' component={Backoffice} />
            {this.props.dialog}
            <Redirect to='/' />
          </Switch>
        </Layout>
    );
  }
}

function mapStateToProps(state) {
  return {
    dialogOpen: state.app.dialogOpen,
    dialog: state.app.dialog,
    jwtToken: state.auth.jwtToken,
    jwtExp: state.auth.jwtExp
  };
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        getUserLogout
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(App);
