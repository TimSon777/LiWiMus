import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Plan } from "../types/Plan";
import PlanService from "../Plan.service";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Paper, Stack, Typography } from "@mui/material";
import PlanInfoEditor from "../components/PlanInfoEditor/PlanInfoEditor";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import { formatDistanceToNow } from "date-fns";
import PlanDeleter from "../components/PlanDeleter/PlanDeleter";

export default function PlanDetailsPage() {
  const { id } = useParams() as { id: string };
  const [plan, setPlan] = useState<Plan>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    setLoading(true);
    PlanService.get(id)
      .then((plan) => setPlan(plan))
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  if (!plan) {
    return <NotFound />;
  }

  return (
    <Grid container spacing={5} justifyContent={"center"} sx={{ py: 5 }}>
      <Grid item xs={12} md={10} lg={8}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            {plan.name}
          </Typography>

          <Grid container spacing={6}>
            <Grid item xs={12} md={6}>
              <Stack spacing={2} alignItems={"end"}>
                <ReadonlyInfo name={"ID"} value={plan.id} />
                <ReadonlyInfo
                  name={"Created"}
                  value={formatDistanceToNow(plan.createdAt, {
                    addSuffix: true,
                  })}
                />
                <ReadonlyInfo
                  name={"Modified"}
                  value={formatDistanceToNow(plan.modifiedAt, {
                    addSuffix: true,
                  })}
                />
                <PlanDeleter plan={plan} setPlan={setPlan} />
              </Stack>
            </Grid>

            <Grid item xs={12} md={6}>
              <PlanInfoEditor plan={plan} setPlan={setPlan} />
            </Grid>
          </Grid>
        </Paper>
      </Grid>
    </Grid>
  );
}
