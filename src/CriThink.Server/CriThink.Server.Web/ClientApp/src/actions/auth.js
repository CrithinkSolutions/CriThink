import * as types from './types';
import debounceAction from '../lib/debounceAction';
import axios from 'axios';
import { apiRequest, apiResponse, apiError, apiSuccess } from './api';
import { newActionId } from '../lib/utils';

function userLogin (user) {
    return {
        type: types.USER_LOGIN,
        user,
    };
}

function userLogout (user) {
    return {
        type: types.USER_LOGOUT,
        user,
    };
}

function toDebounceGetUserLogin ({username, email, password}) {
    return (dispatch) => {
        const actionId = newActionId('Login into user session', 'userLogin');
        dispatch(apiRequest(actionId));
        axios.post('/api/identity/login', {
            username,
            email,
            password,
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(userLogin(res.data.result));
                    dispatch(apiSuccess('Welcome'));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiError('Error'));
                dispatch(apiResponse(actionId));
            });
    };
}

const getUserLogin = debounceAction(toDebounceGetUserLogin, 1000, { leading: true, trailing: false });

function getUserLogout () {
    return (dispatch) => {
        const actionId = newActionId('Logout user session', 'userLogout');
        dispatch(apiRequest(actionId));
        dispatch(userLogout());
    };
}

function toDebounceGetUserRegister ({username, email, password}) {
    return (dispatch) => {
        const actionId = newActionId('Register new user', 'userRegister');
        dispatch(apiRequest(actionId));
        axios.post('/api/identity/sign-up', {
            username,
            email,
            password,
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiSuccess('Check your email'));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiError('Error'));
                dispatch(apiResponse(actionId));
            });
    };
}

const getUserRegister = debounceAction(toDebounceGetUserRegister, 1000, { leading: true, trailing: false });

function toDebounceGetUserForgotPwd ({username, email}) {
    return (dispatch) => {
        const actionId = newActionId('Send email for new password', 'userForgotPwd');
        dispatch(apiRequest(actionId));
        axios.post('/api/identity/forgot-password', {
            username,
            email,
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiSuccess('Check your email'));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiError('Error'));
                dispatch(apiResponse(actionId));
            });
    };
}

const getUserForgotPwd = debounceAction(toDebounceGetUserForgotPwd, 1000, { leading: true, trailing: false });

function toDebounceGetUserChangePwd ({currentPassword, newPassword, jwtToken}) {
    return (dispatch) => {
        const actionId = newActionId('Changing the user password', 'userChangePwd');
        dispatch(apiRequest(actionId));
        axios.post('/api/identity/change-password', {
            currentPassword,
            newPassword,
        }, {
            headers: {
                'Authorization': `Bearer ${ jwtToken }`,
            },
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiSuccess('Password changed'));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiError('Error'));
                dispatch(apiResponse(actionId));
            });
    };
}

const getUserChangePwd = debounceAction(toDebounceGetUserChangePwd, 1000, { leading: true, trailing: false });

function toDebounceGetUserNewPwd ({userId, token, newPassword}) {
    return (dispatch) => {
        const actionId = newActionId('Reset user password', 'userNewPwd');
        dispatch(apiRequest(actionId));
        axios.post('/api/identity/reset-password', {
            userId,
            token,
            newPassword,
        })
            .then(res => {
                if(res.status === 200) {
                    dispatch(apiSuccess('Password changed'));
                    dispatch(apiResponse(actionId));
                }
            })
            .catch(err => {
                dispatch(apiError('Error'));
                dispatch(apiResponse(actionId));
            });
    };
}

const getUserNewPwd = debounceAction(toDebounceGetUserNewPwd, 1000, { leading: true, trailing: false });

export {
    getUserLogin,
    getUserLogout,
    getUserRegister,
    getUserForgotPwd,
    getUserChangePwd,
    getUserNewPwd,
};