import React, {useState, useEffect, useRef, useCallback} from "react";
import {
    DataGrid,
    GridRowsProp,
    GridColDef,
    GridApi,
    GridCellValue,
    GridToolbar,
    GridFilterModel,
    GridToolbarContainer,
    GridSelectionModel,
    GridSortModel
} from '@mui/x-data-grid';
import {Button} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";

export default function UsersPage() {
    const [users, setUsers] = useState([])
    const [rows, setRows] = useState<GridRowsProp>([]);
    
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState<boolean>(false);
    const [selectionModel, setSelectionModel] = useState<GridSelectionModel>([]);
    const prevSelectionModel = useRef<GridSelectionModel>(selectionModel);
    const [sortModel, setSortModel] = useState<GridSortModel>([{field: 'id', sort:'asc' }])
    const [filterValue, setFilterValue] = useState<string | undefined>();
    
    const onFilterChange = useCallback((filterModel: GridFilterModel) => {
        setFilterValue(filterModel.items[0].value);
    },[]);
    const handleSortModelChange = (newModel: GridSortModel) => {
        setSortModel(newModel);
    }
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
    
    useEffect(() => {
        const getUsers = async () => {
            const res = await fetch('https://liwimus.aslipatov.site/admin/api/getusers');
            const data = await res.json();
            setUsers(data)
        };
        getUsers();
    }, []);
    
    useEffect(() => {
        let active = true;

        (async () => {
            setLoading(true);
            const res = await fetch('');
            const newRows = await res.json();
            
            if(!active){
                return;
            }
            
            setRows(newRows);
            setLoading(false);
            setTimeout(() => {
                setSelectionModel(prevSelectionModel.current)
            });
        })();
        
        return () => {
            active = false;
        };
    }, [page]);
    
    useEffect(() => {
        let active = true;

        (async () => {
            setLoading(true);
            const res = await fetch('');
            const newRows = await res.json();
            
            if(!active){
                return;
            }
            
            setRows(newRows);
            setLoading(false);
        })();
        
        return () => {
            active = false;
        };
    },[sortModel,page]);

    useEffect(() => {
        let active = true;

        (async () => {
            setLoading(true);
            const res = await fetch('');
            const newRows = await res.json();
            
            if (!active) {
                return;
            }

            setRows(newRows);
            setLoading(false);
        })();

        return () => {
            active = false;
        };
    }, [filterValue, page]);
    
    return (
        <div>
            <div>
                <h1>Users page</h1>
            </div>
            <div>
                <DataGrid
                    columns={columns}
                    rows={users}
                    components={{Toolbar: GridToolbar}}
                    checkboxSelection
                    pagination
                    pageSize={5}
                    rowsPerPageOptions={[5]}
                    rowCount={40}
                    sortingMode="server"
                    sortModel={sortModel}
                    onSortModelChange={handleSortModelChange}
                    paginationMode="server"
                    onPageChange={(newPage) => {
                        prevSelectionModel.current = selectionModel;
                        setPage(newPage);
                    }}
                    onSelectionModelChange={(newSelectionModel) => {
                        setSelectionModel(newSelectionModel);
                    }}
                    selectionModel={selectionModel}
                    filterMode="server"
                    onFilterModelChange={onFilterChange}
                    loading = {loading}
                    autoHeight></DataGrid>
            </div>
        </div>
    );
}
