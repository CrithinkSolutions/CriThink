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

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
      	<Route exact path='/' component={Home} />
        <Route exact path='/login' component={LoginArea} />
        <Route exact path='/signup' component={SignUpArea} />
        <Route exact path='/forgotpassword' component={ForgotPwdArea} />
        <Route exact path='/profile' component={ProfileArea} />
        <Route exact path='/profile/changepassword' component={ChangePwdArea} />
        <Route path='/api/identity/reset-password' component={NewPwdArea} />
      </Layout>
    );
  }
}
