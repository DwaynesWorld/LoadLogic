import "./index.css";
import React from "react";
import ReactDOM from "react-dom";
import { BrowserRouter as Router } from "react-router-dom";

import reportWebVitals from "./report-web-vitals";
import { App } from "./app";

ReactDOM.render(
  <React.StrictMode>
    <Router>
      <App />
    </Router>
  </React.StrictMode>,
  document.getElementById("root")
);

// eslint-disable-next-line no-console
reportWebVitals(console.info);
