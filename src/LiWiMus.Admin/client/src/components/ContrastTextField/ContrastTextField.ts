import { styled, TextField } from "@mui/material";

const ContrastTextField = styled(TextField)({
  "& label.MuiInputLabel-root, & p.MuiFormHelperText-root": {
    color: "white",
  },
  "& :autofill": {
    boxShadow: "none!important",
  },
});

export default ContrastTextField;
