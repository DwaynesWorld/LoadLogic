import React from "react";
import { Navigate } from "react-router-dom";
import { AppLayout, GenericLayout } from "src/components";
import { OrderRouting } from "./modules/orders";
import { BillingRouting } from "./modules/billing";
import { CustomerRouting } from "./modules/customers";
import { TrackingRouting } from "./modules/tracking";
import { SettingsRouting } from "./modules/settings";
import { LoginView } from "./modules/auth";
import { NotFoundView } from "./modules/errors";

export const routes = [
  {
    path: "app",
    element: <AppLayout />,
    children: [
      { path: "orders", element: <OrderRouting /> },
      { path: "tracking", element: <TrackingRouting /> },
      { path: "billing", element: <BillingRouting /> },
      { path: "people", element: <CustomerRouting /> },
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
      { path: "/", element: <Navigate to="/app/orders" /> },
      { path: "*", element: <Navigate to="/404" /> },
    ],
  },
];
