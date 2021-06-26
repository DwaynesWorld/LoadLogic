import React from "react";
import ReactDOM from "react-dom";
import MomentUtils from "@date-io/moment";
import { BrowserRouter as Router } from "react-router-dom";
import { MuiPickersUtilsProvider } from "@material-ui/pickers";

import reportWebVitals from "./report-web-vitals";
import Auth0ProviderWithHistory from "./auth/auth0-provider-with-history";
import { App } from "./app";

ReactDOM.render(
  <React.StrictMode>
    <Router>
      <Auth0ProviderWithHistory>
        <MuiPickersUtilsProvider utils={MomentUtils}>
          <App />
        </MuiPickersUtilsProvider>
      </Auth0ProviderWithHistory>
    </Router>
  </React.StrictMode>,
  document.getElementById("root")
);

// eslint-disable-next-line no-console
reportWebVitals(console.info);
