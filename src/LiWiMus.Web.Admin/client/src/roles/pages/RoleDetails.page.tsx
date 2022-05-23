import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import {Role} from "../types/Role";
import RoleService from "../Role.service";
import {useNotifier} from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import {Grid, Paper, Stack, Typography} from "@mui/material";
import RoleInfoEditor from "../components/RoleInfoEditor/RoleInfoEditor";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import {formatDistanceToNow} from "date-fns";
import RoleDeleter from "../components/RoleDeleter/RoleDeleter";
import RolePermissionsEditor from "../components/RolePermissionsEditor/RolePermissionsEditor";

export default function RoleDetailsPage() {
  const { id } = useParams() as { id: string };
  const [role, setRole] = useState<Role>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    setLoading(true);
    RoleService.get(id)
      .then((role) => setRole(role))
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  if (!role) {
    return <NotFound />;
  }

  return (
    <Grid container spacing={5} justifyContent={"center"} sx={{ py: 5 }}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            {role.name}
          </Typography>

          <Grid container spacing={6}>
            <Grid item xs={12} md={6}>
              <Stack spacing={2} alignItems={"end"}>
                <ReadonlyInfo name={"ID"} value={role.id} />
                <ReadonlyInfo
                  name={"Created"}
                  value={formatDistanceToNow(role.createdAt, {
                    addSuffix: true,
                  })}
                />
                <ReadonlyInfo
                  name={"Modified"}
                  value={formatDistanceToNow(role.modifiedAt, {
                    addSuffix: true,
                  })}
                />
                <RoleDeleter role={role} setRole={setRole} />
              </Stack>
            </Grid>

            <Grid item xs={12} md={6}>
              <RoleInfoEditor role={role} setRole={setRole} />
            </Grid>
          </Grid>
        </Paper>
      </Grid>

      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            Permissions
          </Typography>

          <RolePermissionsEditor role={role} setRole={setRole} />
        </Paper>
      </Grid>
    </Grid>
  );
}