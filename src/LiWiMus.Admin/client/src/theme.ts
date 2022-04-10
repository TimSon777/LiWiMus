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
  },
});

export default theme;
