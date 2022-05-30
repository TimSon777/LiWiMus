import React, { useEffect, useState } from "react";
import { Artist } from "../../types/Artist";
import { User } from "../../../users/types/User";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Loading from "../../../shared/components/Loading/Loading";
import AlertDialog from "../../../shared/components/AlertDialog/AlertDialog";
import UsersList from "../../../users/components/UsersList/UsersList";
import { useArtistService } from "../../ArtistService.hook";

type Props = {
  artist: Artist;
  owners: User[];
  setOwners: (owners: User[]) => void;
};

export default function ArtistOwners({ setOwners, owners, artist }: Props) {
  const artistService = useArtistService();

  const [isLoading, setIsLoading] = useState<boolean>(true);
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newArtists = await artistService.getOwners(artist);
      setOwners(newArtists);
    } catch (e) {
      showError(e);
    }
  };

  useEffect(() => {
    setIsLoading(true);
    fetchMore()
      .then(() => {
        setIsLoading(false);
      })
      .catch((e) => {
        showError(e);
        setIsLoading(false);
      });
  }, []);

  const removeOwner = async (user: User) => {
    try {
      await artistService.removeOwner(artist, user);
      setOwners(owners.filter((t) => t.id !== user.id));
      showSuccess("User removed from album");
    } catch (e) {
      showError(e);
    }
  };

  if (isLoading) {
    return <Loading />;
  }

  if (owners.length === 0) {
    return <></>;
  }

  return (
    <UsersList
      users={owners}
      avatar
      userName
      action={(user) => (
        <AlertDialog
          onAgree={() => removeOwner(user)}
          title={"Remove user from artist?"}
          text={"You can add it back later"}
          agreeText={"Remove"}
          disagreeText={"Cancel"}
          buttonText={"Remove"}
        />
      )}
    />
  );
}
