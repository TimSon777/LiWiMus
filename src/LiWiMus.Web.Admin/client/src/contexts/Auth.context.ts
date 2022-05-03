import { createContext } from "react";

function noop() {}

type AuthContextInfo = {
  token: string | null;
  userId: string | null;
  login: (jwtToken: string, uid: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
};

export const AuthContext = createContext<AuthContextInfo>({
  token: null,
  userId: null,
  login: noop,
  logout: noop,
  isAuthenticated: false,
});
