import React from "react";
import { Navigate } from "react-router-dom";
import { AppLayout, GenericLayout } from "src/components";
import { DashboardRouting } from "./modules/dashboard";
import { OrderRouting } from "./modules/orders";
import { BillingRouting } from "./modules/billing";
import { CustomerRouting } from "./modules/customers";
import { DriverRouting } from "./modules/drivers";
import { SettingsRouting } from "./modules/settings";
import { LoginView } from "./modules/auth";
import { NotFoundView } from "./modules/errors";

export const routes = [
  {
    path: "app",
    element: <AppLayout />,
    children: [
      { path: "dashboard", element: <DashboardRouting /> },
      { path: "orders", element: <OrderRouting /> },
      { path: "billing", element: <BillingRouting /> },
      { path: "customers", element: <CustomerRouting /> },
      { path: "drivers", element: <DriverRouting /> },
      { path: "settings", element: <SettingsRouting /> },
      { path: "*", element: <Navigate to="/404" /> },
    ],
  },
  {
    path: "/",
    element: <GenericLayout />,
    children: [
      { path: "login", element: <LoginView /> },
      { path: "404", element: <NotFoundView /> },
      { path: "/", element: <Navigate to="/app/dashboard" /> },
      { path: "*", element: <Navigate to="/404" /> },
    ],
  },
];
