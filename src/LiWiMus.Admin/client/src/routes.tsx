import React from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/Login.page";
import UsersPage from "./pages/Users.page";
import DashboardPage from "./pages/Dashboard.page";
import ArtistsPage from "./pages/Artists.page";

export const useRoutes = (isAuthenticated: boolean) => {
  if (isAuthenticated) {
    return (
      <Routes>
        <Route path="/admin/dashboard" element={<DashboardPage />} />
        <Route path="/admin/users" element={<UsersPage />} />
        <Route path="/admin/artists" element={<ArtistsPage />} />
        <Route path="*" element={<Navigate to="/admin/dashboard" replace />} />
      </Routes>
    );
  }

  return (
    <Routes>
      <Route path={"/admin"} element={<LoginPage />} />
      <Route path="*" element={<Navigate to="/admin" replace />} />
    </Routes>
  );
};
