import { combineReducers } from 'redux';
import storage from 'redux-persist/lib/storage';
import { persistReducer } from 'redux-persist';
import app from './app';
import backoffice from './backoffice';
import auth from './auth';
import demo from './demo';

const authConfig = {
    key: 'auth',
    storage,
    blacklist: ['loggedIn'],
};

const appConfig = {
    key: 'app',
    storage,
    whitelist: ['loginRoutesEnabled'],
};

export default combineReducers({
    app: persistReducer(appConfig, app),
    backoffice,
    auth: persistReducer(authConfig, auth),
    demo,
});
