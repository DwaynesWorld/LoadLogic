import React from "react";
import { useRoutes } from "react-router-dom";
import { ThemeProvider } from "@material-ui/styles";
import { CssBaseline } from "@material-ui/core";
import { routes } from "./app.routes";
import { theme, GlobalStyles } from "./theme";

export function App() {
  const routing = useRoutes(routes);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <GlobalStyles />
      {routing}
    </ThemeProvider>
  );
}
