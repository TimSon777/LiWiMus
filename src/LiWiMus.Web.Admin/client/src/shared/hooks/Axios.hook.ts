import axios from "axios";
import Qs from "qs";
import axiosRetry from "axios-retry";
import { useContext } from "react";
import { AuthContext } from "../contexts/Auth.context";

const API_URL = process.env.REACT_APP_API_URL;

const getMessage = (error: any): string => {
  if (error.response) {
    // The request was made and the server responded with a status code
    // that falls out of the range of 2xx
    return error.response.statusText;
  } else if (error.request) {
    // The request was made but no response was received
    // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
    // http.ClientRequest in node.js
    return "The server is not responding";
  } else if (error.message) {
    // Something happened in setting up the request that triggered an Error
    return error.message;
  } else {
    return error;
  }
};

export { getMessage };

export const useAxios = (prefix?: string) => {
  const baseUrl = prefix ? API_URL!.concat(prefix) : API_URL;
  const authContext = useContext(AuthContext);

  if (authContext.token) {
  }
  const authHeader = authContext.token
    ? { Authorization: `Bearer ${authContext.token}` }
    : undefined;

  const instance = axios.create({
    baseURL: baseUrl,
    paramsSerializer: (params) => {
      return Qs.stringify(params, {
        arrayFormat: "indices",
        encode: false,
      });
    },
    headers: authHeader,
  });

  axiosRetry(instance, { retries: 3, retryDelay: () => 100 });

  return instance;
};
