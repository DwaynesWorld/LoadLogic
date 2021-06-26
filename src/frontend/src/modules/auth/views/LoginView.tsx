import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "@material-ui/core";
import { useNavigate } from "react-router";

export function LoginView() {
  const navigate = useNavigate();

  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isAuthenticated) {
    navigate("/app/orders");
    return <div>Loading...</div>;
  }

  return (
    <div>
      <LoginButton />
      <LogoutButton />
    </div>
  );
}

function LoginButton() {
  const { loginWithRedirect } = useAuth0();
  return <Button onClick={() => loginWithRedirect()}>Log In</Button>;
}

function LogoutButton() {
  const { logout } = useAuth0();
  return (
    <Button onClick={() => logout({ returnTo: window.location.origin })}>
      Log Out
    </Button>
  );
}
