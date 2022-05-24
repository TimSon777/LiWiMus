import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { User } from "../types/User";
import UserService from "../User.service";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Paper, Stack, Typography } from "@mui/material";
import UserImageEditor from "../components/UserImageEditor/UserImageEditor";
import UserInfoEditor from "../components/UserInfoEditor/UserInfoEditor";
import UserRoles from "../components/UserRoles/UserRoles";
import UserReadonlyInfo from "../components/UserReadonlyInfo/UserReadonlyInfo";

export default function UserProfilePage() {
  const { id } = useParams() as { id: string };
  const [user, setUser] = useState<User>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    setLoading(true);
    UserService.get(id)
      .then(setUser)
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  if (!user) {
    return <NotFound />;
  }

  return (
    <Grid container spacing={5} justifyContent={"center"} sx={{ py: 5 }}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            {user.userName}
          </Typography>

          <Grid container spacing={5} justifyContent={"center"}>
            <Grid item xs={12} md={6}>
              <Stack spacing={2} alignItems={"center"}>
                <UserImageEditor user={user} setUser={setUser} />
              </Stack>
            </Grid>

            <Grid item xs={12} md={6}>
              <UserReadonlyInfo user={user} />
            </Grid>

            <Grid item xs={12} md={6}>
              <UserInfoEditor user={user} setUser={setUser} />
            </Grid>
          </Grid>
        </Paper>
      </Grid>

      <Grid item xs={12} md={10} lg={8}>
        <UserRoles user={user} />
      </Grid>
    </Grid>
  );
}
