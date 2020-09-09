import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { Icon } from 'semantic-ui-react';
import './NavMenu.css';
import { connect } from 'react-redux';

class NavMenu extends Component {
  static displayName = NavMenu.name;
  constructor (props) {
      super(props);

      this.toggleNavbar = this.toggleNavbar.bind(this);
      this.state = {
          collapsed: true,
      };
  }

  toggleNavbar () {
      this.setState({
          collapsed: !this.state.collapsed,
      });
  }

  render () {
      return (
          <header>
              <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                  <Container>
                      <NavbarBrand tag={Link} to="/">CriThink</NavbarBrand>
                      <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                      <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                          <ul className="navbar-nav flex-grow">
                              {this.props.user ? (
                                  <NavItem>
                                      <NavLink tag={Link} className="text-dark" to="/profile">
                                          <Icon name="user circle" />
                                          {this.props.user}
                                      </NavLink>
                                  </NavItem>
                              ) : <NavItem>
                                  <NavLink tag={Link} className="text-dark" to="/login">Login</NavLink>
                              </NavItem>}
                          </ul>
                      </Collapse>
                  </Container>
              </Navbar>
          </header>
      );
  }
}

function mapStateToProps (state) {
    return {
        user: state.auth.username,
    };
}

export default connect(mapStateToProps)(NavMenu);