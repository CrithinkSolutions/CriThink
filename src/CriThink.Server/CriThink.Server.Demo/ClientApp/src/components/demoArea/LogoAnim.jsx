import React, { Component } from 'react';
import { Segment, Popup } from 'semantic-ui-react';
import { ReactComponent as Logo } from './../../svg/logoround.svg';

class LogoAnim extends Component {
    render () {
        return (
            <Popup
                content={this.props.msgPopup}
                trigger={
                    <Segment basic>
                        <div className="flip-card">
                            <div className="flip-card-inner">
                                <div className="flip-card-front">
                                    <Logo />
                                </div>
                                <div className="flip-card-back">
                                    <span>{this.props.letter}</span>
                                </div>
                            </div>
                        </div>
                    </Segment>
                }
            />
        );
    }
}

export default LogoAnim;