import React, { useCallback } from "react";
import { Button } from "@mui/material";
import HoverImage from "../HoverImage/HoverImage";
import styles from "./ImageEditor.module.sass";

export type ImageEditorProps = {
  src?: string | undefined;
  alt?: string | undefined;
  width: number;
  handler1: (input: HTMLInputElement) => void;
  handler2?: (input: HTMLInputElement) => void;
  handler3?: (input: HTMLInputElement) => void;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  button1Text?: string;
  button2Text?: string;
  button3Text?: string;
};

export default function ImageEditor({
  src,
  alt,
  width,
  handler1,
  handler2,
  handler3,
  onChange,
  button1Text = "Upload",
  button2Text = "Remove",
  button3Text = "Random",
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
    <>
      <input
        type="file"
        id={styles.input}
        onChange={onChange}
        name={"file"}
        accept={"image/png, image/gif, image/jpeg"}
      />

      <HoverImage src={src} alt={alt ?? "Image editor"} size={size}>
        {/*<EditIcon className={styles.icon} />*/}

        <Button
          variant="text"
          color={"secondary"}
          onClick={() => callHandler(handler1)}
        >
          {button1Text}
        </Button>
        {handler2 && (
          <Button
            variant="text"
            color={"secondary"}
            // @ts-ignore
            onClick={() => callHandler(handler2)}
          >
            {button2Text}
          </Button>
        )}
        {handler3 && (
          <Button
            variant="text"
            color={"secondary"}
            // @ts-ignore
            onClick={() => callHandler(handler3)}
          >
            {button3Text}
          </Button>
        )}
      </HoverImage>
    </>
  );
}
