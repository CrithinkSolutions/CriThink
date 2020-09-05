import React, { Component } from 'react'
import { Route, Redirect } from 'react-router'

const AuthRoute = ({ component: Component, authed, ...rest }) => (
  <Route
    {...rest}
    render={props => (
      authed
        ? <Component {...props} />
        : <Redirect to="/" />
    )}
  />
);

const NoAuthRoute = ({ component: Component, authed, ...rest }) => (
  <Route
    {...rest}
    render={props => (
      !authed
        ? <Component {...props} />
        : <Redirect to="/" />
    )}
  />
);

export {
  AuthRoute,
  NoAuthRoute
}