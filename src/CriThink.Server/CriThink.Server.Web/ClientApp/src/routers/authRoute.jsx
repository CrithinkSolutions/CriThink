import React from "react";
import { Route, Redirect } from "react-router-dom";
import AuthHandler from "../handlers/authHandler";

export const AuthRoute = ({
  component: Component,
  ...rest
}) => {
  return (
    <Route
      {...rest}
      render={props => {
        if (AuthHandler.getCurrentUser()) {
          return <Component {...props} />;
        } else {
          return (
            <Redirect
              to={{
                pathname: "/",
                state: {
                  from: props.location
                }
              }}
            />
          );
        }
      }}
    />
  );
};
