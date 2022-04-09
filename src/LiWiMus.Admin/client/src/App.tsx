import React from "react";
import { Layout } from "./components/layout/Layout";
import { useRoutes } from "./routes";
import { ThemeProvider } from "@mui/material";
import theme from "./theme";
import { BrowserRouter } from "react-router-dom";
import { useAuth } from "./hooks/Auth.hook";
import { AuthContext } from "./contexts/Auth.context";
import "./App.sass";

function App() {
  const { token, login, logout, userId } = useAuth();
  const isAuthenticated = !!token;
  const routes = useRoutes(isAuthenticated);

  return (
    <AuthContext.Provider
      value={{ token, login, logout, userId, isAuthenticated }}
    >
      <BrowserRouter>
        <ThemeProvider theme={theme}>
          <Layout>{routes}</Layout>
        </ThemeProvider>
      </BrowserRouter>
    </AuthContext.Provider>
  );
}

export default App;
