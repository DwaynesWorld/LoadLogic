import React from "react";
import {
  Box,
  Button,
  Divider,
  Drawer,
  List,
  makeStyles,
  Typography
} from "@material-ui/core";

import {
  // BarChart as BarChartIcon,
  // Settings as SettingsIcon,
  Folder as FolderIcon,
  Truck as TruckIcon,
  User as UserIcon,
  Users as UsersIcon,
  HelpCircle as HelpIcon,
  CreditCard as BillingIcon
} from "react-feather";

import { NavItem } from "./nav-item";

const items = [
  {
    to: "/app/orders",
    icon: FolderIcon,
    title: "Orders"
  },
  {
    to: "/app/tracking",
    icon: TruckIcon,
    title: "Tracking"
  },
  {
    to: "/app/billing",
    icon: BillingIcon,
    title: "Billing"
  },
  {
    to: "/app/people",
    icon: UsersIcon,
    title: "People"
  }
];

export function SideNavigation() {
  const classes = useStyles();

  return (
    <Drawer
      anchor="left"
      classes={{ paper: classes.drawer }}
      variant="persistent"
      open
    >
      <Box height="100%" display="flex" flexDirection="column">
        <Box
          alignItems="center"
          justifyContent="center"
          display="flex"
          flexDirection="row"
          p={2}
        >
          Some
        </Box>

        <Box p={2}>
          <List>
            {items.map(item => (
              <NavItem
                to={item.to}
                key={item.title}
                title={item.title}
                icon={item.icon}
              />
            ))}
          </List>
        </Box>

        <Box flexGrow={3} />
        <Divider light />

        <Box
          display="flex"
          flexDirection="column"
          flexGrow={1}
          p={2}
          m={2}
          justifyContent="flex-end"
        >
          {/* <NavItem to="/app/settings" icon={SettingsIcon} title="Settings" /> */}
          <NavItem to="/app/settings" icon={UserIcon} title="Account" />
          {/* <NavItem to="/app/help" icon={HelpIcon} title="Help" /> */}
        </Box>

        <Box>
          <Typography variant="body2">Need Help?</Typography>
          <Typography variant="body2">Check out our docs</Typography>
          <Button variant="contained" color="primary">
            Documentation
          </Button>
        </Box>
      </Box>
    </Drawer>
  );
}

const useStyles = makeStyles(() => ({
  drawer: {
    width: 256,
    top: 0,
    height: "100%",
    backgroundColor: "black"
  },
  avatar: {
    cursor: "pointer",
    width: 64,
    height: 64
  }
}));
