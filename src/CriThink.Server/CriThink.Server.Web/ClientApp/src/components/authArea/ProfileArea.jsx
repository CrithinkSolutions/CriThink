import React, { Component } from 'react';
import { Link } from 'react-router-dom'
import { Grid, Image, Container, Icon, Card, Button } from 'semantic-ui-react'
import AuthHandler from "../../handlers/authHandler";

export class ProfileArea extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false,
            msg: '',
            user: AuthHandler.getCurrentUser()
        };
    }
    
    /*
    changeHandler = event => {
        this.setState({
          [event.target.name]: event.target.value
        });
      }
    */

    componentDidMount() {
        console.log(AuthHandler.getCurrentUser())    
    }

    render() {
        const { user } = this.state
        return (
            <div id="mainrender">
                {user ? (
                    <Grid>
                        <Grid.Row columns={2}>
                            <Grid.Column width={4}>
                                <Card fluid>
                                    <Image src='https://increasify.com.au/wp-content/uploads/2016/08/default-image.png' wrapped ui={false} />
                                    <Card.Content>
                                      <Card.Header>{user.userName}</Card.Header>
                                      <Card.Meta>
                                        <span className='date'>{user.userEmail}</span>
                                      </Card.Meta>
                                    </Card.Content>
                                </Card>
                            </Grid.Column>
                            <Grid.Column width={10}>
                                <h2>Settings</h2>
                                <Link to='/profile/changepassword'><p><Icon name="key" />
                                <span className="link">Change password</span></p></Link>
                                <Link to='/profile'><p><Icon name="mail" />
                                <span className="link">Change email</span></p></Link>
                                <Link to='/profile'><p><Icon name="settings" />
                                <span className="link">All settings</span></p></Link>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                ) : <h1>Please login first</h1>
                }         
            </div>
        );
    }
}