import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Grid, Image, Icon, Card } from 'semantic-ui-react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getUserLogout } from '../../actions/auth';

class ProfileArea extends Component {

    /* Hander for future settings
    changeHandler = event => {
        this.setState({
          [event.target.name]: event.target.value
        });
      }
    */

    render () {
        return (
            <div id="mainrender">
                <Grid>
                    <Grid.Row columns={2}>
                        <Grid.Column width={4}>
                            <Card fluid>
                                <Image src='https://increasify.com.au/wp-content/uploads/2016/08/default-image.png' wrapped ui={false} />
                                <Card.Content>
                                    <Card.Header>{this.props.user}</Card.Header>
                                    <Card.Meta>
                                        <span className='date'>email</span>
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
                            <Link to='/backoffice'><p><Icon name="edit" />
                                <span className="link">Backoffice</span></p></Link>
                            <Link onClick={this.props.getUserLogout}><p><Icon name="sign-out" />
                                <span className="link">Logout</span></p></Link>
                        </Grid.Column>
                    </Grid.Row>
                </Grid>
            </div>
        );
    }
}

function mapStateToProps (state) {
    return {
        user: state.auth.username,
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        getUserLogout,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ProfileArea);