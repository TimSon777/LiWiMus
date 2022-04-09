import React from "react";
import "./App.sass";
import { Route, Routes } from "react-router-dom";
import { Layout } from "./components/layout/Layout";
import LoginPage from "./pages/Login.page";

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/admin/login" element={<LoginPage />} />
      </Routes>
    </Layout>
  );
}

export default App;
