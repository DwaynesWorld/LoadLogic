import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Tracking } from "./views/tracking";

export function TrackingRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Tracking />} />
      </Routes>
    </Suspense>
  );
}
