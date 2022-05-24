import React, {useEffect, useRef, useState} from "react";
import {useNavigate} from "react-router-dom";
// @ts-ignore
import dateFormat from "dateformat";
import {
    DataGrid,
    GridColDef,
    GridRowsProp,
    GridSelectionModel,
    GridSortModel,
} from "@mui/x-data-grid";
import {
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    TextField,
    Popover, Fab
} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import AddIcon from "@mui/icons-material/Add";
import FiltersPopover from "../../shared/components/FiltersPopover/FiltersPopover";
import FilterListIcon from '@mui/icons-material/FilterList';
import PlanService from "../Plan.service";
import {FilterOptions} from "../../shared/types/FilterOptions";
import {Plan} from "../types/Plan";


type FilterItem = {
    id: number;
    filterColumn: string;
    filterOperator: string;
    filterValue: string | number;
};

interface FilterModel extends Array<FilterItem> {
}

export default function PlansPage() {
    const [rows, setRows] = useState/*<GridRowsProp>*/([]);
    const navigate = useNavigate();
    const columns: GridColDef[] = [
        {field: "id", hide: true,},
        {
            field: "name",
            headerName: "Name",
            flex: 0.5,
            filterable: false,
            sortable: false,
        },
        {
            field: "pricePerMonth",
            headerName: "PricePerMonth",
            flex: 0.5,
            filterable: false,
            sortable: false,
        },
        {
            field: "description",
            headerName: "Description",
            flex: 0.5,
            filterable: false,
            sortable: false,
        },
        {
            field: "edit",
            headerName: "Edit",
            sortable: false,
            flex: 0.5,
            filterable: false,
            renderCell: (params) => {
                const onClick = (e: any) => {
                    e.stopPropagation();
                    const path = "/admin/plans/" + params.getValue(params.id, "id");
                    navigate(path);
                };

                return (
                    <Button
                        variant="contained"
                        endIcon={<ArrowForwardIcon/>}
                        sx={{borderRadius: "20px", px: 4}}
                        onClick={onClick}
                    >
                        Edit
                    </Button>
                );
            },
        },
    ];



    useEffect(() => {
        let active = true;

        (async () => {
            const response = await PlanService.getPlans()
            
            if (!active) {
                return;
            }
            setRows(response);
        })();
        // @ts-ignore

        return () => {
            active = false;
        };
    },[] );

    return (
        <div>
            <div>
                <h1>Plans page</h1>
            </div>
            <div>
               <DataGrid
                    columns={columns}
                    rows={rows}
                    pagination={true}
                    disableColumnMenu={true}
                    rowsPerPageOptions={[2, 5, 10, 20]}
                    disableSelectionOnClick={true}
                    autoHeight
                ></DataGrid>
            </div>
            <div>
                <Fab color="primary" sx={{ position: 'fixed', bottom: 50, right: 50}} aria-label="add" href="/admin/plans/create">
                    <AddIcon/>
                </Fab>
            </div>
        </div>
    );
}
