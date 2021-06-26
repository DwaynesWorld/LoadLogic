import React from "react";
import { useNavigate } from "react-router-dom";

import config from "src/app.config";
import { AppState, Auth0Provider } from "@auth0/auth0-react";

interface Props {
  children?: React.ReactNode;
}

function Auth0ProviderWithHistory({ children }: Props) {
  const navigate = useNavigate();

  function onRedirectCallback(appState: AppState) {
    navigate(appState?.returnTo || window.location.pathname);
  }

  return (
    <Auth0Provider
      domain={config.auth.domain}
      clientId={config.auth.clientId}
      redirectUri={window.location.origin}
      onRedirectCallback={onRedirectCallback}
    >
      {children}
    </Auth0Provider>
  );
}

export default Auth0ProviderWithHistory;
