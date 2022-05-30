import { useAxios } from "./Axios.hook";

const API_URL = process.env.REACT_APP_API_URL;

export const useFileService = () => {
  const axios = useAxios();

  const remove = (location: string) => axios.delete(location);

  const save = async (file: File) => {
    const formData = new FormData();
    formData.append("file", file);
    const { data } = await axios.post("/files", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return data.location as string;
  };

  const getLocation = (relativeLocation: string) => API_URL + relativeLocation;

  const saveByUrl = async (url: string) => {
    const response = await axios.post("/files/url", { url });
    return response.data.location as string;
  };

  return { remove, save, saveByUrl, getLocation };
};
