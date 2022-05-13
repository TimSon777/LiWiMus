import React from "react";
import AlertDialog from "../AlertDialog/AlertDialog";

type Props = {
  itemName?: string;
  deleteHandler: () => void;
};

export default function Deleter({ deleteHandler, itemName = "item" }: Props) {
  return (
    <AlertDialog
      onAgree={deleteHandler}
      buttonText={"Delete"}
      disagreeText={"Cancel"}
      agreeText={"Delete"}
      title={`Delete ${itemName}?`}
    />
  );
}
