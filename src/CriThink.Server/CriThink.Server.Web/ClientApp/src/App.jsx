import React, { Component } from 'react'
import { Route } from 'react-router'
import { Layout } from './components/Layout'
import { Home } from './components/Home'
import { LoginArea } from './components/authArea/LoginArea'
import { SignUpArea } from './components/authArea/SignUpArea'
import { ForgotPwdArea } from './components/authArea/ForgotPwdArea'
import { ProfileArea } from './components/authArea/ProfileArea'
import { ChangePwdArea } from './components/authArea/ChangePwdArea'
import { NewPwdArea } from './components/authArea/NewPwdArea'
import { AuthRoute } from './routers/authRoute'
import { NoAuthRoute } from './routers/noauthRoute'

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
      	<Route exact path='/' component={Home} />
        <NoAuthRoute exact path='/login' component={LoginArea} />
        <NoAuthRoute exact path='/signup' component={SignUpArea} />
        <NoAuthRoute exact path='/forgotpassword' component={ForgotPwdArea} />
        <AuthRoute exact path='/profile' component={ProfileArea} />
        <AuthRoute exact path='/profile/changepassword' component={ChangePwdArea} />
        <NoAuthRoute path='/api/identity/reset-password' component={NewPwdArea} />
      </Layout>
    );
  }
}
