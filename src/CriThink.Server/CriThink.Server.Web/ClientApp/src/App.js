import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { LoginArea } from './components/Home_2';
import { Counter } from './components/Counter';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/react' component={LoginArea} />
        <Route path='/counter' component={Counter} />
      </Layout>
    );
  }
}
