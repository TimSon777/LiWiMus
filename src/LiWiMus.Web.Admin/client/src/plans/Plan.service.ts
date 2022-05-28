import axios from "../shared/services/Axios";
import { Plan } from "./types/Plan";
import { UpdatePlanDto } from "./types/UpdatePlanDto";
import { Permission } from "../permissions/Permission";
import { CreatePlanDto } from "./types/CreatePlanDto";
import { User } from "../users/types/User";
import UserPlanService from "../userPlans/UserPlan.service";

const map = (data: any) => {
  return new Plan(
    data.id,
    data.name,
    data.description,
    data.pricePerMonth,
    data.permissions,
    data.createdAt,
    data.modifiedAt,
    data.deletable
  );
};

const PlanService = {
  get: async (id: number | string) => {
    const response = await axios.get(`/plans/${id}`);
    return map(response.data);
  },

  getPlans: async () => {
    const response = await axios.get(`/plans`);
    return response.data;
  },

  getAll: async (): Promise<Plan[]> => {
    const response = await axios.get(`/plans`);
    return response.data.map((x: any) => map(x));
  },

  remove: async (plan: Plan) => {
    return await axios.delete(`/plans/${plan.id}`);
  },

  update: async (dto: UpdatePlanDto) => {
    const response = await axios.patch("/plans", dto);
    return map(response.data);
  },

  save: async (plan: CreatePlanDto) => {
    const response = await axios.post(`/plans`, plan);
    return response.data as Plan;
  },

  replacePermissions: async (plan: Plan, permissions: Permission[]) => {
    const dto = {
      planId: plan.id,
      permissions: permissions.map((p) => p.id),
    };
    const response = await axios.put("/plans/permissions", dto);
    return map(response.data);
  },

  getAvailablePlans: async (user: User) => {
    const userPlans = await UserPlanService.search(user.id, undefined, true);
    const plansIds = userPlans.map((up) => up.planId);
    const allPlans = await PlanService.getAll();
    return allPlans.filter((p) => !plansIds.includes(p.id));
  },
};

export default PlanService;
