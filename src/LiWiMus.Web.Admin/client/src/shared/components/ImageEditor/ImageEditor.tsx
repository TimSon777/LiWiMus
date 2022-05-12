import React, { useCallback } from "react";
import styles from "./ImageEditor.module.sass";
import { Box, Button } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";

export type ImageEditorProps = {
  src?: string | undefined;
  alt?: string | undefined;
  width: number;
  handler1: (input: HTMLInputElement) => void;
  handler2?: (input: HTMLInputElement) => void;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  button1Text?: string;
  button2Text?: string;
};

export default function ImageEditor({
  src,
  alt,
  width,
  handler1,
  handler2,
  onChange,
  button1Text = "Choose photo",
  button2Text = "Remove photo",
}: ImageEditorProps) {
  const callHandler = useCallback(
    (handler: (input: HTMLInputElement) => void) => {
      const input = document.getElementById(styles.input) as HTMLInputElement;
      handler(input);
    },
    []
  );

  const size = {
    xs: width * 0.75,
    md: width * 0.875,
    lg: width,
  };

  return (
    <Box className={styles.container} width={size} height={size}>
      <img src={src} alt={alt ?? "Image editor"} className={styles.image} />
      <input
        type="file"
        id={styles.input}
        onChange={onChange}
        name={"file"}
        accept={"image/png, image/gif, image/jpeg"}
      />
      <div className={styles.controls}>
        <Button
          variant="text"
          color={"secondary"}
          onClick={() => callHandler(handler1)}
        >
          {button1Text}
        </Button>
        {handler2 ? (
          <>
            <EditIcon className={styles.icon} />
            <Button
              variant="text"
              color={"secondary"}
              // @ts-ignore
              onClick={() => callHandler(handler2)}
            >
              {button2Text}
            </Button>
          </>
        ) : (
          ""
        )}
      </div>
    </Box>
  );
}
