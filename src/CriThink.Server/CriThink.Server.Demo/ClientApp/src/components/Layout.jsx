import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Grid, Segment } from 'semantic-ui-react';
import { ReactComponent as Wave } from './../svg/wave.svg';
import Logo from './../svg/logo.svg';

export class WaveUpDown extends Component {
    render () {
        return (
            <div>
                <Grid
                    className="initgrid"
                    padded
                    celled
                    textAlign="center"
                    verticalAlign="middle"
                >
                    <Grid.Row style={{ height: '15%' }}>
                        <Wave width="100%" height="15vh" />
                    </Grid.Row>

                    <Grid.Row style={{ height: '70%' }}>
                        <Grid.Column className="p-3">
                            {this.props.children}
                        </Grid.Column>
                    </Grid.Row>

                    <Grid.Row style={{ height: '15%' }}>
                        <Wave className="flipsvg" width="100%" height="15vh" />
                    </Grid.Row>
                </Grid>
            </div>
        );
    }
}

export class WaveDown extends Component {
    render () {
        return (
            <div>
                <Grid celled padded className="initgrid">
                    <Grid.Row
                        column="equal"
                        className="bottomborder"
                        style={{ height: '15%', padding: '0.5rem' }}
                    >
                        <Segment
                            as={Link}
                            basic
                            style={{
                                width: '5%',
                                background: 'url(' + Logo + ') 0% 0% no-repeat',
                                backgroundPosition: 'center',
                                marginRight: '1%',
                            }}
                            to='/'
                        ></Segment>
                        <Segment
                            basic
                            style={{ width: '74%' }}
                            className="bigfont bold endtext"
                        >
                            Before sharing, use the HEAD
                        </Segment>
                        <Segment
                            basic
                            style={{ width: '20%' }}
                            className="bigfont bold middletext"
                        >
                            {this.props.namePage && (
                                <p className="bottombordergradient">
                                    Check your news
                                </p>
                            )}
                        </Segment>
                    </Grid.Row>

                    <Grid.Row
                        style={{ minHeight: '70%' }}
                        textAlign="center"
                        verticalAlign="middle"
                    >
                        <Grid.Column style={{ padding: '5rem 20rem' }}>
                            {this.props.children}
                        </Grid.Column>
                    </Grid.Row>

                    <Grid.Row style={{ height: '15%' }} className="footer">
                        <Wave className="flipsvg" width="100%" height="15vh" />
                    </Grid.Row>
                </Grid>
            </div>
        );
    }
}
