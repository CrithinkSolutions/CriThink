import React, { Component } from 'react';
import { WaveUpDown, WaveDown } from './Layout'
import { Grid, Segment } from 'semantic-ui-react'

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <WaveDown>
        </WaveDown>
      </div>
    );
  }
}
