import axios from "../shared/services/Axios";
import { Plan } from "./types/Plan";
import { UpdatePlanDto } from "./types/UpdatePlanDto";
import { Permission } from "../permissions/Permission";

const map = (data: any) => {
  return new Plan(
    data.id,
    data.name,
    data.description,
    data.pricePerMonth,
    data.permissions,
    data.createdAt,
    data.modifiedAt
  );
};

const PlanService = {
  get: async (id: number | string) => {
    const response = await axios.get(`/plans/${id}`);
    return map(response.data);
  },

  remove: async (plan: Plan) => {
    return await axios.delete(`/plans/${plan.id}`);
  },

  update: async (dto: UpdatePlanDto) => {
    const response = await axios.patch("/plans", dto);
    return map(response.data);
  },

  replacePermissions: async (plan: Plan, permissions: Permission[]) => {
    const dto = {
      planId: plan.id,
      permissions: permissions.map((p) => p.id),
    };
    const response = await axios.put("/plans/permissions", dto);
    return map(response.data);
  },
};

export default PlanService;
