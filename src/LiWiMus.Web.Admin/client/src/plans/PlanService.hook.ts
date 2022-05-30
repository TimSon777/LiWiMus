import { Plan } from "./types/Plan";
import { UpdatePlanDto } from "./types/UpdatePlanDto";
import { Permission } from "../permissions/Permission";
import { CreatePlanDto } from "./types/CreatePlanDto";
import { User } from "../users/types/User";
import { useAxios } from "../shared/hooks/Axios.hook";
import { useUserPlanService } from "../userPlans/UserPlanService.hook";

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

export const usePlanService = () => {
  const axios = useAxios("/plans");
  const userPlanService = useUserPlanService();

  const get = async (id: number | string) => {
    const response = await axios.get(`/${id}`);
    return map(response.data);
  };

  const getPlans = async () => {
    const response = await axios.get(``);
    return response.data;
  };

  const getAll = async (): Promise<Plan[]> => {
    const response = await axios.get(``);
    return response.data.map((x: any) => map(x));
  };

  const remove = async (plan: Plan) => {
    return await axios.delete(`/${plan.id}`);
  };

  const update = async (dto: UpdatePlanDto) => {
    const response = await axios.patch("", dto);
    return map(response.data);
  };

  const save = async (plan: CreatePlanDto) => {
    const response = await axios.post(``, plan);
    return response.data as Plan;
  };

  const replacePermissions = async (plan: Plan, permissions: Permission[]) => {
    const dto = {
      planId: plan.id,
      permissions: permissions.map((p) => p.id),
    };
    const response = await axios.put("/permissions", dto);
    return map(response.data);
  };

  const getAvailablePlans = async (user: User) => {
    const userPlans = await userPlanService.search(user.id, undefined, true);
    const plansIds = userPlans.map((up) => up.planId);
    const allPlans = await getAll();
    return allPlans.filter((p) => !plansIds.includes(p.id));
  };

  return {
    remove,
    update,
    getAvailablePlans,
    save,
    replacePermissions,
    getAll,
    get,
    getPlans,
  };
};
