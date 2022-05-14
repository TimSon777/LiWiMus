import axios from "./Axios";

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
};

export default FileService;
