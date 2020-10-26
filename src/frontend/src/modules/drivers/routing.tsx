import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Drivers } from "./views/drivers";

export function DriverRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Drivers />} />
      </Routes>
    </Suspense>
  );
}
