import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Login } from './components/Login';
import { SignUp } from './components/SignUp';
import Agenda from './components/Agenda';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Login} />
        <Route path='/SignUp' component={SignUp} />
        <Route path='/Agenda' component={Agenda} />
      </Layout>
    );
  }
}
