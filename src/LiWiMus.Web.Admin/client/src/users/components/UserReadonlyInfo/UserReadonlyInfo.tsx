import React from "react";
import { User } from "../../types/User";
import ReadonlyInfo from "../../../shared/components/InfoItem/ReadonlyInfo";
import { formatDistanceToNow } from "date-fns";
import { Stack } from "@mui/material";

type Props = {
  user: User;
};

export default function UserReadonlyInfo({ user }: Props) {
  return (
    <Stack spacing={3}>
      <ReadonlyInfo name={"ID"} value={user.id} />
      <ReadonlyInfo name={"Email"} value={user.email} />
      <ReadonlyInfo name={"Email confirmed"} value={`${user.emailConfirmed}`} />
      <ReadonlyInfo name={"Balance"} value={user.balance || 0} />
      <ReadonlyInfo
        name={"Created"}
        value={formatDistanceToNow(user.createdAt, {
          addSuffix: true,
        })}
      />
      <ReadonlyInfo
        name={"Modified"}
        value={formatDistanceToNow(user.modifiedAt, {
          addSuffix: true,
        })}
      />
    </Stack>
  );
}
