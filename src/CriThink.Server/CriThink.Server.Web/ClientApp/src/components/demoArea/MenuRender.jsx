import React, { Component } from 'react';
import { Segment } from 'semantic-ui-react';

class MenuRender extends Component {
    render () {
        const { uppercase } = this.props;
        return (
            <Segment
                color={this.props.color}
                textAlign="left"
                className="noborderafter light"
            >
                <h3 style={{textTransform: uppercase ? 'uppercase' : 'unset'}}>{this.props.header}</h3>
                {this.props.children}
            </Segment>
        );
    }
}

export default MenuRender;