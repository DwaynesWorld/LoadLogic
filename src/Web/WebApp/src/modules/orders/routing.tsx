import React, { Suspense } from "react";
import { Switch, Route } from "react-router-dom";
import { Orders } from "./views/orders";

export function OrderRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Switch>
        <Route path="/orders">
          <Orders />
        </Route>
      </Switch>
    </Suspense>
  );
}
