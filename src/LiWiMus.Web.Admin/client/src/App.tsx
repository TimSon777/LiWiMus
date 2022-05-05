import React from "react";
import { Layout } from "./components/Layout/Layout";
import { useRoutes } from "./routes";
import { ThemeProvider } from "@mui/material";
import theme from "./theme";
import { BrowserRouter } from "react-router-dom";
import { useAuth } from "./hooks/Auth.hook";
import { AuthContext } from "./contexts/Auth.context";
import "./App.sass";

function App() {
  const { userData, login, logout, ready } = useAuth();
  const isAuthenticated = !!userData;
  const routes = useRoutes(isAuthenticated);

  if (!ready) {
    return <div />;
  }

  return (
    <AuthContext.Provider
      value={{ userData, login, logout, isAuthenticated }}
    >
      <BrowserRouter>
        <ThemeProvider theme={theme}>
          <Layout userData={userData}>{routes}</Layout>
        </ThemeProvider>
      </BrowserRouter>
    </AuthContext.Provider>
  );
}

export default App;
