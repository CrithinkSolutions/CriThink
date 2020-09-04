import { combineReducers } from 'redux';
import app from './app';
import backoffice from './backoffice';
import auth from './auth';

export default combineReducers({
    app,
    backoffice,
    auth
});
