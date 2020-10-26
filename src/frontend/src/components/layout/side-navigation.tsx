import React from "react";
import { Box, Divider, Drawer, List, makeStyles } from "@material-ui/core";

import {
  BarChart as BarChartIcon,
  Settings as SettingsIcon,
  Folder as FolderIcon,
  FileText as FileTextIcon,
  Truck as TruckIcon,
  User as UserIcon,
  Users as UsersIcon,
} from "react-feather";

import { NavItem } from "./nav-item";

const items = [
  {
    to: "/app/dashboard",
    icon: BarChartIcon,
    title: "Dashboard",
  },
  {
    to: "/app/orders",
    icon: FolderIcon,
    title: "Orders",
  },
  {
    to: "/app/billing",
    icon: FileTextIcon,
    title: "Billing",
  },
  {
    to: "/app/customers",
    icon: UsersIcon,
    title: "Customers",
  },
  {
    to: "/app/drivers",
    icon: TruckIcon,
    title: "Drivers",
  },
];

export function SideNavigation() {
  const classes = useStyles();

  return (
    <Drawer
      anchor="left"
      classes={{ paper: classes.drawer }}
      variant="persistent"
      open={true}
    >
      <Box height="100%" display="flex" flexDirection="column">
        <Box alignItems="center" display="flex" flexDirection="column" p={2}>
          some
        </Box>
        <Divider />
        <Box p={2}>
          <List>
            {items.map((item) => (
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
        <Divider />
        <Box flexGrow={1} p={2} m={2}>
          <NavItem to="/app/settings" icon={SettingsIcon} title="Settings" />
          <NavItem to="/app/account" icon={UserIcon} title="Account" />
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
  },
  avatar: {
    cursor: "pointer",
    width: 64,
    height: 64,
  },
}));
