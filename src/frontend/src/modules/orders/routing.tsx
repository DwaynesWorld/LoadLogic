import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { CreateOrder } from "./views/create-order";
import { Orders } from "./views/orders";

export function OrderRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Orders />} />
        <Route path="/create" element={<CreateOrder />} />
      </Routes>
    </Suspense>
  );
}
