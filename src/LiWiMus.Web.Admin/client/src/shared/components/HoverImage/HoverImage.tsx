import React, { PropsWithChildren } from "react";
import { Box } from "@mui/material";
import styles from "./HoverImage.module.sass";

type Props = {
  size: any;
  src: string | undefined;
  alt: string;
};

export default function HoverImage({
  children,
  size,
  src,
  alt,
}: PropsWithChildren<Props>) {
  return (
    <Box className={styles.container} width={size} height={size}>
      <img src={src} alt={alt} />
      <div className={styles.controls}>{children}</div>
    </Box>
  );
}
