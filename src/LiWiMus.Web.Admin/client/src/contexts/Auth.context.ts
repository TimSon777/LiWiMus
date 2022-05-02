import { createContext } from "react";
import { UserData } from "../types/UserData";

function noop() {
  return false;
}

type AuthContextInfo = {
  userData: UserData | null;
  login: (jwtToken: string) => boolean;
  logout: () => void;
  isAuthenticated: boolean;
};

export const AuthContext = createContext<AuthContextInfo>({
  userData: null,
  login: noop,
  logout: noop,
  isAuthenticated: false,
});
