import React, { Suspense } from "react";
import { Switch, Route } from "react-router-dom";
import { Vendors } from "./views/vendors";

export function VendorRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Switch>
        <Route path="/vendors">
          <Vendors />
        </Route>
      </Switch>
    </Suspense>
  );
}
