export type UpdateUserDto = {
  id: number;
  firstName?: string;
  secondName?: string;
  patronymic?: string;
  gender?: "Female" | "Male";
  birthDate?: string;
  avatarLocation?: string;
};
