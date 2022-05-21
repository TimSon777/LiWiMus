import React from "react";
import { User } from "../../types/User";
import { Link } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

type Props = {
  user: User;
};

export default function UserLink({ user }: Props) {
  return (
    <Link
      component={RouterLink}
      to={`/admin/users/${user.id}`}
      underline="none"
      color={"secondary"}
    >
      {user.userName}
    </Link>
  );
}
