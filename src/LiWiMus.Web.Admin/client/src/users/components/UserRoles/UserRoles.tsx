import React, { useEffect, useState } from "react";
import { User } from "../../types/User";
import { Grid, IconButton, Typography } from "@mui/material";
import { Role } from "../../../roles/types/Role";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import UserService from "../../User.service";
import ActionCard from "../../../shared/components/ActionCard/ActionCard";
import DeleteIcon from "@mui/icons-material/Delete";
import AddRole from "../AddRole/AddRole";
import Loading from "../../../shared/components/Loading/Loading";

type Props = {
  user: User;
};

export default function UserRoles({ user }: Props) {
  const [userRoles, setUserRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const { showError, showSuccess } = useNotifier();

  useEffect(() => {
    setLoading(true);
    UserService.getRoles(user)
      .then(setUserRoles)
      .catch(showError)
      .then(() => setLoading(false));
  }, [user]);

  const removeRole = async (role: Role) => {
    try {
      await UserService.removeRole(user, role);
      setUserRoles(userRoles?.filter((r) => r.id !== role.id));
      showSuccess("User removed from role");
    } catch (e) {
      showError(e);
    }
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <>
      <Typography variant={"h4"} component={"div"}>
        Roles
      </Typography>
      <Grid container spacing={3}>
        {userRoles?.map((role) => (
          <Grid item xs={4} md={3} lg={2} key={role.id}>
            <ActionCard
              text={role.name}
              action={
                <IconButton onClick={async () => await removeRole(role)}>
                  <DeleteIcon sx={{ fontSize: "2rem" }} />
                </IconButton>
              }
            />
          </Grid>
        ))}
        <Grid item xs={4} md={3} lg={2}>
          <AddRole
            user={user}
            userRoles={userRoles}
            setUserRoles={setUserRoles}
          />
        </Grid>
      </Grid>
    </>
  );
}
