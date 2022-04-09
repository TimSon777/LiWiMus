import React from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "./pages/Login.page";
import UsersPage from "./pages/Users.page";

export const useRoutes = (isAuthenticated: boolean) => {
  if (isAuthenticated) {
    return (
      <Routes>
        <Route path={"/admin/users"} element={<UsersPage />} />
        <Route path="*" element={<Navigate to="/admin/users" replace />} />
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
