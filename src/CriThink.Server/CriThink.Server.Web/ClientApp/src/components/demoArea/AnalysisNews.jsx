import React, { Component } from 'react';
import { Segment, Grid } from 'semantic-ui-react';

class AnalysisNews extends Component {
    render () {
        return (
            <Grid>
                <Grid.Row>
                    <Grid.Column style={{width:'95%'}}>
                        <Segment basic>{this.props.body}</Segment>
                    </Grid.Column>
                    <Grid.Column style={{width:'5%', padding:'0'}}>
                        {this.props.children}
                    </Grid.Column>
                </Grid.Row>
            </Grid>
        );
    }
}

export default AnalysisNews;