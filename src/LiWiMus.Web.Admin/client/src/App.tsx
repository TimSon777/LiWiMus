import React from "react";
import { Layout } from "./components/Layout/Layout";
import { useRoutes } from "./routes";
import { ThemeProvider } from "@mui/material";
import theme from "./theme";
import { BrowserRouter } from "react-router-dom";
import { useAuth } from "./hooks/Auth.hook";
import { AuthContext } from "./contexts/Auth.context";
import { SnackbarProvider } from "notistack";
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
      </BrowserRouter>
    </AuthContext.Provider>
  );
}

export default App;
