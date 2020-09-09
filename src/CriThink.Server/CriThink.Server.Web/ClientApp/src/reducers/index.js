import { combineReducers } from 'redux';
import storage from 'redux-persist/lib/storage';
import { persistReducer } from 'redux-persist'
import app from './app';
import backoffice from './backoffice';
import auth from './auth';
import demo from './demo';

const authConfig = {
    key: 'auth',
    storage,
    blacklist: ['loggedIn'],
};

export default combineReducers({
    app,
    backoffice,
    auth: persistReducer(authConfig, auth),
    demo
});
