import React, {useEffect, useState} from "react";
import {Role} from "../../types/Role";
import {useNotifier} from "../../../shared/hooks/Notifier.hook";
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
    Tooltip,
} from "@mui/material";
import InfoIcon from "@mui/icons-material/Info";
import RoleService from "../../Role.service";
import {SystemPermission} from "../../../systemPermissions/SystemPermission";
import SystemPermissionService from "../../../systemPermissions/SystemPermission.service";

type Prop = {
  role: Role;
  setRole: (role: Role) => void;
};

export default function RolePermissionsEditor({ role, setRole }: Prop) {
  const [permissions, setPermissions] = useState<SystemPermission[]>();
  const [loading, setLoading] = useState(true);
  const [checked, setChecked] = useState(role.permissions);
  const { showError, showSuccess } = useNotifier();

  useEffect(() => {
    setLoading(true);
    SystemPermissionService.getAll()
      .then(setPermissions)
      .catch(showError)
      .then(() => setLoading(false));
  }, []);

  if (loading) {
    return <Loading />;
  }

  const handleToggle = (permission: SystemPermission) => {
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
    setChecked(role.permissions);
  };

  const saveHandler = async () => {
    try {
      const response = await RoleService.replacePermissions(role, checked);
      setRole({ ...role, ...response });
      showSuccess("Permissions updated");
    } catch (e) {
      showError(e);
    }
  };

  const isChanged =
    JSON.stringify(checked) !== JSON.stringify(role.permissions);

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