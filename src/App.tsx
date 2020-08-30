import "./app.css";
import React from "react";
import styled from "styled-components";
import logo from "./logo.png";
import { Switch, Route, Link, NavLink, Redirect } from "react-router-dom";
import { Navbar, Nav as RBNav, NavDropdown } from "react-bootstrap";
import { MdSearch, MdNotifications, MdSettings } from "react-icons/md";

function App() {
  return (
    <div>
      <Navbar bg="dark" variant="dark">
        <Navbar.Brand as={Link} to="/">
          <img
            src={logo}
            width="30"
            height="30"
            className="d-inline-block align-top"
            alt="logo"
          />
        </Navbar.Brand>

        <Nav>
          <PrimaryNav>
            <Nav.Link as={NavLink} to="/dashboard">
              Dashboard
            </Nav.Link>
            <Nav.Link as={NavLink} to="/orders">
              Orders
            </Nav.Link>
            <Nav.Link as={NavLink} to="/billing">
              Billing
            </Nav.Link>
            <Nav.Link as={NavLink} to="/vendors">
              Vendors
            </Nav.Link>
          </PrimaryNav>

          <SecondaryNav>
            <Nav.Link>
              <MdSearch size={22} />
            </Nav.Link>

            <Nav.Link>
              <MdNotifications size={22} />
            </Nav.Link>

            <Nav.Link as={NavLink} to="/settings">
              <MdSettings size={22} />
            </Nav.Link>

            <NavDropdown id="basic-nav-dropdown" title="Hello, John">
              <NavDropdown.Item href="#action/3.1">
                Another action
              </NavDropdown.Item>
              <NavDropdown.Item href="#action/3.2">
                Another action
              </NavDropdown.Item>
              <NavDropdown.Item href="#action/3.3">
                Another action
              </NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item href="#action/3.4">
                Separated link
              </NavDropdown.Item>
            </NavDropdown>
          </SecondaryNav>
        </Nav>
      </Navbar>

      <div>
        <Switch>
          <Route path="/dashboard">
            <Dashboard />
          </Route>
          <Route path="/orders">
            <Orders />
          </Route>
          <Route path="/billing">
            <Billing />
          </Route>
          <Route path="/vendors">
            <Vendors />
          </Route>
          <Route path="/settings">
            <Settings />
          </Route>
          <Route path="/">
            <Redirect to="/dashboard" />
          </Route>
        </Switch>
      </div>
    </div>
  );
}

function Dashboard() {
  return <h2>Dashboard</h2>;
}

function Orders() {
  return <h2>Orders</h2>;
}

function Billing() {
  return <h2>Invoices</h2>;
}

function Vendors() {
  return <h2>Vendors</h2>;
}

function Settings() {
  return <h2>Settings</h2>;
}

const Nav = styled(RBNav)`
  flex: 1;
  justify-content: flex-end;
`;

const PrimaryNav = styled.div`
  display: flex;

  a.nav-link {
    display: flex;
    align-items: center;
    text-transform: uppercase;
    font-size: 14px;
    font-weight: 600;
    padding-left: 15px !important;
  }
`;

const SecondaryNav = styled.div`
  display: flex;
  margin-left: 20px;
  padding-left: 20px;
  border-left: 1px solid rgba(255, 255, 255, 0.25);

  .dropdown-menu {
    right: 0;
    left: auto;
  }
`;

export default App;
