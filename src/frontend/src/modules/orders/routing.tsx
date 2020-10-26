import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Orders } from "./views/orders";

export function OrderRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Orders />} />
      </Routes>
    </Suspense>
  );
}
