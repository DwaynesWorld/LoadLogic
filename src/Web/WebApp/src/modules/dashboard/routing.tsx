import React, { Suspense } from "react";
import { Switch, Route } from "react-router-dom";
import { Dashboard } from "./views/dashboard";

export function DashboardRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Switch>
        <Route path="/dashboard">
          <Dashboard />
        </Route>
      </Switch>
    </Suspense>
  );
}
