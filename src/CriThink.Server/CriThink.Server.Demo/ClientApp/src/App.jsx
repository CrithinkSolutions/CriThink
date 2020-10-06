import React, { Component } from 'react';
import { Route, Redirect, Switch } from 'react-router';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import Home from './components/Home';
import LoginArea from './components/authArea/LoginArea';
import SignUpArea from './components/authArea/SignUpArea';
import ForgotPwdArea from './components/authArea/ForgotPwdArea';
import ProfileArea from './components/authArea/ProfileArea';
import ChangePwdArea from './components/authArea/ChangePwdArea';
import NewPwdArea from './components/authArea/NewPwdArea';
import AuthRoute from './routers/AuthRoute';
import Backoffice from './views/Backoffice';
import { getUserLogout, getEnabledRoutes } from './actions/auth';
import { SelectionArea } from './components/demoArea/Selection';
import ChooseArea from './components/demoArea/Choose';
import AnalysisArea from './components/demoArea/Analysis';

class App extends Component {
  static displayName = App.name;

  componentDidMount () {
      this.props.getEnabledRoutes();
      if (Date.parse(this.props.jwtExp) < Date.now()) {
          this.props.getUserLogout();
      }
  }

  render () {
      const { enableLogin } = this.props;
      return (
          <div>
              <Switch>
                  <Route exact path="/" component={Home} />
                  <Route path="/menu" component={SelectionArea} />
                  <Route path="/source-selection" component={ChooseArea} />
                  <Route path="/analysis" component={AnalysisArea} />
                  <AuthRoute exact path='/login' component={LoginArea} anonymous disabled={!enableLogin} />
                  <AuthRoute exact path='/signup' component={SignUpArea} anonymous disabled={!enableLogin} />
                  <AuthRoute exact path='/forgotpassword' component={ForgotPwdArea} anonymous disabled={!enableLogin} />
                  <AuthRoute exact path='/profile' component={ProfileArea} />
                  <AuthRoute exact path='/profile/changepassword' component={ChangePwdArea} />
                  <AuthRoute path='/api/identity/reset-password' component={NewPwdArea} disabled={!enableLogin} />
                  <AuthRoute path='/backoffice' component={Backoffice} />
                  <Redirect to="/" />
              </Switch>
              {this.props.dialog}
          </div>
      );
  }
}

function mapStateToProps (state) {
    return {
        dialogOpen: state.app.dialogOpen,
        dialog: state.app.dialog,
        jwtToken: state.auth.jwtToken,
        jwtExp: state.auth.jwtExp,
        enableLogin: state.app.loginRoutesEnabled,
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        getEnabledRoutes,
        getUserLogout,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(App);
