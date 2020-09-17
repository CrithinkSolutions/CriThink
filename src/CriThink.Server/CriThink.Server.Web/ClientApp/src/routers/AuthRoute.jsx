import React, { Component } from 'react';
import { Route, Redirect } from 'react-router';
import { connect } from 'react-redux';

class AuthRoute extends Component {
    render () {
        const { anonymous, isLoggedIn, disabled, ...rest } = this.props;
        let { component: DestinationComponent } = this.props;

        let redirect = false;

        if ((!!anonymous && isLoggedIn)
            || (!anonymous && !isLoggedIn)
            || !!disabled) {
            redirect = true;
        }

        return redirect
            ? <Redirect to='/' />
            : <Route {...rest} render={props => <DestinationComponent {...props} />} />;
    }
}

function mapStateToProps (state) {
    return {
        isLoggedIn: !!state.auth.jwtToken,
    };
}

export default connect(mapStateToProps)(AuthRoute);