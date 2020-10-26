import React, { Suspense } from "react";
import { Routes, Route } from "react-router-dom";
import { Settings } from "./views/settings";

export function SettingsRouting() {
  return (
    <Suspense fallback={<div>Loading</div>}>
      <Routes>
        <Route path="/" element={<Settings />} />
      </Routes>
    </Suspense>
  );
}
