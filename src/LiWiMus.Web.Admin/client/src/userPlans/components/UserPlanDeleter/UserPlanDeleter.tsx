import React from "react";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { UserPlan } from "../../types/UserPlan";
import { Action } from "../../../shared/types/Action";
import { UpdateUserPlanDto } from "../../types/UpdateUserPlanDto";
import AlertDialog from "../../../shared/components/AlertDialog/AlertDialog";
import { useUserPlanService } from "../../UserPlanService.hook";

type Props = {
  userPlan: UserPlan;
  setUserPlan: Action<UserPlan>;
};

export default function UserPlanDeleter({ userPlan, setUserPlan }: Props) {
  const userPlanService = useUserPlanService();

  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      const dto: UpdateUserPlanDto = { end: new Date() };
      const result = await userPlanService.update(userPlan.id, dto);
      setUserPlan(result);
      showSuccess("UserPlan end date set to now");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return (
    <AlertDialog
      onAgree={deleteHandler}
      buttonText={"Disable"}
      disagreeText={"Cancel"}
      agreeText={"Disable"}
      title={`Disable UserPlan?`}
      text={"End date will be set to now"}
    />
  );
}
