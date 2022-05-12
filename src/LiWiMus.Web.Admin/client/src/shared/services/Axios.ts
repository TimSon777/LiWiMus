import axios from "axios";

const baseUrl = process.env.REACT_APP_API_URL;

const instance = axios.create({
  baseURL: baseUrl,
});

export default instance;
