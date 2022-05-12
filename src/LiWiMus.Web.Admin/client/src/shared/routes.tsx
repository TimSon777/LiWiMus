import React from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import LoginPage from "../login/Login.page";
import UsersPage from "../users/Users.page";
import DashboardPage from "../dashboard/Dashboard.page";
import ArtistsPage from "../artists/Artists.page";
import UserProfilePage from "../users/UserProfile.page";
import PlaylistDetailsPage from "../playlists/pages/PlaylistDetails.page";

export const useRoutes = (isAuthenticated: boolean) => {
  if (isAuthenticated) {
    return (
      <Routes>
        <Route path="/admin/dashboard" element={<DashboardPage />} />
        <Route path="/admin/users" element={<UsersPage />} />
        <Route path="/admin/artists" element={<ArtistsPage />} />
        <Route path="/admin/playlists/:id" element={<PlaylistDetailsPage />} />
        <Route path="/admin/users/:id" element={<UserProfilePage />} />
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
