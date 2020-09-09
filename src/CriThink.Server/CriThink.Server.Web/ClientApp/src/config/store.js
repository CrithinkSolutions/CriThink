import allReducers from '../reducers';
import thunk from 'redux-thunk';
import logger from 'redux-logger';
import {
    applyMiddleware,
    compose,
    createStore,
} from 'redux';

export default () => {
    let store = null;
    let middleware = null;

    if (process.env.PRODUCTION === true) {
        middleware = applyMiddleware(thunk);
    }
    else {
        middleware = applyMiddleware(thunk, logger);
    }

    middleware = compose(middleware);

    store = createStore(
        allReducers,
        middleware,
    );

    return store;
};