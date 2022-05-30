import React, { useEffect, useState } from "react";
import { User } from "../../types/User";
import { UserPlan } from "../../../userPlans/types/UserPlan";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Loading from "../../../shared/components/Loading/Loading";
import { Button, Grid, Paper, Typography } from "@mui/material";
import ActionCard from "../../../shared/components/ActionCard/ActionCard";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import { Link } from "react-router-dom";
import { useUserPlanService } from "../../../userPlans/UserPlanService.hook";

type Props = {
  user: User;
};

export default function UserPlans({ user }: Props) {
  const userPlanService = useUserPlanService();

  const [userPlans, setUserPlans] = useState<UserPlan[]>([]);
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    setLoading(true);
    userPlanService
      .search(user.id, undefined, true)
      .then(setUserPlans)
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  return (
    <>
      <Typography variant={"h4"} component={"div"}>
        Plans
      </Typography>
      <Grid container spacing={3}>
        {userPlans?.map((plan) => (
          <Grid item xs={4} md={3} lg={2} key={plan.id}>
            <ActionCard
              text={plan.planName}
              action={
                <Button
                  variant="text"
                  color={"secondary"}
                  sx={{
                    borderRadius: "50%",
                    width: "48px",
                    height: "48px",
                    minWidth: "48px",
                  }}
                  component={Link}
                  to={`/admin/userPlans/${plan.id}`}
                >
                  <EditIcon sx={{ fontSize: "2rem" }} />
                </Button>
              }
            />
          </Grid>
        ))}
        <Grid item xs={4} md={3} lg={2}>
          <Paper
            sx={{ width: "100%", pb: "100%", position: "relative" }}
            elevation={10}
          >
            <Button
              variant="text"
              color={"secondary"}
              sx={{
                borderRadius: "50%",
                width: "48px",
                height: "48px",
                minWidth: "48px",
                position: "absolute",
                top: "50%",
                left: "50%",
                transform: "translate(-50%, -50%)",
              }}
              component={Link}
              to={`/admin/userPlans/create/${user.id}`}
            >
              <AddIcon sx={{ fontSize: "2rem" }} />
            </Button>
          </Paper>
        </Grid>
      </Grid>
    </>
  );
}
