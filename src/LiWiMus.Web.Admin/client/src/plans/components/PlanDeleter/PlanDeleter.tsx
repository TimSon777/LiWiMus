import React from "react";
import { Plan } from "../../types/Plan";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { usePlanService } from "../../PlanService.hook";

type Props = {
  plan: Plan;
  setPlan: (plan: Plan | undefined) => void;
};

export default function PlanDeleter({ plan, setPlan }: Props) {
  const planService = usePlanService();

  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await planService.remove(plan);
      setPlan(undefined);
      showSuccess("Plan deleted");

      navigate("/admin/plans");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={plan.name} deleteHandler={deleteHandler} />;
}
