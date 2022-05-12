import React, { useCallback, useEffect, useState } from "react";
import styles from "./ImageEditor.module.sass";
import { Box, Button } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import { IWithPhoto } from "../../types/IWithPhoto";

const API_URL = process.env.REACT_APP_API_URL;

export type ImageEditorProps = {
  src?: string | null;
  width: number;
  updatePhoto: (photo: File) => Promise<IWithPhoto>;
  removePhoto: () => Promise<IWithPhoto>;
  imagePlaceholderSrc: string;
};

export default function ImageEditor(props: ImageEditorProps) {
  const [src, setSrc] = useState<string | undefined>();

  const updateSrc = useCallback(
    (newSrc: string | null | undefined) => {
      if (!newSrc) {
        setSrc(props.imagePlaceholderSrc);
      } else {
        setSrc(`${API_URL}${newSrc}`);
      }
    },
    [props.imagePlaceholderSrc]
  );

  useEffect(() => updateSrc(props.src), [updateSrc, props.src]);

  const size = {
    xs: props.width * 0.75,
    md: props.width * 0.875,
    lg: props.width,
  };

  async function changeImage(event: React.ChangeEvent<HTMLInputElement>) {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      event.target.form?.reset();
      const res = await props.updatePhoto(file);
      updateSrc(res.photoLocation);
    }
  }

  const removePhotoHandler = async () => {
    const input = document.getElementById(styles.input) as HTMLInputElement;
    input.value = "";
    const response = await props.removePhoto();
    updateSrc(response.photoLocation);
  };

  return (
    <Box className={styles.container} width={size} height={size}>
      <img src={src} alt="" className={styles.image} />
      <input
        type="file"
        id={styles.input}
        onChange={changeImage}
        name={"file"}
        accept={"image/png, image/gif, image/jpeg"}
      />
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
