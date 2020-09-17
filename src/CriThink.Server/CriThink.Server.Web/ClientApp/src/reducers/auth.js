import {
    USER_LOGIN,
    USER_LOGOUT,
} from '../actions/types';

const initialAuthState = {
    username: '',
    userid: '',
    jwtToken: '',
    jwtExp: '',
};

const authreducer = (state = initialAuthState, action) => {
    switch(action.type) {
        case USER_LOGIN:
            return {
                username: action.user.username,
                userid: action.user.userId,
                jwtToken: action.user.token.token,
                jwtExp: action.user.token.expirationDate,
            };
        case USER_LOGOUT:
            return {
                username: '',
                jwtToken: '',
                jwtExp: '',
            };
        default:
            return state;
    }
};

export default authreducer;