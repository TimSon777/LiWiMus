import React, { ReactElement } from "react";
import { User } from "../../types/User";
import {
  Avatar,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import EditIcon from "@mui/icons-material/Edit";
import FileService from "../../../shared/services/File.service";
import { format } from "date-fns";
import UserLink from "../UserLink/UserLink";

type Props = {
  users: User[];
  userName?: boolean;
  email?: boolean;
  firstName?: boolean;
  secondName?: boolean;
  patronymic?: boolean;
  birthDate?: boolean;
  gender?: boolean;
  balance?: boolean;
  avatar?: boolean;
  createdAt?: boolean;
  modifiedAt?: boolean;
  action?: (user: User) => ReactElement;
};

export default function UsersList({
  users,
  userName,
  firstName,
  secondName,
  avatar,
  balance,
  birthDate,
  email,
  gender,
  patronymic,
  modifiedAt,
  createdAt,
  action,
}: Props) {
  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell>#</TableCell>
            {avatar && (
              <TableCell>
                <AccountCircleIcon />
              </TableCell>
            )}
            {userName && <TableCell>UserName</TableCell>}
            {createdAt && <TableCell>Created at</TableCell>}
            {modifiedAt && <TableCell>Modified at</TableCell>}
            {action && (
              <TableCell>
                <EditIcon />
              </TableCell>
            )}
          </TableRow>
        </TableHead>
        <TableBody>
          {users.map((user, index) => (
            <TableRow key={index}>
              <TableCell>{index + 1}</TableCell>
              {avatar && (
                <TableCell>
                  <Avatar src={FileService.getLocation(user.avatarLocation)} />
                </TableCell>
              )}
              {userName && (
                <TableCell>
                  <UserLink user={user} />
                </TableCell>
              )}
              {createdAt && (
                <TableCell>
                  {format(new Date(user.createdAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {modifiedAt && (
                <TableCell>
                  {format(new Date(user.modifiedAt), "dd.MM.yyyy HH:mm")}
                </TableCell>
              )}
              {action && <TableCell>{action(user)}</TableCell>}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
