import {
	USER_LOGIN,
	USER_LOGOUT
} from '../actions/types';

const initialAuthState = {
    username: '',
    userid: '',
    jwtToken: '',
    jwtExp: ''
};

const authreducer = (state = initialAuthState, action) => {
	switch(action.type) {
		case USER_LOGIN: 
			return {
				username: action.user.userName,
				userid: action.user.userId,
				jwtToken: action.user.jwtToken.token,
				jwtExp: action.user.jwtToken.expirationDate
			};
		case USER_LOGOUT: 
			return {
				username: '',
				jwtToken: '',
				jwtExp: ''
			};
		default:
			return state;
	}
}

export default authreducer;