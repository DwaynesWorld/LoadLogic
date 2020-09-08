import React from "react";
import { NavLink as RouterLink } from "react-router-dom";
import { Button, ListItem, makeStyles } from "@material-ui/core";

interface Props {
  title: string;
  to: string;
  icon: any;
}
export function NavItem({ to, icon: Icon, title, ...rest }: Props) {
  const classes = useStyles();

  return (
    <ListItem className={classes.item} disableGutters {...rest}>
      <Button
        activeClassName={classes.active}
        className={classes.button}
        component={RouterLink}
        to={to}
      >
        {Icon && <Icon className={classes.icon} size="20" />}
        <span className={classes.title}>{title}</span>
      </Button>
    </ListItem>
  );
}

const useStyles = makeStyles((theme) => ({
  item: {
    display: "flex",
    paddingTop: 0,
    paddingBottom: 0,
  },
  button: {
    color: theme.palette.text.secondary,
    fontSize: 15,
    fontWeight: theme.typography.fontWeightMedium,
    justifyContent: "flex-start",
    letterSpacing: 0,
    padding: "10px 8px",
    textTransform: "none",
    width: "100%",
  },
  icon: {
    marginRight: theme.spacing(1),
  },
  title: {
    marginRight: "auto",
  },
  active: {
    backgroundColor: theme.palette.background.default,
    color: theme.palette.primary.main,
    "& $title": {
      fontWeight: theme.typography.fontWeightBold,
    },
    "& $icon": {
      color: theme.palette.primary.main,
    },
  },
}));
