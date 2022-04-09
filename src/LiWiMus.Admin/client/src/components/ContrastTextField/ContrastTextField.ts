import { styled, TextField } from "@mui/material";

const ContrastTextField = styled(TextField)({
  "& label.MuiInputLabel-root, & p.MuiFormHelperText-root": {
    color: "white",
  },
});

export default ContrastTextField;
