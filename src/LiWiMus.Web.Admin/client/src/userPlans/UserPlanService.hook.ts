import { UserPlan } from "./types/UserPlan";
import { UpdateUserPlanDto } from "./types/UpdateUserPlanDto";
import { CreateUserPlanDto } from "./types/CreateUserPlanDto";
import { useAxios } from "../shared/hooks/Axios.hook";

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

export const useUserPlanService = () => {
  const axios = useAxios("/userPlans");

  const search = async (
    userId?: number | string,
    planId?: number | string,
    active?: boolean
  ): Promise<UserPlan[]> => {
    const response = await axios.get("", {
      params: { userId, planId, active },
    });
    return response.data.map((x: any) => map(x));
  };

  const get = async (id: string | number) => {
    const response = await axios.get(`/${id}`);
    return map(response.data);
  };

  const update = async (id: string | number, dto: UpdateUserPlanDto) => {
    const response = await axios.patch(`/${id}`, dto);
    return map(response.data);
  };

  const create = async (dto: CreateUserPlanDto) => {
    const response = await axios.post(``, dto);
    return map(response.data);
  };

  return { search, get, update, create };
};
