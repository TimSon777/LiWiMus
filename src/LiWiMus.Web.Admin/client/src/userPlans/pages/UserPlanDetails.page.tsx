import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { UserPlan } from "../types/UserPlan";
import UserPlanService from "../UserPlan.service";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Paper, Stack, Typography } from "@mui/material";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import { formatDistanceToNow } from "date-fns";
import UserPlanInfoEditor from "../components/UserPlanInfoEditor/UserPlanInfoEditor";
import UserPlanDeleter from "../components/UserPlanDeleter/UserPlanDeleter";

export default function UserPlanDetailsPage() {
  const { id } = useParams() as { id: string };
  const [userPlan, setUserPlan] = useState<UserPlan>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    setLoading(true);
    UserPlanService.get(id)
      .then(setUserPlan)
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  if (!userPlan) {
    return <NotFound />;
  }

  return (
    <Grid container spacing={5} justifyContent={"center"} sx={{ py: 5 }}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            UserPlan
          </Typography>

          <Grid container spacing={6}>
            <Grid item xs={12} md={6}>
              <Stack spacing={3} alignItems={"end"}>
                <ReadonlyInfo name={"ID"} value={userPlan.id} />
                <ReadonlyInfo
                  name={"Created"}
                  value={formatDistanceToNow(userPlan.createdAt, {
                    addSuffix: true,
                  })}
                />
                <ReadonlyInfo
                  name={"Modified"}
                  value={formatDistanceToNow(userPlan.modifiedAt, {
                    addSuffix: true,
                  })}
                />
                {userPlan.updatable && (
                  <UserPlanDeleter
                    userPlan={userPlan}
                    setUserPlan={setUserPlan}
                  />
                )}
              </Stack>
            </Grid>

            <Grid item xs={12} md={6}>
              <UserPlanInfoEditor
                userPlan={userPlan}
                setUserPlan={setUserPlan}
              />
            </Grid>
          </Grid>
        </Paper>
      </Grid>
    </Grid>
  );
}
