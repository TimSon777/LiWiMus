import { User } from "../../users/types/User";

export type Transaction = {
  id: string;
  user: User;
  amount: string;
  description: string;

  createdAt: string;
  modifiedAt: string;
};
