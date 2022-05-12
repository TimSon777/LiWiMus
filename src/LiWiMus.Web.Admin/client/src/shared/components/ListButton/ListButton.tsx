import React from "react";
import {
  ListItem,
  ListItemIcon,
  ListItemText,
  Typography,
} from "@mui/material";

export interface ListButtonProps {
  icon?: JSX.Element;
  text: string;
  onClick: () => void;
}

export default function ListButton({ icon, onClick, text }: ListButtonProps) {
  const listItemIcon = icon ? <ListItemIcon>{icon}</ListItemIcon> : "";

  return (
    <ListItem button onClick={onClick}>
      {listItemIcon}
      <ListItemText>
        <Typography>{text}</Typography>
      </ListItemText>
    </ListItem>
  );
}
