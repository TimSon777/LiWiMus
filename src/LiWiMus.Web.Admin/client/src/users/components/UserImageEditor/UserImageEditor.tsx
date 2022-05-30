import React, { useEffect, useState } from "react";
import { User } from "../../types/User";
import AvatarPlaceholder from "../../../shared/images/avatar-placeholder.jpg";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ImageEditor from "../../../shared/components/ImageEditor/ImageEditor";
import { useUserService } from "../../UserService.hook";
import { useFileService } from "../../../shared/hooks/FileService.hook";

type Props = {
  user: User;
  setUser: (user: User) => void;
};

export default function UserImageEditor({ user, setUser }: Props) {
  const userService = useUserService();
  const fileService = useFileService();

  const [avatarSrc, setAvatarSrc] = useState(AvatarPlaceholder);

  const setAvatar = (avatarLocation: string | undefined) => {
    if (avatarLocation) {
      setAvatarSrc(fileService.getLocation(avatarLocation));
    } else {
      setAvatarSrc(AvatarPlaceholder);
    }
  };

  const setUserWithAvatar = (newUser: User) => {
    setUser({ ...user, ...newUser });
    setAvatar(newUser.avatarLocation);
  };

  useEffect(() => {
    setAvatar(user.avatarLocation);
  }, [user.avatarLocation]);

  const { showSuccess, showError } = useNotifier();

  const updatePhotoHandler = (input: HTMLInputElement) => {
    input.click();
  };

  const removePhotoHandler = async () => {
    if (!user.avatarLocation) {
      return;
    }
    try {
      const response = await userService.removeAvatar(user);
      setUserWithAvatar(response);
      showSuccess("Avatar removed");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  const changePhotoHandler = async (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const input = event.target;
    if (!input.files || !input.files[0]) {
      return;
    }
    try {
      const photo = input.files[0];

      const response = await userService.changeAvatar(user, photo);
      setUserWithAvatar(response);
      showSuccess("Avatar updated");
    } catch (error) {
      // @ts-ignore
      showError(error);
    }
  };

  const setRandomAvatar = async () => {
    try {
      const response = await userService.setRandomAvatar(user!);
      setUser(response);
      showSuccess("Random avatar set");
    } catch (e) {
      showError(e);
    }
  };

  return (
    <ImageEditor
      width={250}
      src={avatarSrc}
      onChange={changePhotoHandler}
      handler1={updatePhotoHandler}
      handler2={removePhotoHandler}
      handler3={setRandomAvatar}
    />
  );
}
