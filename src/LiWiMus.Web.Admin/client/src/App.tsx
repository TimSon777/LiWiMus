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

function App() {
  const { userData, login, logout, ready } = useAuth();
  const isAuthenticated = !!userData;
  const routes = useRoutes(isAuthenticated);

  if (!ready) {
    return <div />;
  }

  return (
    <AuthContext.Provider value={{ userData, login, logout, isAuthenticated }}>
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
              <Layout userData={userData}>{routes}</Layout>
            </SnackbarProvider>
          </ThemeProvider>
        </LocalizationProvider>
      </BrowserRouter>
    </AuthContext.Provider>
  );
}

export default App;
