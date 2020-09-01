import React, { Suspense } from "react";
import { Switch, Route } from "react-router-dom";
import { Settings } from "./views/settings";

export function SettingsRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Switch>
        <Route path="/settings">
          <Settings />
        </Route>
      </Switch>
    </Suspense>
  );
}
