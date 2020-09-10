import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { WaveUpDown, WaveDown } from './Layout'
import { Segment, Button } from 'semantic-ui-react'
import { ReactComponent as Logo } from './../svg/logo.svg';
import { openCustomDialog } from '../actions/app';
import LicenceModal from '../components/modals/LicenceModal';
import './../fonts/fonts.css'
import './../custom.css'

class Home extends Component {
  static displayName = Home.name;

  openModal = () => {
        this.props.openCustomDialog(<LicenceModal />);
  }

  render () {
    return (
      <div>
        <WaveUpDown>
        	<Segment basic className='bigfont regular'>WELCOME TO CRITHINK!</Segment>
        	<Logo width='10%'/>
        	<Segment basic className='bigfont bold'>Before sharing, use the HEAD</Segment>
        	<Link to='/2'><Button style={{backgroundColor:'#3C50C8', color:'white'}}><span className='bold'>CONTINUE</span></Button></Link>
          <Segment basic className='link regular' style={{fontSize:'8pt'}} onClick={this.openModal}>LICENCE</Segment>
        </WaveUpDown>
      </div>
    );
  }
}


function mapDispatchToProps(dispatch) {
    return bindActionCreators({
        openCustomDialog
    }, dispatch);
}

export default connect(null, mapDispatchToProps)(Home);