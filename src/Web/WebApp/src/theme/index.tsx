import { createMuiTheme, colors } from "@material-ui/core";
import { shadows } from "./shadows";
import { typography } from "./typography";

export { GlobalStyles } from "./global-styles";

export const theme = createMuiTheme({
  palette: {
    background: {
      default: colors.grey[200],
      paper: colors.common.white,
    },
    primary: {
      main: colors.blue[700],
    },
    secondary: {
      main: colors.blue[400],
    },
    text: {
      primary: colors.blueGrey[900],
      secondary: colors.blueGrey[600],
    },
  },
  shadows,
  typography: {
    ...typography,
    fontFamily: ['"Lato"', "sans-serif"].join(","),
  },
});