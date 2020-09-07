import React, { Component } from 'react';
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
                <Image circular src={Book} style={{height:'59px'}}/>
                <p style={{fontSize:'12px', margin:'50px'}}>Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                <Card.Content style={{textAlign:'right'}}>
                  Check  <Icon name='long arrow alternate right' />
                </Card.Content>              
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{color:'white', fontSize:'22px', fontWeight:'800'}}>Debunked Fake News</p>
                <Image circular src={Check} style={{height:'59px'}}/>
                <p style={{color:'white', fontSize:'12px', margin:'50px'}}>Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                <Card.Content style={{textAlign:'right'}}>
                  Debunk  <Icon name='long arrow alternate right' />
                </Card.Content> 
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{color:'white', fontSize:'22px', fontWeight:'800'}}>Spot Fake News</p>
                <Image circular src={Graduation} style={{height:'59px'}}/>
                <p style={{color:'white', fontSize:'12px', margin:'50px'}}>Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                <Card.Content style={{textAlign:'right'}}>
                  Learn  <Icon name='long arrow alternate right' />
                </Card.Content> 
              </Card.Content>
            </Card>

            <Card style={{backgroundColor:'#3C50C8'}}>
              <Card.Content>
                <p style={{color:'white', fontSize:'22px', fontWeight:'800'}}>Quiz & Games</p>
                <Image circular src={LogoRound} style={{height:'59px'}}/>
                <p style={{color:'white', fontSize:'12px', margin:'50px'}}>Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
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
