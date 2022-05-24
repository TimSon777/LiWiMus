import axios from "./Axios";

const API_URL = process.env.REACT_APP_API_URL;

const FileService = {
  remove: (location: string) => axios.delete(location),
  save: async (file: File) => {
    const formData = new FormData();
    formData.append("file", file);
    const { data } = await axios.post("/files", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return data.location as string;
  },
  getLocation: (relativeLocation: string) => API_URL + relativeLocation,

  saveByUrl: async (url: string) => {
    const response = await axios.post("/files/url", { url });
    return response.data.location as string;
  },
};

export default FileService;
