import { createTheme } from "@mui/material/styles";

declare module "@mui/material/styles" {
  interface TypeBackground {
    paperLight: string;
  }
}

const theme = createTheme({
  palette: {
    mode: "dark",
    primary: {
      main: "#ed6c02",
    },
    background: {
      default: "#727271",
      paper: "#21201f",
      paperLight: "#444444",
    },
    secondary: {
      main: "#ffffff",
    },
  },
  typography: {
    fontFamily: "'Overpass', sans-serif",
  },
});

export default theme;
