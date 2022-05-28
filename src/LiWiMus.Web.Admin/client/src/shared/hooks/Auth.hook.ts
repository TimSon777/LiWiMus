import { useCallback, useEffect, useState } from "react";
import jwtDecode from "jwt-decode";
import UserService from "../../users/User.service";
import { User } from "../../users/types/User";

const storageName = "userData";

type JwtPayload = {
  sub: string;
  name: string;
  email: string;
  role: string[];
  sysperm: string[];
};

export const useAuth = () => {
  const [user, setUser] = useState<User | null>(null);
  const [ready, setReady] = useState<boolean>(false);
  const [payload, setPayload] = useState<JwtPayload | null>(null);
  const [token, setToken] = useState<string | null>(null);

  const login = useCallback(async (jwtToken: string) => {
    const payload = jwtDecode<JwtPayload>(jwtToken);
    if (!payload.sysperm || !payload.sysperm.includes("Admin.Access")) {
      return false;
    }

    const userId = payload.sub;
    try {
      const user = await UserService.get(userId);

      setUser(user);
    } catch (e) {
      console.error(e);
    }

    setToken(jwtToken);
    setPayload(payload);

    localStorage.setItem(storageName, jwtToken);
    return true;
  }, []);

  const logout = useCallback(() => {
    setUser(null);
    setPayload(null);
    setToken(null);
    localStorage.removeItem(storageName);
  }, []);

  useEffect(() => {
    const token = localStorage.getItem(storageName);
    if (token) {
      login(token)
        .then()
        .catch((e) => console.error(e))
        .then(() => setReady(true));
    } else {
      setReady(true);
    }
  }, []);

  return { login, logout, user, payload, token, setUser, ready };
};
