import { createContext } from "react";
import { User } from "../../users/types/User";
import { Action } from "../types/Action";
import { JwtPayload } from "jwt-decode";

function noop() {
  return false;
}

export type AuthContextInfo = {
  user: User | null;
  login: (jwtToken: string) => Promise<boolean>;
  logout: () => void;
  isAuthenticated: boolean;
  token: string | null;
  setUser: Action<User>;
  payload: JwtPayload | null;
};

export const AuthContext = createContext<AuthContextInfo>({
  login: () => Promise.resolve(false),
  logout: noop,
  isAuthenticated: false,
  setUser: noop,
  user: null,
  token: null,
  payload: null,
});
