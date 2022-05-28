import axios from "../shared/services/Axios";
import { UserPlan } from "./types/UserPlan";
import { UpdateUserPlanDto } from "./types/UpdateUserPlanDto";
import { CreateUserPlanDto } from "./types/CreateUserPlanDto";

const map = (data: any) =>
  new UserPlan(
    data.id,
    data.planName,
    data.planDescription,
    data.planId,
    data.userName,
    data.userId,
    data.start,
    data.end,
    data.createdAt,
    data.modifiedAt,
    data.updatable
  );

const UserPlanService = {
  search: async (
    userId?: number | string,
    planId?: number | string,
    active?: boolean
  ): Promise<UserPlan[]> => {
    const response = await axios.get("/userPlans", {
      params: { userId, planId, active },
    });
    return response.data.map((x: any) => map(x));
  },

  get: async (id: string | number) => {
    const response = await axios.get(`/userPlans/${id}`);
    return map(response.data);
  },

  update: async (id: string | number, dto: UpdateUserPlanDto) => {
    const response = await axios.patch(`/userPlans/${id}`, dto);
    return map(response.data);
  },

  create: async (dto: CreateUserPlanDto) => {
    const response = await axios.post(`/userPlans`, dto);
    return map(response.data);
  },
};

export default UserPlanService;
