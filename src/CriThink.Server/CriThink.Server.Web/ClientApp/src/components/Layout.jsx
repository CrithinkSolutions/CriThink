import React, { Component } from 'react';
import { Grid, Segment, Item, Container } from 'semantic-ui-react'
import { ReactComponent as Wave } from './../svg/wave.svg';
import { ReactComponent as Logo } from './../svg/logo.svg';
import './../custom.css'
import 'bootstrap/dist/css/bootstrap.css';

export class WaveUpDown extends Component {
  render () {
    return (
      <div>
        <Grid className='initgrid' padded celled textAlign='center' verticalAlign='middle'>

          <Grid.Row style={{height: '15%'}}>
            <Wave width='100%' height='15vh'/>
          </Grid.Row>

          <Grid.Row style={{height: '70%'}}>
            <Grid.Column className='p-3'>{this.props.children}</Grid.Column>
          </Grid.Row>
          
          <Grid.Row style={{height: '15%'}} >
            <Wave className='flipsvg' width='100%' height='15vh'/>
          </Grid.Row>

        </Grid>  
      </div>
    );
  }
}


export class WaveDown extends Component {
  render () {
    return (
      <div>
        <Grid celled padded className='initgrid'>

          <Grid.Row className='bottomborder' style={{height: '15%'}}>
              <Logo height='100%' style={{padding: '1rem'}}/>
              <span className='bigfont wavedowntext'>
                Before sharing, use the HEAD
              </span>
          </Grid.Row>

          <Grid.Row style={{minHeight: '70%'}} textAlign='center' verticalAlign='middle'>
            <Grid.Column className='p-5'>{this.props.children}</Grid.Column>
          </Grid.Row>
          
          <Grid.Row style={{height: '15%'}} className='footer'>
            <Wave className='flipsvg' width='100%' height='15vh'/>
          </Grid.Row>

        </Grid>  
      </div>
    );
  }
}