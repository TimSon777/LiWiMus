import React from "react";
import { Role } from "../../types/Role";
import { useNavigate } from "react-router-dom";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Deleter from "../../../shared/components/Deleter/Deleter";
import { useRoleService } from "../../RoleService.hook";

type Props = {
  role: Role;
  setRole: (role: Role | undefined) => void;
};

export default function RoleDeleter({ role, setRole }: Props) {
  const roleService = useRoleService();

  const navigate = useNavigate();
  const { showSuccess, showError } = useNotifier();

  const deleteHandler = async () => {
    try {
      await roleService.remove(role);
      setRole(undefined);
      showSuccess("Role deleted");

      navigate("/admin/roles");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  return <Deleter itemName={role.name} deleteHandler={deleteHandler} />;
}
