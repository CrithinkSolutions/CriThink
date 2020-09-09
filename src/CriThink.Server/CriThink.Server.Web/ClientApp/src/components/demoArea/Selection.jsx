import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { WaveUpDown, WaveDown } from './../Layout'
import { Segment, Button, Grid, Card, Label, Image, Icon } from 'semantic-ui-react'
import Book from './../../svg/bookreader.svg';
import Check from './../../svg/check.svg';
import Graduation from './../../svg/graduation.svg';
import LogoRound from './../../svg/logoround.svg';
import './../../custom.css'

export class SelectionArea extends Component {
  render () {
    return (
      <div>
        <WaveDown>
          <Card.Group itemsPerRow={4} style={{marginLeft:'5%', marginRight:'5%', color:'white'}}>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{fontSize:'22px', fontWeight:'800'}}>Check Your News</p>
                <Image src={Book} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  <Link to='/3'><span>Check  <Icon name='long arrow alternate right' /></span></Link>
                </Card.Content>              
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{color:'white', fontSize:'22px', fontWeight:'800'}}>Debunked Fake News</p>
                <Image src={Check} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  Debunk  <Icon name='long arrow alternate right' />
                </Card.Content> 
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{color:'white', fontSize:'22px', fontWeight:'800'}}>Spot Fake News</p>
                <Image src={Graduation} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  Learn  <Icon name='long arrow alternate right' />
                </Card.Content> 
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{color:'white', fontSize:'22px', fontWeight:'800'}}>Quiz & Games</p>
                <Image src={LogoRound} style={{height:'59px'}}/>
                <Card.Content style={{textAlign:'right'}}>
                  Play  <Icon name='long arrow alternate right' />
                </Card.Content> 
              </Card.Content>
            </Card>

          </Card.Group>
        </WaveDown>
      </div>
    );
  }
}
