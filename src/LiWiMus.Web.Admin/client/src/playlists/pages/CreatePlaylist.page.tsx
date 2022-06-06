import React, {useEffect, useState} from "react";
import {
    Autocomplete,
    Button,
    Checkbox,
    FormControlLabel,
    Grid,
    Paper,
    Stack,
    Typography,
} from "@mui/material";
import ContrastTextField from "../../shared/components/ContrastTextField/ContrastTextField";
import ImageEditor from "../../shared/components/ImageEditor/ImageEditor";
import {useNotifier} from "../../shared/hooks/Notifier.hook";
import artistPlaceholder from "../../shared/images/image-placeholder.png";
import {SubmitHandler, useForm} from "react-hook-form";
import {CreatePlaylistDto} from "../types/CreatePlaylistDto";
import {usePlaylistService} from "../PlaylistService.hook";
import {useFileService} from "../../shared/hooks/FileService.hook";
import {useUserService} from "../../users/UserService.hook"
import {User} from "../../users/types/User"
import {
    DefaultPaginatedData,
    PaginatedData,
} from "../../shared/types/PaginatedData";
import {useNavigate} from "react-router-dom";

type Inputs = {
    ownerName: string;
    name: string;
    isPublic: boolean;
    photoFlag: string;
};

export default function CreatePlaylistPage() {
    const [ownerValue, setOwnerValue] = useState('');
    const playlistService = usePlaylistService();
    const fileService = useFileService();
    const userService = useUserService();
    const [value, setInputValue] = useState('')
    const [searchLoading, setSearchLoading] = useState(false);
    const [users, setUsers] = useState<{ label: string | undefined; id: number; }[]>([])
    const [searchUsers, setSearchUsers] = useState<{ label: string | undefined; id: number; }[]>([])

    const {
        register,
        handleSubmit,
        formState: {errors},
        setValue,
        clearErrors,
    } = useForm<Inputs>();
    const [checked, setChecked] = React.useState(false);
    const [photo, setPhoto] = useState<File>();
    const [photoBase64, setPhotoBase64] = useState<string>(artistPlaceholder);
    const {showError, showSuccess} = useNotifier();
    const navigate = useNavigate();


    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setChecked(event.target.checked);
    };

    const onSubmit: SubmitHandler<Inputs> = async (data) => {
        try {
            console.log(users)
            // @ts-ignore
            const photoLocation = await fileService.save(photo);
            const dto: CreatePlaylistDto = {
                owner: users.filter(a => a.label === data.ownerName)[0].id,
                name: data.name,
                isPublic: checked,
                photoLocation,
            };

            const playlist = await playlistService.save(dto);
            showSuccess("Playlist created");
            navigate(`/admin/playlists/${playlist.id}`);

        } catch (e) {
            showError(e);
        }
    };

    const changePhotoHandler = async (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        const input = event.target;
        if (!input.files || !input.files[0]) {
            setValue("photoFlag", "");
            return;
        }
        const cover = input.files[0];

        const reader = new FileReader();
        reader.readAsDataURL(cover);
        reader.onload = () => {
            setPhotoBase64(reader.result as string);
        };

        reader.onerror = (error) => {
            showError(error);
        };

        setPhoto(cover);
        setValue("photoFlag", ".");
        clearErrors("photoFlag");
    };

    useEffect(() => {
        (async () => {
            setSearchLoading(true);
            try {
                const items = await userService.getUsers({
                });
                const users = await userService.getUsers({
                    page: {pageNumber: 1, numberOfElementsOnPage: items.totalItems}
                });
                const res = users.data.map(a => ({
                    label: a.email, id: a.id
                }))
                const searchUsersAwait = await userService.getUsers({
                    filters: [{columnName: "email", operator: "cnt", value: value}],
                   /* page: {pageNumber: 1, numberOfElementsOnPage: 5}*/
                });
                const searchUserRes = searchUsersAwait.data.map(a => ({
                    label: a.email, id: a.id
                }))
                setUsers(res);
                setSearchUsers(searchUserRes)
            } catch (e) {
                showError(e);
            }
            setSearchLoading(false);
        })();
    }, [value]);

    return (
        <Grid container spacing={2} justifyContent={"center"}>
            <Grid item xs={12} md={10} lg={8}>
                <Paper sx={{p: 4}} elevation={10}>
                    <Typography variant={"h3"} component={"div"} sx={{mb: 4}}>
                        New playlist
                    </Typography>

                    <Grid container spacing={2}>
                        <Grid
                            item
                            xs={12}
                            md={6}
                            sx={{
                                display: "flex",
                                flexDirection: "column",
                                alignItems: "center",
                            }}
                        >
                            <ImageEditor
                                src={photoBase64}
                                width={250}
                                handler1={(input) => input.click()}
                                onChange={changePhotoHandler}
                            />
                            {errors.photoFlag && errors.photoFlag.message && (
                                <Typography color={"error"} sx={{mt: 2}}>
                                    {errors.photoFlag.message}
                                </Typography>
                            )}
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <form onSubmit={handleSubmit(onSubmit)}>
                                <input
                                    type="text"
                                    hidden
                                    {...register("photoFlag", {
                                        required: {value: true, message: "Photo required"},
                                    })}
                                />

                                <Stack spacing={2} alignItems={"center"}>
                                    <Autocomplete
                                        autoComplete
                                        onInputChange={(event) => setInputValue((event.target as HTMLInputElement).value)}
                                        fullWidth
                                        isOptionEqualToValue={(option, value) => option.id === value.id}
                                        options={searchUsers}
                                        renderInput={(params) =>
                                            (<ContrastTextField
                                                error={!!errors.ownerName && !!errors.ownerName.message}
                                                helperText={errors.ownerName?.message}
                                                label={"Owner"}
                                                {...params}
                                                {...register("ownerName", {
                                                    required: {value: true, message: "Owner required"},
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
                                            minLength: {value: 5, message: "Min length - 5"},
                                            maxLength: {value: 50, message: "Max length - 50"},
                                        })}
                                    />

                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={checked}
                                                onChange={handleChange}
                                                inputProps={{"aria-label": "controlled"}}
                                            />
                                        }
                                        label="Public"
                                        labelPlacement="start"
                                    />
                                    <Button
                                        type={"submit"}
                                        variant="contained"
                                        sx={{borderRadius: "20px", px: 4, width: "200px"}}
                                    >
                                        Create
                                    </Button>
                                </Stack>
                            </form>
                        </Grid>
                    </Grid>
                </Paper>
            </Grid>
        </Grid>
    );
}
