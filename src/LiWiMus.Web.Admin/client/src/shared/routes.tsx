import React from "react";
import {Navigate, Route, Routes} from "react-router-dom";
import LoginPage from "../login/Login.page";
import UsersPage from "../users/pages/Users.page";
import DashboardPage from "../dashboard/Dashboard.page";
import ArtistsPage from "../artists/pages/Artists.page";
import UserProfilePage from "../users/pages/UserProfile.page";
import PlaylistDetailsPage from "../playlists/pages/PlaylistDetails.page";
import GenreDetailsPage from "../genres/pages/GenreDetails.page";
import TransactionDetailsPage from "../transactions/pages/TransactionDetails.page";
import AlbumDetailsPage from "../albums/pages/AlbumDetails.page";
import CreateArtistPage from "../artists/pages/CreateArtist.page";
import ArtistDetailsPage from "../artists/pages/ArtistDetails.page";
import TrackDetailsPage from "../tracks/pages/TrackDetails.page";
import TransactionsPage from "../transactions/pages/Transactions.page"
import TracksPage from "../tracks/pages/Tracks.page"
import CreateUserPage from "../users/pages/CreateUser.page"
import GenresPage from "../genres/pages/Genres.page"
import CreateGenrePage from "../genres/pages/CreateGenre.page"
import CreateTrackPage from "../tracks/pages/CreateTrack.page"
import PlaylistsPage from "../playlists/pages/Playlists.page"
import CreatePlaylistPage from "../playlists/pages/CreatePlaylist.page"
import PlanDetailsPage from "../plans/pages/PlanDetails.page";
import RoleDetailsPage from "../roles/pages/RoleDetails.page";
import CreateTransactionPage from "../transactions/pages/CreateTransaction.page"
import CreatePlanPage from "../plans/pages/CreatePlan.page"
import PlansPage from "../plans/pages/Plans.page"

export const useRoutes = (isAuthenticated: boolean) => {
    if (isAuthenticated) {
        return (
            <Routes>
                <Route path="/admin/dashboard" element={<DashboardPage/>}/>
                <Route path="/admin/users" element={<UsersPage/>}/>
                <Route path="/admin/artists" element={<ArtistsPage/>}/>
                <Route path="/admin/transactions" element={<TransactionsPage/>}/>
                <Route path="/admin/tracks" element={<TracksPage/>}/>
                <Route path="/admin/genres" element={<GenresPage/>}/>
                <Route path="/admin/plans" element={<PlansPage/>}/>
                <Route path="/admin/playlists" element={<PlaylistsPage/>}/>
                <Route path="/admin/artists/:id" element={<ArtistDetailsPage/>}/>
                <Route path="/admin/tracks/:id" element={<TrackDetailsPage/>}/>
                <Route path="/admin/artists/create" element={<CreateArtistPage/>}/>
                <Route path="/admin/genres/create" element={<CreateGenrePage/>}/>
                <Route path="/admin/users/create" element={<CreateUserPage/>}/>
                <Route path="/admin/tracks/create" element={<CreateTrackPage/>}/>
                <Route path="/admin/playlists/:id" element={<PlaylistDetailsPage/>}/>
                <Route path="/admin/albums/:id" element={<AlbumDetailsPage/>}/>
                <Route path="/admin/plans/create" element={<CreatePlanPage/>}/>
                <Route path="/admin/playlists/create" element={<CreatePlaylistPage/>}/>
                <Route path="/admin/transactions/create" element={<CreateTransactionPage/>}/>
                <Route path="/admin/plans/:id" element={<PlanDetailsPage />} />
                <Route path="/admin/roles/:id" element={<RoleDetailsPage />} />
                <Route
                    path="/admin/transactions/:id"
                    element={<TransactionDetailsPage/>}
                />
                <Route path="/admin/genres/:id" element={<GenreDetailsPage/>}/>
                <Route path="/admin/users/:id" element={<UserProfilePage/>}/>
                <Route path="*" element={<Navigate to="/admin/dashboard" replace/>}/>
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