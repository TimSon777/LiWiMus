import React, { useEffect, useState } from "react";
import { Plan } from "../../types/Plan";
import { Permission } from "../../../permissions/Permission";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Loading from "../../../shared/components/Loading/Loading";
import {
  Box,
  Button,
  Checkbox,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Stack,
  Tooltip
} from "@mui/material";
import InfoIcon from "@mui/icons-material/Info";
import { usePlanService } from "../../PlanService.hook";
import { usePermissionService } from "../../../permissions/PermissionService.hook";

type Prop = {
  plan: Plan;
  setPlan: (plan: Plan) => void;
};

export default function PlanPermissionsEditor({ plan, setPlan }: Prop) {
  const planService = usePlanService();
  const permissionService = usePermissionService();

  const [permissions, setPermissions] = useState<Permission[]>();
  const [loading, setLoading] = useState(true);
  const [checked, setChecked] = useState(plan.permissions);
  const { showError, showSuccess } = useNotifier();

  useEffect(() => {
    setLoading(true);
    permissionService
      .getAll()
      .then(setPermissions)
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  const handleToggle = (permission: Permission) => {
    const currentIndex = checked.map((p) => p.id).indexOf(permission.id);
    const newChecked = [...checked];

    if (currentIndex === -1) {
      newChecked.push(permission);
    } else {
      newChecked.splice(currentIndex, 1);
    }

    setChecked(newChecked);
  };

  const rollbackHandler = () => {
    setChecked(plan.permissions);
  };

  const saveHandler = async () => {
    try {
      const response = await planService.replacePermissions(plan, checked);
      setPlan({ ...plan, ...response });
      showSuccess("Permissions updated");
    } catch (e) {
      showError(e);
    }
  };

  const isChanged =
    JSON.stringify(checked) !== JSON.stringify(plan.permissions);

  return (
    <Box display={"flex"} flexDirection={"column"}>
      <List>
        {permissions?.map((permission, index) => (
          <ListItem
            key={index}
            disablePadding
            secondaryAction={
              <Tooltip title={permission.description} placement={"left"} arrow>
                <IconButton>
                  <InfoIcon />
                </IconButton>
              </Tooltip>
            }
          >
            <ListItemButton onClick={() => handleToggle(permission)} dense>
              <ListItemIcon>
                <Checkbox
                  checked={
                    checked.map((p) => p.id).indexOf(permission.id) !== -1
                  }
                  disableRipple
                />
              </ListItemIcon>
              <ListItemText primary={permission.name} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>

      {isChanged && (
        <Stack direction={"row"} sx={{ alignSelf: "flex-end" }} spacing={2}>
          <Button
            onClick={rollbackHandler}
            variant="text"
            color={"secondary"}
            sx={{ borderRadius: "20px", px: 4 }}
          >
            Rollback
          </Button>
          <Button
            onClick={saveHandler}
            variant="contained"
            sx={{ borderRadius: "20px", px: 4 }}
          >
            Save
          </Button>
        </Stack>
      )}
    </Box>
  );
}
