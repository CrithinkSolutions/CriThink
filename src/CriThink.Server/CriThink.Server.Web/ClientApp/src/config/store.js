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

    middleware = applyMiddleware(thunk, logger);

    middleware = compose(middleware);

    store = createStore(
        allReducers,
        middleware,
    );

    return store;
};