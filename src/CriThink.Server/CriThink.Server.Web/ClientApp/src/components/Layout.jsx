import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Grid, Segment, Item, Container, Image } from 'semantic-ui-react'
import { ReactComponent as Wave } from './../svg/wave.svg';
import Logo from './../svg/logo.svg';
import './../fonts/fonts.css'
import './../custom.css'

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
              <Link to='/'><Image src={Logo} height='140vw' style={{padding: '1rem'}}/></Link>
              <span className='bigfont wavedowntext bold'>
                Before sharing, use the HEAD
              </span>
              {this.props.namePage ?
                <span className='bigfont wavedowntext bottombordergradient bold' style={{marginLeft:'auto', marginRight:'1.5em', alignItems:'center'}}>
                  {this.props.namePage}
                </span> : null
              }
          </Grid.Row>

          <Grid.Row style={{minHeight: '70%'}} textAlign='center' verticalAlign='middle'>
            <Grid.Column style={{padding:'0 20rem'}}>{this.props.children}</Grid.Column>
          </Grid.Row>
          
          <Grid.Row style={{height: '15%'}} className='footer'>
            <Wave className='flipsvg' width='100%' height='15vh'/>
          </Grid.Row>

        </Grid>  
      </div>
    );
  }
}