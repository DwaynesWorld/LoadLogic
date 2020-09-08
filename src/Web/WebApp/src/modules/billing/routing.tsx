import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Billing } from "./views/billing";

export function BillingRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Billing />} />
      </Routes>
    </Suspense>
  );
}
