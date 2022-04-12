import React, {useState, useEffect} from "react";
import {
    DataGrid,
    GridRowsProp,
    GridColDef,
    GridApi,
    GridCellValue,
    GridToolbar,
    GridToolbarContainer
} from '@mui/x-data-grid';
import {Button} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";

export default function UsersPage() {
    const [users, setUsers] = useState([])

    useEffect(() => {
        const getUsers = async () => {
            const res = await fetch('http://localhost:3500/users');
            const data = await res.json();
            setUsers(data)
        };
        getUsers();
    }, []);

    const rows: GridRowsProp = users;
    const columns: GridColDef[] = [{field: 'id', hide: true}, {
        field: 'username', headerName: 'Username', flex: 0.6
    }, {
        field: 'email', headerName: 'Email', flex: 0.7
    }, {
        field: 'phoneNumber', headerName: 'Phone Number', flex: 0.5
    }, {
        field: 'enum', headerName: 'Gender', flex: 0.3
    }, {
        field: 'emailConfirmed', headerName: 'Email Confirmed', flex: 0.4, type: "boolean"
    }, {
        field: 'phoneConfirmed', headerName: 'Phone Confirmed', flex: 0.4, type: "boolean"
    }, {
        field: 'balance', headerName: 'Balance', flex: 0.2
    }, {
        field: 'action',
        headerName: 'Edit',
        sortable: false,
        flex: 0.5,
        filterable: false,
        renderCell: (params) => {
            const onClick = (e: any) => {
                e.stopPropagation();
                const api: GridApi = params.api;
                const fields = api
                    .getAllColumns()
                    .map((c) => c.field)
                    .filter((c) => c !== "__check__" && !!c);
                const thisRow: any = {};

                fields.forEach((f) => {
                    thisRow[f] = params.getValue(params.id, f);
                });

                return alert(JSON.stringify(thisRow, null, 4));
            };

            return <Button variant="contained"
                           endIcon={<ArrowForwardIcon/>}
                           sx={{borderRadius: "20px", px: 4}} onClick={onClick}>Edit</Button>;
        },
    },];

    return (
        <div>
            <div>
                <h1>Users page</h1>
            </div>
            <div>
                <DataGrid
                    columns={columns}
                    rows={rows}
                    components={{Toolbar: GridToolbar}}
                    pageSize={5}
                    checkboxSelection={true}
                    autoHeight></DataGrid>
            </div>
        </div>
    );
}
