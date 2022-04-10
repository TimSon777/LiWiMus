import { useState, useCallback, useEffect } from "react";

const storageName = "userData";

export const useAuth = () => {
  const [token, setToken] = useState<string | null>(null);
  const [userId, setUserId] = useState<string | null>(null);
  const [ready, setReady] = useState<boolean>(false);

  const login = useCallback((jwtToken: string, uid: string) => {
    setToken(jwtToken);
    setUserId(uid);

    localStorage.setItem(
      storageName,
      JSON.stringify({ userId: uid, token: jwtToken })
    );
  }, []);

  const logout = useCallback(() => {
    setToken(null);
    setUserId(null);
    localStorage.removeItem(storageName);
  }, []);

  useEffect(() => {
    const data: { token: string; userId: string } = JSON.parse(
      localStorage.getItem(storageName) || "{}"
    );

    if (data && data.token && data.userId) {
      login(data.token, data.userId);
    }
    setReady(true);
  }, []);

  return { login, logout, token, userId, ready };
};
