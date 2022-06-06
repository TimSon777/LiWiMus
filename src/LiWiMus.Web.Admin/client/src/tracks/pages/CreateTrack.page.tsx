import React, {useEffect, useState} from "react";
import {Autocomplete, Button, Grid, Paper, Stack, Typography} from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import ImageEditor from "../../shared/components/ImageEditor/ImageEditor";
import {useNotifier} from "../../shared/hooks/Notifier.hook";
import albumPlaceholder from "../../shared/images/image-placeholder.png";
import {Controller, SubmitHandler, useForm} from "react-hook-form";
import {CreateTrackDto} from "../types/CreateTrackDto";
import {useNavigate} from "react-router-dom";
import {DatePicker, DateTimePicker} from "@mui/x-date-pickers";
import {useTrackService} from "../TrackService.hook";
import {useAlbumService} from "../../albums/AlbumService.hook"
import {useFileService} from "../../shared/hooks/FileService.hook";
import AudiotrackIcon from '@mui/icons-material/Audiotrack';
import FileUploadIcon from '@mui/icons-material/FileUpload';
import {format, parse} from "date-fns";
import {useArtistService} from "../../artists/ArtistService.hook"
import {useGenreService} from "../../genres/GenreService.hook"

import {Artist} from "../../artists/types/Artist"
import {
    DefaultPaginatedData,
    PaginatedData,
} from "../../shared/types/PaginatedData";

type Inputs = {
    albumId: string,
    name: string,
    publishedAt: Date | null,
    fileLocation: string,
    genreIds: string,
    ownerIds: string,
    duration: number
};

export default function CreateTrackPage() {
    const genreService = useGenreService();
    const artistService = useArtistService();
    const albumService = useAlbumService();
    const trackService = useTrackService();
    const [albumValue, setAlbumValue] = useState('')
    const [genreValue, setGenreValue] = useState('')
    const [artistValue, setArtistValue] = useState('')
    const [searchLoading, setSearchLoading] = useState(false);
    const fileService = useFileService();
    const [albums, setAlbums] = useState<{ label: string | undefined; id: number; }[]>([])
    const [artists, setArtists] = useState<{ label: string | undefined; id: number; }[]>([])
    const [genres, setGenres] = useState<{ label: string | undefined; id: number; }[]>([])

    const [albumOptions, setAlbumOptions] = useState<{ label: string; id: number; }[]>([])
    const [artistOptions, setArtistOptions] = useState<{ label: string; id: number; }[]>([])
    const [genreOptions, setGenreOptions] = useState<{ label: string | undefined; id: number; }[]>([])


    const {
        register,
        handleSubmit,
        formState: {errors},
        clearErrors,
        setValue,
        control,
    } = useForm<Inputs>({
        defaultValues: {name: "", publishedAt: null}
    });

    const [file, setFile] = useState<File>();
    const [fileName, setFileName] = useState('')
    const {showError, showSuccess} = useNotifier();
    const navigate = useNavigate();

    const getAudio = (file: File): Promise<HTMLAudioElement> => {
        return new Promise((resolve, reject) => {
            const url = URL.createObjectURL(file);
            const sound = new Audio(url);
            sound.addEventListener("error", () => {
                reject("Bad audio file");
            });
            sound.addEventListener("canplaythrough", () => {
                URL.revokeObjectURL(url);
                resolve(sound);
            });
        });
    };

    const onSubmit: SubmitHandler<Inputs> = async (data) => {
        try {
            console.log(artists, albums, genres)
            // @ts-ignore
            const fileLocation = await fileService.save(file);
            // @ts-ignore
            const trackDuration = await getAudio(file)
            const dto: CreateTrackDto = {
                albumId: albums.filter(a => a.label === data.albumId.split(" ")[0])[0].id,
                name: data.name,
                publishedAt: format(data.publishedAt as Date, "yyyy-MM-dd"),
                fileLocation,
                genreIds: [genres.filter(a => a.label === data.genreIds)[0].id],
                ownerIds: [artists.filter(a => a.label === data.ownerIds.split(" ")[0])[0].id],
                duration: trackDuration.duration
            };
            const track = await trackService.save(dto);
            showSuccess("Track created");
            navigate(`/admin/tracks/${track}`);
        } catch (e) {
            showError(e);
        }
    };
    const changeFileHandler = async (event: React.ChangeEvent<HTMLInputElement>) => {
        try {
            if (!event.target.files || !event.target.files[0]) {
                return;
            }
            setFile(event.target.files[0])
            setFileName(event.target.files[0].name);
        } catch
            (e) {
            showError(e);
        }
    }
    useEffect(() => {
        (async () => {
            setSearchLoading(true);
            try {

                const albumsItems = await albumService.getAlbums({});
                const genresItems = await genreService.getGenres({})
                const artistsItems = await artistService.getArtists({});

                const albums = await albumService.getAlbums({
                    page: {pageNumber: 1, numberOfElementsOnPage: albumsItems.totalItems}
                })
                const genres = await genreService.getGenres({
                    page: {pageNumber: 1, numberOfElementsOnPage: genresItems.totalItems}
                })
                const artists = await artistService.getArtists({
                    page: {pageNumber: 1, numberOfElementsOnPage: artistsItems.totalItems}
                })

                const albumsRes = albums.data.map(a => ({
                    label: a.title, id: Number(a.id)
                }))
                const genreRes = genres.data.map(a => ({
                    label: a.name, id: Number(a.id)
                }))
                const artistRes = artists.data.map(a => ({
                    label: a.name, id: Number(a.id)
                }))

                const searchAlbumsAwait = await albumService.getAlbums({
                    filters: [{columnName: "title", operator: "cnt", value: albumValue}],
                   /* page: {pageNumber: 1, numberOfElementsOnPage: 5}*/
                });
                const searchArtistsAwait = await artistService.getArtists({
                    filters: [{columnName: "name", operator: "cnt", value: artistValue}],
                   /* page: {pageNumber: 1, numberOfElementsOnPage: 5}*/
                });
                const searchGenresAwait = await genreService.getGenres({
                    filters: [{columnName: "name", operator: "cnt", value: genreValue}],
                    /*page: {pageNumber: 1, numberOfElementsOnPage: 5}*/
                });

                const albumsSearchRes = searchAlbumsAwait.data.map(a => ({
                    label: a.title, id: Number(a.id)
                }))
                const genreSearchRes = searchGenresAwait.data.map(a => ({
                    label: a.name, id: Number(a.id)
                }))
                const artistSearchRes = searchArtistsAwait.data.map(a => ({
                    label: a.name, id: Number(a.id)
                }))
                setAlbums(albumsRes)
                setArtists(artistRes)
                setGenres(genreRes)
                setArtistOptions(artistSearchRes);
                setAlbumOptions(albumsSearchRes);
                setGenreOptions(genreSearchRes)
            } catch (e) {
                showError(e);
            }
            setSearchLoading(false);
        })();
    }, [genreValue, albumValue, artistValue]);


    return (
        <Grid container spacing={2} justifyContent={"center"}>
            <Grid item xs={12} md={10} lg={8}>
                <Paper sx={{p: 4}} elevation={10}>
                    <Typography variant={"h3"} component={"div"} sx={{mb: 4}}>
                        New track
                    </Typography>
                    <form onSubmit={handleSubmit(onSubmit)}>
                        <Stack spacing={2} alignItems={"center"}>
                            <Autocomplete
                                autoComplete
                                onInputChange={event => setAlbumValue((event.target as HTMLInputElement).value)}
                                fullWidth
                                getOptionLabel={(option) => option.label + " (" +option.id.toString()+")" }
                                isOptionEqualToValue={(option, value) => option.id === value.id}
                                options={albumOptions}
                                renderInput={(params) =>
                                    (<ContrastTextField
                                        error={!!errors.albumId && !!errors.albumId.message}
                                        helperText={errors.albumId?.message}
                                        label={"Album"}
                                        {...params}
                                        {...register("albumId", {
                                            required: {value: true, message: "Album required"},
                                        })}
                                    />)
                                }
                            />
                            <ContrastTextField
                                error={!!errors.name && !!errors.name.message}
                                helperText={errors.name?.message}
                                label={"Name"}
                                fullWidth
                                {...register("name", {
                                    required: {value: true, message: "Name required"},
                                    minLength: {value: 1, message: "Min length - 1"},
                                    maxLength: {value: 50, message: "Max length - 50"},
                                })}
                            />

                            <Autocomplete
                                autoComplete
                                onInputChange={event => setArtistValue((event.target as HTMLInputElement).value)}
                                fullWidth
                                getOptionLabel={(option) => option.label + " (" +option.id.toString()+")" }                                isOptionEqualToValue={(option, value) => option.id === value.id}
                                options={artistOptions}
                                renderInput={(params) =>
                                    (<ContrastTextField
                                        error={!!errors.ownerIds && !!errors.ownerIds.message}
                                        helperText={errors.ownerIds?.message}
                                        label={"Owners"}
                                        {...params}
                                        {...register("ownerIds", {
                                            required: {value: true, message: "Owners required"},
                                        })}
                                    />)
                                }
                            />
                            <Autocomplete
                                autoComplete
                                onInputChange={event => setGenreValue((event.target as HTMLInputElement).value)}
                                fullWidth
                                isOptionEqualToValue={(option, value) => option.id === value.id}
                                options={genreOptions}
                                renderInput={(params) =>
                                    (<ContrastTextField
                                        error={!!errors.genreIds && !!errors.genreIds.message}

                                        helperText={errors.genreIds?.message}

                                        label={"Genres"}
                                        {...params}
                                        {...register("genreIds", {
                                            required: {value: true, message: "Genres required"},
                                        })}
                                    />)
                                }
                            />
                            <Controller
                                name={"publishedAt"}
                                control={control}
                                rules={{
                                    required: {value: true, message: "Required"},
                                }}

                                render={({field}) => (
                                    <DatePicker
                                        maxDate={new Date()}
                                        mask={"__.__.____"}
                                        label={"Published at"}
                                        InputProps={{
                                            error: !!errors.publishedAt,
                                        }}
                                        renderInput={(params) => (
                                            <ContrastTextField
                                                fullWidth
                                                helperText={errors.publishedAt?.message}
                                                {...params}
                                            />
                                        )}
                                        {...field}
                                    />
                                )}
                            />
                            <div >
                                <Button
                                variant="contained"
                                component="label"
                                sx={{borderRadius: "20px", px: 4, width: "150px", alignSelf: "start"}}
                                startIcon={<FileUploadIcon/>}>
                                    <input type={"file"} hidden onChange={changeFileHandler} accept={"audio/*"}/>Upload
                                </Button>
                                <span style={{marginLeft: "10px"}}>{fileName}</span>
                            </div>
                            <Button
                                type={"submit"}
                                variant="contained"
                                sx={{borderRadius: "20px", px: 4, width: "200px"}}
                            >
                                Create
                            </Button>
                        </Stack>
                    </form>
                </Paper>
            </Grid>
        </Grid>
    );
}
