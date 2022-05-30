import React, { useState } from "react";
import { Role } from "../../../roles/types/Role";
import { User } from "../../types/User";
import { Action } from "../../../shared/types/Action";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import { Box, IconButton, List, ListItem, ListItemText, Modal, Paper } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import { useRoleService } from "../../../roles/RoleService.hook";
import { useUserService } from "../../UserService.hook";

type Props = {
  user: User;
  userRoles: Role[];
  setUserRoles: Action<Role[]>;
};

const modalStyle = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  overflow: "hidden",
  width: { xs: "90%", sm: "75%", md: "60%", lg: "45%", xl: "30%" },
  bgcolor: "background.paper",
  boxShadow: 24,
  p: 4,
  display: "block",
  border: 1,
  borderColor: "secondary",
};

const contentStyle = {
  overflow: "auto",
  height: "300px",
};

export default function AddRole({ user, userRoles, setUserRoles }: Props) {
  const roleService = useRoleService();
  const userService = useUserService();

  const [roles, setRoles] = useState<Role[]>([]);
  const [open, setOpen] = useState(false);
  const { showError, showSuccess } = useNotifier();

  const handleOpen = async () => {
    try {
      const roles = (await roleService.getAll()).filter(
        (r) => userRoles.map((ur) => ur.id).indexOf(r.id) === -1
      );
      setRoles(roles);
    } catch (e) {
      showError(e);
    } finally {
      setOpen(true);
    }
  };

  const handleClose = () => {
    setOpen(false);
  };

  const addRole = async (role: Role) => {
    try {
      await userService.addRole(user, role);
      setUserRoles([...userRoles, role]);
      setRoles(roles.filter((r) => r.id !== role.id));
      showSuccess("User added into role");
    } catch (e) {
      showError(e);
    }
  };

  return (
    <>
      <Paper
        sx={{ width: "100%", pb: "100%", position: "relative" }}
        elevation={10}
      >
        <IconButton
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
          }}
          onClick={handleOpen}
        >
          <AddIcon sx={{ fontSize: "2rem" }} />
        </IconButton>
      </Paper>

      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalStyle}>
          <List sx={contentStyle} id={"scrollableDiv"} dense>
            {roles.map((role, index) => (
              <ListItem
                key={index}
                secondaryAction={
                  <IconButton edge="end" onClick={() => addRole(role)}>
                    <AddIcon />
                  </IconButton>
                }
              >
                <ListItemText
                  primary={role.name}
                  primaryTypographyProps={{ variant: "h6" }}
                />
              </ListItem>
            ))}
          </List>
        </Box>
      </Modal>
    </>
  );
}
