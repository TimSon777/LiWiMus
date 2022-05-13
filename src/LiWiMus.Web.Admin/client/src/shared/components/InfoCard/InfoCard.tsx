import React from "react";
import {Card, CardContent, Typography} from "@mui/material";

type Props = {
  title: string;
  value: string | number;
};

export default function InfoCard({ title, value }: Props) {
  return (
    <Card variant="outlined">
      <CardContent>
        <Typography
          variant={"h4"}
          component={"div"}
          color="primary"
          gutterBottom
        >
          {title}
        </Typography>
        <Typography component={"div"} variant={"h1"} textAlign={"end"}>
          {value}
        </Typography>
      </CardContent>
    </Card>
  );
}