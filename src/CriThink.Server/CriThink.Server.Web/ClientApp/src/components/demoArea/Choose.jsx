import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { WaveUpDown, WaveDown } from './../Layout'
import { Dropdown, Segment, Icon, Button, Popup } from 'semantic-ui-react'
import './../../fonts/fonts.css'

//Temporary example change values with news
const options = [
  { key: 'angular', text: 'Angular', value: 'angular' },
  { key: 'css', text: 'CSS', value: 'css' },
  { key: 'design', text: 'Graphic Design', value: 'design' },
  { key: 'ember', text: 'Ember', value: 'ember' },
  { key: 'html', text: 'HTML', value: 'html' },
  { key: 'ia', text: 'Information Architecture', value: 'ia' },
  { key: 'javascript', text: 'Javascript', value: 'javascript' },
  { key: 'mech', text: 'Mechanical Engineering', value: 'mech' },
  { key: 'meteor', text: 'Meteor', value: 'meteor' },
  { key: 'node', text: 'NodeJS', value: 'node' },
  { key: 'plumbing', text: 'Plumbing', value: 'plumbing' },
  { key: 'python', text: 'Python', value: 'python' },
  { key: 'rails', text: 'Rails', value: 'rails' },
  { key: 'react', text: 'React', value: 'react' },
  { key: 'repair', text: 'Kitchen Repair', value: 'repair' },
  { key: 'ruby', text: 'Ruby', value: 'ruby' },
  { key: 'ui', text: 'UI Design', value: 'ui' },
  { key: 'ux', text: 'User Experience', value: 'ux' },
]

export class ChooseArea extends Component {
  render () {
    return (
      <div>
	      <WaveDown namePage='Check Your News'>
		      	<Segment basic size='massive'>
		      		<span className='light'>CHOOSE YOUR SOURCE</span>
		      		<Popup content='Add users to your feed' trigger={
		      			<Icon name='info circle' style={{color:'#FF9600', margin:'1rem'}} />
		      		} />	
		      	</Segment>
		      	<Dropdown className='light' placeholder='Skills' fluid selection options={options} />
		      	<Segment basic>
		      		<Link to='/4'><Button style={{backgroundColor:'#FF9600', color:'white'}}><span className='regular'>SEE THE RESULT</span></Button></Link>
		      	</Segment>
	      </WaveDown>
      </div>
    );
  }
}