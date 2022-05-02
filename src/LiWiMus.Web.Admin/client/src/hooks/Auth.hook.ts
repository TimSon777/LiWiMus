import { useState, useCallback, useEffect } from "react";
import jwtDecode from "jwt-decode";
import { UserData } from "../types/UserData";

const storageName = "userData";

type JwtPayload = {
  sub: string,
  name: string,
  email: string,
  role: string[],
  Permission: string[]
}

export const useAuth = () => {
  const [userData, setUserData] = useState<UserData | null>(null);
  const [ready, setReady] = useState<boolean>(false);

  const login = useCallback((jwtToken: string) => {
    const payload = jwtDecode<JwtPayload>(jwtToken);
    if (!payload.role.includes("Admin")) {
      return false;
    }

    const data = {
      token: jwtToken,
      id: payload.sub,
      name: payload.name,
      email: payload.email,
      role: payload.role,
      permissions: payload.Permission
    };

    setUserData(data);

    localStorage.setItem(
      storageName,
      JSON.stringify(data)
    );
    return true;
  }, []);

  const logout = useCallback(() => {
    setUserData(null);
    localStorage.removeItem(storageName);
  }, []);

  useEffect(() => {
    const dataRaw = localStorage.getItem(storageName);
    if (dataRaw) {
      const data: UserData = JSON.parse(dataRaw);
      if (data) {
        login(data.token);
      }
    }

    setReady(true);
  }, []);

  return { login, logout, userData, ready };
};
