import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { WaveUpDown, WaveDown } from './Layout'
import { Segment, Button } from 'semantic-ui-react'
import { ReactComponent as Logo } from './../svg/logo.svg';
import './../fonts/fonts.css'
import './../custom.css'

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <WaveUpDown>
        	<Segment basic className='bigfont regular'>WELCOME TO CRITHINK!</Segment>
        	<Logo width='10%'/>
        	<Segment basic className='bigfont bold'>Before sharing, use the HEAD</Segment>
        	<Link to='/2'><Button style={{backgroundColor:'#3C50C8', color:'white'}}><span className='bold'>CONTINUE</span></Button></Link>
        </WaveUpDown>
      </div>
    );
  }
}
