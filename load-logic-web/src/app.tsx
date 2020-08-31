import React from "react";
import { Switch, Route, Redirect } from "react-router-dom";
import { MdSearch, MdNotifications, MdSettings } from "react-icons/md";

import { OrderRouting } from "src/modules/orders";
import { BillingRouting } from "src/modules/billing";
import { VendorRouting } from "src/modules/vendors";
import { SettingsRouting } from "src/modules/settings";
import { DashboardRouting } from "src/modules/dashboard";

import {
  Navbar,
  PrimaryAppRoute,
  SecondaryAppRoute,
} from "src/core/components";

export function App() {
  const primaryRoutes: PrimaryAppRoute[] = [
    { name: "Dashboard", to: "/dashboard" },
    { name: "Orders", to: "/orders" },
    { name: "Billing", to: "/billing" },
    { name: "Vendors", to: "/vendors" },
  ];

  const secondaryRoutes: SecondaryAppRoute[] = [
    { name: "Search", icon: MdSearch, onClick: () => {} },
    { name: "Notifications", icon: MdNotifications, onClick: () => {} },
    { name: "Settings", icon: MdSettings, to: "/settings" },
  ];

  return (
    <div>
      <Navbar primaryRoutes={primaryRoutes} secondaryRoutes={secondaryRoutes} />

      <div>
        <Switch>
          <Route path="/dashboard">
            <DashboardRouting />
          </Route>
          <Route path="/orders">
            <OrderRouting />
          </Route>
          <Route path="/billing">
            <BillingRouting />
          </Route>
          <Route path="/vendors">
            <VendorRouting />
          </Route>
          <Route path="/settings">
            <SettingsRouting />
          </Route>
          <Route path="/">
            <Redirect to="/dashboard" />
          </Route>
        </Switch>
      </div>
    </div>
  );
}
