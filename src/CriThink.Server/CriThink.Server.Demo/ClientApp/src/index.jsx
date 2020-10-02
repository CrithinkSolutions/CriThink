import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import 'semantic-ui-css/semantic.min.css';
import { Provider } from 'react-redux';
import createStore from './config/store';
import { persistStore } from 'redux-persist';
import { PersistGate } from 'redux-persist/integration/react';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

const store = createStore();

const persistor = persistStore(store, null);

ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <Provider store={ store }>
            <PersistGate persistor={ persistor }>
                <App />
            </PersistGate>
        </Provider>
    </BrowserRouter>,
    rootElement);

registerServiceWorker();

