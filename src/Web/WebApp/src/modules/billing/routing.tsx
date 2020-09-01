import React, { Suspense } from "react";
import { Switch, Route } from "react-router-dom";
import { Billing } from "./views/billing";

export function BillingRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Switch>
        <Route path="/billing">
          <Billing />
        </Route>
      </Switch>
    </Suspense>
  );
}
