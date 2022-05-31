import { useCallback, useEffect, useState } from "react";
import jwtDecode from "jwt-decode";
import { User } from "../../users/types/User";
import axios from "axios";

const storageName = "userData";
const API_URL = process.env.REACT_APP_API_URL;

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
      const response = await axios
        .create({
          baseURL: API_URL,
          headers: { Authorization: `Bearer ${jwtToken}` },
        })
        .get(`/users/${userId}`);
      const user = new User(
        response.data.id,
        response.data.userName,
        response.data.email,
        response.data.emailConfirmed,
        response.data.firstName,
        response.data.secondName,
        response.data.patronymic,
        response.data.birthDate,
        response.data.gender,
        response.data.balance,
        response.data.avatarLocation,
        response.data.createdAt,
        response.data.modifiedAt,
        response.data.lockoutEnd
      );

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
