import React from "react";
import { Artist } from "../../types/Artist";
import { PaginatedData } from "../../../shared/types/PaginatedData";
import { User } from "../../../users/types/User";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import ArtistService from "../../Artist.service";
import Loading from "../../../shared/components/Loading/Loading";
import InfiniteScroll from "react-infinite-scroll-component";
import { Button } from "@mui/material";
import UserService from "../../../users/User.service";
import UsersList from "../../../users/components/UsersList/UsersList";

type Props = {
  artist: Artist;
  users: PaginatedData<User>;
  setUsers: (users: PaginatedData<User>) => void;
  filter: string;
  loading: boolean;
  owners: User[];
  setOwners: (owners: User[]) => void;
};

export default function ArtistUsersSearch({
  setUsers,
  users,
  artist,
  owners,
  setOwners,
  loading,
  filter,
}: Props) {
  const { showError, showSuccess } = useNotifier();

  const fetchMore = async () => {
    try {
      const newUsers = await UserService.getUsers({
        filters: [
          { columnName: "userName", operator: "cnt", value: filter },
          {
            columnName: "id",
            operator: "-in",
            value: [0, ...owners.map((user) => user.id)],
          },
        ],
        page: {
          pageNumber: users.actualPage + 1,
          numberOfElementsOnPage: 10,
        },
      });
      setUsers({
        ...newUsers,
        data: [...users.data, ...newUsers.data],
      });
    } catch (e) {
      showError(e);
    }
  };

  const addOwner = async (user: User) => {
    try {
      await ArtistService.addOwner(artist, user);
      setOwners([...owners, user]);
      setUsers({
        ...users,
        totalItems: users.totalItems - 1,
        data: users.data.filter((t) => t.id !== user.id),
      });
      showSuccess("User added to album");
    } catch (e) {
      showError(e);
    }
  };

  if (loading) {
    return <Loading />;
  }

  if (users.data.length === 0) {
    return <></>;
  }

  return (
    <InfiniteScroll
      dataLength={users.data.length}
      hasMore={users.hasMore}
      loader={<Loading />}
      next={fetchMore}
    >
      <UsersList
        users={users.data}
        avatar
        userName
        action={(user) => (
          <Button
            variant="outlined"
            color={"secondary"}
            sx={{ borderRadius: "20px", px: 4 }}
            onClick={() => addOwner(user)}
          >
            Add
          </Button>
        )}
      />
    </InfiniteScroll>
  );
}
