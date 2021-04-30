import React from "react";
import ReactDOM from "react-dom";
import MomentUtils from "@date-io/moment";
import { BrowserRouter as Router } from "react-router-dom";
import { MuiPickersUtilsProvider } from "@material-ui/pickers";

import reportWebVitals from "./report-web-vitals";
import { App } from "./app";

ReactDOM.render(
  <React.StrictMode>
    <Router>
      <MuiPickersUtilsProvider utils={MomentUtils}>
        <App />
      </MuiPickersUtilsProvider>
    </Router>
  </React.StrictMode>,
  document.getElementById("root")
);

// eslint-disable-next-line no-console
reportWebVitals(console.info);
