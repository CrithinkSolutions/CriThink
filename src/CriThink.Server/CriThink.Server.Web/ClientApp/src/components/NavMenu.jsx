import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { Icon } from 'semantic-ui-react'
import './NavMenu.css';
import AuthHandler from "../handlers/authHandler";

export class NavMenu extends Component {
  static displayName = NavMenu.name;
  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      user: AuthHandler.getCurrentUser()
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  render () {
    const { user } = this.state
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">CriThink</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                {user ? (
                  <NavItem>
                    <NavLink tag={Link} className="text-dark" to="/profile">
                      <Icon name="user circle" />
                      {user.userName}
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
