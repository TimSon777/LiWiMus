import React from "react";
import { Layout } from "./shared/components/Layout/Layout";
import { useRoutes } from "./shared/routes";
import { ThemeProvider } from "@mui/material";
import theme from "./shared/theme";
import { BrowserRouter } from "react-router-dom";
import { useAuth } from "./shared/hooks/Auth.hook";
import { AuthContext } from "./shared/contexts/Auth.context";
import { SnackbarProvider } from "notistack";
import { LocalizationProvider } from "@mui/x-date-pickers";
import ruLocale from "date-fns/locale/ru";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import "./App.sass";
import Loading from "./shared/components/Loading/Loading";

function App() {
  const { user, token, payload, setUser, login, logout, ready } = useAuth();
  let isAuthenticated = !!token;
  const routes = useRoutes(isAuthenticated);

  if (!ready) {
    return <Loading />;
  }

  return (
    <AuthContext.Provider
      value={{
        user,
        setUser,
        token,
        login,
        logout,
        isAuthenticated,
        payload,
      }}
    >
      <BrowserRouter>
        <LocalizationProvider dateAdapter={AdapterDateFns} locale={ruLocale}>
          <ThemeProvider theme={theme}>
            <SnackbarProvider
              maxSnack={3}
              preventDuplicate
              autoHideDuration={2000}
              anchorOrigin={{
                vertical: "top",
                horizontal: "center",
              }}
            >
              <Layout user={user}>{routes}</Layout>
            </SnackbarProvider>
          </ThemeProvider>
        </LocalizationProvider>
      </BrowserRouter>
    </AuthContext.Provider>
  );
}

export default App;
