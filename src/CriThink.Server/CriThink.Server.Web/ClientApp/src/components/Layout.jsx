import React, { Component } from 'react';
import { Grid, Segment, Item, Container } from 'semantic-ui-react'
import { ReactComponent as Wave } from './../svg/wave.svg';
import { ReactComponent as Logo } from './../svg/logo.svg';
import './../custom.css'

export class WaveUpDown extends Component {
  render () {
    return (
      <div>
        <Grid className='initgrid' padded celled textAlign='center'>

          <Grid.Row style={{height: '15%'}}>
            <Wave width='100%' height='15vh'/>
          </Grid.Row>

          <Grid.Row style={{height: '70%'}}>
            <Grid.Column>{this.props.children}</Grid.Column>
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
              <span style={{display: 'flex', justifyContent: 'center', alignItems: 'end', paddingBottom: '1rem', paddingTop: '1rem', fontSize:'23pt'}}>
                Before sharing, use the HEAD
              </span>
          </Grid.Row>

          <Grid.Row style={{minHeight: '70%'}} textAlign='center'>
            <Grid.Column>{this.props.children}</Grid.Column>
          </Grid.Row>
          
          <Grid.Row style={{height: '15%'}} className='footer'>
            <Wave className='flipsvg' width='100%' height='15vh'/>
          </Grid.Row>

        </Grid>  
      </div>
    );
  }
}