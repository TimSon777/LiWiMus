import { useSnackbar } from "notistack";
import { getMessage } from "./Axios.hook";

export const useNotifier = () => {
  const { enqueueSnackbar } = useSnackbar();

  // @ts-ignore
  const showError = (error) => {
    enqueueSnackbar(getMessage(error), { variant: "error" });
  };

  const showInfo = (text: string) => {
    enqueueSnackbar(text, { variant: "info" });
  };

  const showSuccess = (text: string) => {
    enqueueSnackbar(text, { variant: "success" });
  };

  const showWarning = (text: string) => {
    enqueueSnackbar(text, { variant: "warning" });
  };

  const showDefault = (text: string) => {
    enqueueSnackbar(text, { variant: "default" });
  };

  return { showDefault, showInfo, showSuccess, showError, showWarning };
};
