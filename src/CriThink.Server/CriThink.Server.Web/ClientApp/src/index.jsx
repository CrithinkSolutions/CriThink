import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import 'semantic-ui-css/semantic.min.css'
import { Provider } from 'react-redux';
import createStore from './config/store';;

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

const store = createStore();

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <Provider store={ store }>
      <App />
    </Provider>
  </BrowserRouter>,
  rootElement);

registerServiceWorker();

