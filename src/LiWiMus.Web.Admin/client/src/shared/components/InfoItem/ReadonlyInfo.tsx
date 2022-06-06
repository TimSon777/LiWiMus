import React, {ReactElement} from "react";
import {Box, Typography} from "@mui/material";

type Props = {
  name: string;
  value: ReactElement | string | number;
};

export default function ReadonlyInfo({ name, value }: Props) {
  return (
    <Box
      sx={{
        px: 1,
        width: "100%",
        borderBottom: 1,
        borderColor: "secondary.light",
        display: "flex",
        justifyContent: "space-between",
        transition: "border 100ms ease-out",
        ":hover": {
          borderColor: "secondary.main",
        },
      }}
    >
      <Typography variant={"h6"} component={"div"} sx={{mr: 2}}>
        {name}
      </Typography>
      <Typography variant={"h6"} component={"div"} sx={{overflowX: "scroll", "&::-webkit-scrollbar": {width: "0px", background: "transparent"}}}>
        {value}
      </Typography>
    </Box>
  );
}