import React from "react";
import { NavLink as RouterLink } from "react-router-dom";
import { Button, colors, ListItem, makeStyles } from "@material-ui/core";
import { Icon as IconType } from "react-feather";

interface Props {
  title: string;
  to: string;
  icon: IconType;
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
    color: colors.grey[400],
    fontSize: 15,
    fontWeight: theme.typography.fontWeightMedium,
    justifyContent: "flex-start",
    letterSpacing: 0,
    padding: "15px 8px",
    textTransform: "none",
    width: "100%",
  },
  icon: {
    marginRight: theme.spacing(3),
  },
  title: {
    marginRight: "auto",
  },
  active: {
    backgroundColor: colors.grey[900],
    color: colors.grey[50],
    "& $title": {
      fontWeight: theme.typography.fontWeightBold,
    },
    "& $icon": {
      color: colors.grey[100],
    },
  },
}));
