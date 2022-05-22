import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

export default function UserProfilePage() {
  const { id } = useParams();

  useEffect(() => {
    (async () => {
     
    })();
  }, []);
  return (
    <div>
      <div>
        <h1>User Profile {id}</h1>
      </div>
    </div>
  );
}
