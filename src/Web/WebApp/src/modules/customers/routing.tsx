import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Customers } from "./views/customers";

export function CustomerRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Customers />} />
      </Routes>
    </Suspense>
  );
}
