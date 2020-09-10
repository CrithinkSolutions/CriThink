import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { WaveUpDown, WaveDown } from './../Layout'
import { Segment, Button, Grid, Card, Label, Image, Icon, Popup, Item } from 'semantic-ui-react'
import Book from './../../svg/bookreader.svg';
import Check from './../../svg/check.svg';
import Graduation from './../../svg/graduation.svg';
import LogoRound from './../../svg/logoround.svg';
import './../../fonts/fonts.css'
import './../../custom.css'

export class SelectionArea extends Component {
  render () {
    return (
      <div>
        <WaveDown>
          <Card.Group itemsPerRow={4} style={{marginLeft:'5%', marginRight:'5%', color:'white'}}>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p className='regular' style={{fontSize:'18px'}}>Check Your News</p>
                <Image src={Book} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  <Link to='/3'><span className='whitelink bold'>CHECK  <Icon name='long arrow alternate right' /></span></Link>
                </Card.Content>              
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p className='regular' style={{fontSize:'18px'}}>Debunked Fake News</p>
                <Image src={Check} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  <Popup content='Section under construction...' trigger={<span className='nolink bold'>DEBUNK  <Icon name='long arrow alternate right' /></span>}></Popup>
                </Card.Content> 
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p className='regular' style={{fontSize:'18px'}}>Spot Fake News</p>
                <Image src={Graduation} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  <Popup content='Section under construction...' trigger={<span className='nolink bold'>LEARN  <Icon name='long arrow alternate right' /></span>}></Popup>
                </Card.Content> 
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p className='regular' style={{fontSize:'18px'}}>Quiz & Games</p>
                <Image src={LogoRound} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  <Popup content='Section under construction...' trigger={<span className='nolink bold'>PLAY  <Icon name='long arrow alternate right' /></span>}></Popup>
                </Card.Content> 
              </Card.Content>
            </Card>
          </Card.Group>
        </WaveDown>
      </div>
    );
  }
}
