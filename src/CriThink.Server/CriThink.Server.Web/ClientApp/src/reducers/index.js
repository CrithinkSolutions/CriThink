import { combineReducers } from 'redux';
import app from './app';
import backoffice from './backoffice';

export default combineReducers({
    app,
    backoffice,
});
