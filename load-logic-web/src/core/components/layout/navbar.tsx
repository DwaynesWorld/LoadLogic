import React from "react";
import styled from "styled-components";
import { Link, NavLink } from "react-router-dom";
import { Navbar as RBNavbar, Nav as RBNav, NavDropdown } from "react-bootstrap";
import { IconType } from "react-icons/lib";

import logo from "src/assets/img/logo.png";

export interface PrimaryAppRoute {
  name: string;
  to: string;
}

export interface SecondaryAppRoute {
  name: string;
  icon: IconType;
  to?: string;
  onClick?: () => void;
}

interface Props {
  primaryRoutes: PrimaryAppRoute[];
  secondaryRoutes: SecondaryAppRoute[];
}
export function Navbar({ primaryRoutes, secondaryRoutes }: Props) {
  return (
    <RBNavbar bg="dark" variant="dark" sticky="top">
      <RBNavbar.Brand as={Link} to="/">
        <img
          src={logo}
          width="30"
          height="30"
          className="d-inline-block align-top"
          alt="logo"
        />
      </RBNavbar.Brand>

      <Nav>
        <PrimaryNav>
          {primaryRoutes.map((r) => (
            <Nav.Link as={NavLink} to={r.to}>
              {r.name}
            </Nav.Link>
          ))}
        </PrimaryNav>

        <SecondaryNav>
          {secondaryRoutes.map((r) => {
            const Icon = r.icon;

            if (r.to) {
              return (
                <Nav.Link as={NavLink} to={r.to}>
                  <Icon size={22} />
                </Nav.Link>
              );
            }

            return (
              <Nav.Link>
                <Icon size={22} />
              </Nav.Link>
            );
          })}

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
    </RBNavbar>
  );
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
