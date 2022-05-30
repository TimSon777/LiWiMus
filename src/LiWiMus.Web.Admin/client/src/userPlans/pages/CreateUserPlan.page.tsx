import React, { useEffect, useState } from "react";
import { User } from "../../users/types/User";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import { useNavigate, useParams } from "react-router-dom";
import {
  Button,
  FormControl,
  FormHelperText,
  Grid,
  InputLabel,
  MenuItem,
  Paper,
  Select,
  Stack,
  Typography
} from "@mui/material";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import UserLink from "../../users/components/UserLink/UserLink";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { Plan } from "../../plans/types/Plan";
import { DateTimePicker } from "@mui/x-date-pickers";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import { useUserPlanService } from "../UserPlanService.hook";
import { usePlanService } from "../../plans/PlanService.hook";
import { useUserService } from "../../users/UserService.hook";

type Inputs = {
  userId: number;
  planId: number | "";
  end: Date | null;
};

export default function CreateUserPlanPage() {
  const userPlanService = useUserPlanService();
  const planService = usePlanService();
  const userService = useUserService();

  const { userId } = useParams() as { userId: string };
  const [user, setUser] = useState<User>();
  const [loading, setLoading] = useState(true);
  const { showError, showSuccess } = useNotifier();
  const [availablePlans, setAvailablePlans] = useState<Plan[]>();
  const navigate = useNavigate();

  const {
    handleSubmit,
    formState: { errors },
    control,
  } = useForm<Inputs>({
    defaultValues: { userId: +userId, planId: "", end: null },
  });

  const fetchData = async () => {
    const user = await userService.get(userId);
    setUser(user);

    const userPlans = await userPlanService.search(userId, undefined, true);
    const allPlans = await planService.getAll();
    const available = allPlans.filter(
      (p) => !userPlans.map((up) => up.planId).includes(p.id)
    );
    setAvailablePlans(available);
  };

  useEffect(() => {
    setLoading(true);
    fetchData()
      .then(() => {})
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  const handleCreate: SubmitHandler<Inputs> = async (data) => {
    try {
      // @ts-ignore
      const result = await userPlanService.create({
        ...data,
        start: new Date(),
      });
      showSuccess("UserPlan created");
      navigate(`/admin/userPlans/${result.id}`);
    } catch (e) {
      showError(e);
    }
  };

  if (loading) {
    return <Loading />;
  }

  if (!user) {
    return <NotFound />;
  }

  return (
    <Grid container spacing={5} justifyContent={"center"} sx={{ py: 5 }}>
      <Grid item xs={12} md={9} lg={6}>
        <Paper sx={{ p: 4 }} elevation={10}>
          <Typography variant={"h3"} component={"div"}>
            UserPlan
          </Typography>

          <form onSubmit={handleSubmit(handleCreate)}>
            <Stack spacing={3}>
              <ReadonlyInfo name={"User"} value={<UserLink user={user} />} />
              <Controller
                name="planId"
                control={control}
                rules={{
                  required: { value: true, message: "Required" },
                }}
                render={({ field }) => (
                  <FormControl fullWidth error={!!errors.planId}>
                    <InputLabel id="demo-simple-select-label">Plan</InputLabel>
                    <Select
                      value={field.value}
                      label="Plan"
                      onChange={field.onChange}
                    >
                      {availablePlans?.map((plan) => (
                        <MenuItem value={plan.id} key={plan.id}>
                          {plan.name}
                        </MenuItem>
                      ))}
                    </Select>
                    <FormHelperText>{errors.planId?.message}</FormHelperText>
                  </FormControl>
                )}
              />

              <Controller
                name={"end"}
                control={control}
                rules={{
                  required: { value: true, message: "Required" },
                  validate: (value) =>
                    !value || value > new Date() ? true : "Must be in future",
                }}
                render={({ field }) => (
                  <DateTimePicker
                    label={"End"}
                    InputProps={{
                      error: !!errors.end,
                    }}
                    minDateTime={new Date()}
                    renderInput={(params) => (
                      <ContrastTextField
                        helperText={errors.end?.message}
                        {...params}
                      />
                    )}
                    {...field}
                  />
                )}
              />

              <Button
                type={"submit"}
                variant="contained"
                sx={{ borderRadius: "20px", px: 4 }}
              >
                Create
              </Button>
            </Stack>
          </form>
        </Paper>
      </Grid>
    </Grid>
  );
}
