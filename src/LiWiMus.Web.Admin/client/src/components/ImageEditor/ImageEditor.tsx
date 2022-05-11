import React, { useCallback, useEffect, useState } from "react";
import { useSnackbar } from "notistack";
import styles from "./ImageEditor.module.sass";
import { Box, Button } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import { IWithPhoto } from "../../types/IWithPhoto";

const DATA_URL = process.env.REACT_APP_DATA_URL;

export type ImageEditorProps = {
  src?: string | null;
  width: number;
  updatePhoto: (data: FormData) => Promise<IWithPhoto | null>;
  removePhoto: () => Promise<IWithPhoto | null>;
  imagePlaceholderSrc: string;
};

export default function ImageEditor(props: ImageEditorProps) {
  const [src, setSrc] = useState<string | undefined>();
  const { enqueueSnackbar } = useSnackbar();

  const updateSrc = useCallback((newSrc: string | null | undefined) => {
    if (!newSrc) {
      setSrc(props.imagePlaceholderSrc);
    } else {
      setSrc(`${DATA_URL}/${newSrc}`);
    }
  }, []);

  useEffect(() => updateSrc(props.src), [props.src]);

  const size = {
    xs: props.width * 0.75,
    md: props.width * 0.875,
    lg: props.width,
  };

  async function changeImage(event: React.ChangeEvent<HTMLInputElement>) {
    if (event.target.files && event.target.files[0]) {
      let reader = new FileReader();
      reader.readAsDataURL(event.target.files[0]);
      const form = document.getElementById(styles.form) as HTMLFormElement;
      const formData = new FormData(form);
      // @ts-ignore
      const response = await props.updatePhoto(formData);
      if (response) {
        updateSrc(response.photoPath);
        enqueueSnackbar("Photo successfully updated", { variant: "success" });
      } else {
        enqueueSnackbar("Something went wrong", { variant: "error" });
      }
    }
  }

  const removePhotoHandler = async () => {
    const input = document.getElementById(styles.input) as HTMLInputElement;
    input.form?.reset();
    const response = await props.removePhoto();
    if (response) {
      updateSrc(response.photoPath);
      enqueueSnackbar("Photo successfully deleted", { variant: "success" });
    } else {
      enqueueSnackbar("Something went wrong", { variant: "error" });
    }
  };

  return (
    <Box className={styles.container} width={size} height={size}>
      <img src={src} alt="" className={styles.image} />
      <form id={styles.form}>
        <input
          type="file"
          id={styles.input}
          onChange={changeImage}
          name={"photo"}
        />
      </form>
      <div className={styles.controls}>
        <Button
          variant="text"
          color={"secondary"}
          onClick={() => {
            document.getElementById(styles.input)?.click();
          }}
        >
          Choose photo
        </Button>
        <EditIcon className={styles.icon} />
        <Button variant="text" color={"secondary"} onClick={removePhotoHandler}>
          Remove photo
        </Button>
      </div>
    </Box>
  );
}
