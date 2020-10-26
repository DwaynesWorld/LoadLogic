import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Dashboard } from "./views/dashboard";

export function DashboardRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Dashboard />} />
      </Routes>
    </Suspense>
  );
}
