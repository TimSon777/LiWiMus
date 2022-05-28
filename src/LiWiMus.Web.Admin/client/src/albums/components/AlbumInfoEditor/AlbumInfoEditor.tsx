import React from "react";
import {useNotifier} from "../../../shared/hooks/Notifier.hook";
import {SubmitHandler, useForm} from "react-hook-form";
import AlbumService from "../../Album.service";
import {Box, Button, Stack} from "@mui/material";
import ContrastTextField from "../../../shared/components/ContrastTextField/ContrastTextField";
import {DatePicker} from "@mui/x-date-pickers";
import {UpdateAlbumDto} from "../../types/UpdateAlbumDto";
import {format, parse} from "date-fns";

type Inputs = {
    title: string;
    publishedAt: Date;
};

type Props = {
    id: string;
    dto: Inputs;
    setDto: (dto: Inputs) => void;
};

export default function AlbumInfoEditor({id, dto, setDto}: Props) {
    const {showSuccess, showError} = useNotifier();
    const {
        register,
        handleSubmit,
        watch,
        formState: {errors},
        reset,
        getValues,
        setValue,
    } = useForm<Inputs>({defaultValues: dto});

    const actual = watch();
    const isChanged = JSON.stringify(actual) !== JSON.stringify(dto);

    const rollbackHandler = () => {
        reset(dto);
    };

    const saveHandler: SubmitHandler<Inputs> = async (data) => {
        if (JSON.stringify(data) === JSON.stringify(dto)) {
            return;
        }
        try {
            const req: UpdateAlbumDto = {
                id,
                title: data.title,
                publishedAt: format(new Date(data.publishedAt), "dd.MM.yyyy"),
            };
            const response = await AlbumService.update(req);
            showSuccess("Info updated");
            setDto({
                publishedAt: parse(response.publishedAt, "yyyy-MM-dd", new Date()),
                title: response.title,
            });
        } catch (error) {
            // @ts-ignore
            showError(error);
        }
    };

    return (
        <Box sx={{display: "flex", flexDirection: "column", width: "100%"}}>
            <Stack direction={"column"} spacing={2}>
                <ContrastTextField
                    error={!!errors.title && !!errors.title.message}
                    helperText={errors.title?.message}
                    label="Title"
                    InputLabelProps={{
                        shrink: true,
                    }}
                    variant="outlined"
                    fullWidth
                    {...register("title", {
                        required: true,
                        maxLength: 50,
                        minLength: 5,
                    })}
                />
                <DatePicker
                    label={"Published at"}
                    mask={"__.__.____"}
                    value={getValues("publishedAt")}
                    onChange={(newValue) => {
                        newValue && setValue("publishedAt", newValue);
                    }}
                    renderInput={(params) => (
                        <ContrastTextField
                            InputLabelProps={{
                                shrink: true,
                            }}
                            error={!!errors.publishedAt && !!errors.publishedAt.message}
                            helperText={errors.publishedAt?.message}
                            {...params}
                            {...register("publishedAt", {required: true})}
                        />
                    )}
                />
                {isChanged && (
                    <Stack direction={"row"} sx={{alignSelf: "flex-end"}} spacing={2}>
                        <Button
                            onClick={rollbackHandler}
                            variant="text"
                            color={"secondary"}
                            sx={{borderRadius: "20px", px: 4}}
                        >
                            Rollback
                        </Button>
                        <Button
                            onClick={handleSubmit(saveHandler)}
                            variant="contained"
                            sx={{borderRadius: "20px", px: 4}}
                        >
                            Save
                        </Button>
                    </Stack>
                )}
            </Stack>
        </Box>
    );
}
