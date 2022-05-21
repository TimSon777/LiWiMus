import React, { useState } from "react";
import { Track } from "../../types/Track";
import {
  Box,
  IconButton,
  List,
  ListItem,
  ListItemText,
  Modal,
  Paper,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import { Genre } from "../../../genres/types/Genre";
import {
  DefaultPaginatedData,
  PaginatedData,
} from "../../../shared/types/PaginatedData";
import GenreService from "../../../genres/Genre.service";
import { useNotifier } from "../../../shared/hooks/Notifier.hook";
import Loading from "../../../shared/components/Loading/Loading";
import InfiniteScroll from "react-infinite-scroll-component";
import TrackService from "../../Track.service";

type Props = {
  track: Track;
  setTrack: (track: Track) => void;
};

const modalStyle = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  overflow: "hidden",
  width: { xs: "90%", sm: "75%", md: "60%", lg: "45%", xl: "30%" },
  bgcolor: "background.paper",
  boxShadow: 24,
  p: 4,
  display: "block",
  border: 1,
  borderColor: "secondary",
};

const headerStyle = {};

const contentStyle = {
  overflow: "auto",
  height: "300px",
};

export default function AddGenreCard({ track, setTrack }: Props) {
  const [genres, setGenres] =
    useState<PaginatedData<Genre>>(DefaultPaginatedData);
  const [open, setOpen] = React.useState(false);
  const [filter, setFilter] = useState("");
  const { showError } = useNotifier();

  const handleOpen = async () => {
    setOpen(true);
    const more = await getGenres("", 1);
    setGenres(more);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const getGenres = async (filter: string, page: number) => {
    return await GenreService.getGenres({
      filters: [
        { columnName: "name", operator: "cnt", value: filter },
        {
          columnName: "id",
          operator: "-in",
          value: [0, ...track.genres.map((genre) => genre.id)],
        },
      ],
      page: {
        pageNumber: page,
        numberOfElementsOnPage: 15,
      },
    });
  };

  const handleInput = async (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
    setFilter(e.target.value);
    const more = await getGenres(e.target.value, 1);
    setGenres(more);
  };

  const fetchMore = async () => {
    const more = await getGenres(filter, genres.actualPage + 1);
    setGenres({ ...more, data: [...genres.data, ...more.data] });
  };

  const addGenre = async (genre: Genre) => {
    try {
      await TrackService.addGenre(track, genre);
      setTrack({ ...track, genres: [...track.genres, genre] });
      setGenres({
        ...genres,
        data: genres.data.filter((t) => t.id !== genre.id),
      });
    } catch (e) {
      showError(e);
    }
  };

  return (
    <>
      <Paper
        sx={{ width: "100%", pb: "100%", position: "relative" }}
        elevation={10}
      >
        <IconButton
          sx={{
            position: "absolute",
            top: "50%",
            left: "50%",
            transform: "translate(-50%, -50%)",
          }}
          onClick={handleOpen}
        >
          <AddIcon sx={{ fontSize: "2rem" }} />
        </IconButton>
      </Paper>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalStyle}>
          <ContrastTextField
            sx={headerStyle}
            fullWidth
            label={"Genre name"}
            value={filter}
            onChange={handleInput}
          />
          <List sx={contentStyle} id={"scrollableDiv"} dense>
            <InfiniteScroll
              dataLength={genres.data.length}
              next={fetchMore}
              hasMore={genres.hasMore}
              loader={<Loading />}
              style={{ overflow: "scroll" }}
              scrollableTarget={"scrollableDiv"}
            >
              {genres.data.map((genre, index) => (
                <ListItem
                  key={index}
                  secondaryAction={
                    <IconButton edge="end" onClick={() => addGenre(genre)}>
                      <AddIcon />
                    </IconButton>
                  }
                >
                  <ListItemText
                    primary={genre.name}
                    primaryTypographyProps={{ variant: "h6" }}
                  />
                </ListItem>
              ))}
            </InfiniteScroll>
          </List>
        </Box>
      </Modal>
    </>
  );
}
