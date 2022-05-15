import axios from "axios";

const baseUrl = process.env.REACT_APP_API_URL;

const instance = axios.create({
  baseURL: baseUrl,
});

export default instance;

// @ts-ignore
const getMessage = (error): string => {
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
