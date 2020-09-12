import React, { Component } from 'react';
import { Segment, Grid } from 'semantic-ui-react';

class AnalysisNews extends Component {
    render () {
        return (
            <Grid padded>
                <Grid.Row column={2}>
                    <Grid.Column width={15}>
                        <Segment basic>{this.props.body}</Segment>
                    </Grid.Column>
                    <Grid.Column>{this.props.children}</Grid.Column>
                </Grid.Row>
            </Grid>
        );
    }
}

export default AnalysisNews;