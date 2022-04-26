import React, {useState, useEffect, useRef, useCallback} from "react";
import {useNavigate} from 'react-router-dom'
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
import {Button, Select, MenuItem, TextField, InputLabel, FormControl} from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import AddIcon from '@mui/icons-material/Add';

type FilterItem = {
    [key: string]: any;
    id: number;
    filterColumn: string;
    filterOperator: string;
    filterValue: string | number;
}

interface FilterFormProps{
    id:number;
}
interface FilterModel extends Array<FilterItem>{}

export default function UsersPage() {
    const [rows, setRows] = useState<GridRowsProp>([]);
    const [page, setPage] = useState(1);
    const [limitItems, setLimitItems] = useState(5)
    const [loading, setLoading] = useState<boolean>(false);
    const [selectionModel, setSelectionModel] = useState<GridSelectionModel>([]);
    const prevSelectionModel = useRef<GridSelectionModel>(selectionModel);
    const [sortModel, setSortModel] = useState<GridSortModel>([{field: 'id', sort: 'asc'}])
    //const [filterItem, setFilterItem] = useState<FilterItem>(Object);
    const [filterModel, setFilterModel] = useState<FilterModel>([]);
    /*const [filterValue, setFilterValue] = useState<string>();
    const [filterColumn, setFilterColumn] = useState<string>()
    const [filterOperator, setFilterOperator] = useState<string>();
    const onFilterChange = useCallback((filterModel: GridFilterModel) => {
        setFilterValue(filterModel.items[0].value)
        setFilterColumn(filterModel.items[0].columnField);
    }, []);*/
    const handleSortModelChange = (newModel: GridSortModel) => {
        setSortModel(newModel);
    }
    const navigate = useNavigate();
    const columns: GridColDef[] = [{field: 'id', hide: true}, {
        field: 'userName', headerName: 'Username', flex: 0.6, filterable: false
    }, {
        field: 'firstName', headerName: 'First Name', flex: 0.6, filterable: false
    }, {
        field: 'secondName', headerName: 'Second Name', flex: 0.6, filterable: false
    }, {
        field: 'email', headerName: 'Email', flex: 0.7, filterable: false
    }, {
        field: 'birthDate', headerName: 'BirthDate', flex: 0.5, filterable: false
    }, {
        field: 'gender', headerName: 'Gender', flex: 0.3, filterable: false
    }, {
        field: 'emailConfirmed', headerName: 'Email Confirmed', flex: 0.4, type: "boolean", filterable: false
    }, {
        field: 'phoneConfirmed', headerName: 'Phone Confirmed', flex: 0.4, type: "boolean", filterable: false
    }, {
        field: 'balance', headerName: 'Balance', flex: 0.2, filterable: false
    }, {
        field: 'edit',
        headerName: 'Edit',
        sortable: false,
        flex: 0.5,
        filterable: false,
        renderCell: (params) => {
            const onClick = (e: any) => {
                e.stopPropagation();
                const path = "/admin/users/" + params.getValue(params.id, "id")
                navigate(path)
            };

            return <Button variant="contained"
                           endIcon={<ArrowForwardIcon/>}
                           sx={{borderRadius: "20px", px: 4}}
                           onClick={onClick}>Edit</Button>;
        },
    },];
    
    
    const FilterForm = ( props:FilterFormProps ) => {
        return (
            <form style={{width: "auto"}}>
                <FormControl style={{margin: 10}}>
                    <InputLabel id="selectColumn">Column</InputLabel>
                    <SelectColumnDropdown onChange={
                        (event: any) => setFilterModel(
                            prevState => (prevState.map
                                (el => 
                                    (el.id === props.id ? 
                                            {
                                        ...el, filterColumn: event.target.value
                                            }: el
                                    )
                                )
                            )
                        )
                    }/>
                </FormControl>
                <FormControl style={{margin: 10}}>
                    <InputLabel id="selectOperator">Operator</InputLabel>
                    <SelectOperatorDropdown  /*onChange={(event) => setFilterOperator(event.target.value)*//>
                </FormControl>
                <FormControl style={{margin: 10}}>
                    <TextField /*value={filterValue} onChange={(event) => setFilterValue(event.target.value)}*/
                        label="Search for"/>
                </FormControl>
            </form>
        )
    }
    const filterOperators = ["Equals", "Contains", "GreaterThan", "LessThan", "StartsWith"];
    const [filters, setFilters] = useState<any[]>([]);
    const addFilter = () => {
        const nextId = filterModel.length + 1
        const filterItem: FilterItem = {
            id: nextId,
            filterColumn: "",
            filterOperator: "",
            filterValue: ""
        }
        setFilters(filters => [...filters, <FilterForm id={nextId}/>]);
        setFilterModel(filterModel => [...filterModel, filterItem])
    }
    const SelectColumnDropdown = () => {
        const renderOption = (text: string) => {
            return <MenuItem value={text}>{text}</MenuItem>
        }
        const columnsOptions: any[] =
            columns
                .filter(name => (name.headerName !== undefined && name.headerName !== 'Edit'))
                .map(opt => opt.headerName);

        return (
            <Select 
                labelId="selectColumn" label="Column" sx={{minWidth: 170}}
            >{columnsOptions.map(renderOption)}</Select>
        )
    }

    const SelectOperatorDropdown = () => {
        const renderOption = (text: string) => {
            return <MenuItem value={text}>{text}</MenuItem>
        }
        const operatorsOptions: string[] = filterOperators.map(a => a);

        return (
            <Select
                labelId="selectOperator" label="Operator" sx={{minWidth: 170}}
            >{operatorsOptions.map(renderOption)}</Select>
        )
    }
    
    
    useEffect(() => {
        let active = true;

        (async () => {
            setLoading(true);

            if (sortModel.length === 0) {
                setSortModel([{field: 'id', sort: 'asc'}])
            }
            // @ts-ignore
            const res = await fetch('http://localhost:3500/users?_page=' + page + '&_limit=' + limitItems + '&_sort=' + sortModel[0].field.toString() + '&_order=' + sortModel[0].sort.toString());
            const newRows = await res.json();

            if (!active) {
                return;
            }

            setRows(newRows);
            setLoading(false);
            setTimeout(() => {
                setSelectionModel(prevSelectionModel.current)
            });
        })();
        // @ts-ignore

        return () => {
            active = false;
        };
    }, [page, limitItems, sortModel, filterModel]);
    console.log(filters)
    console.log(page, limitItems, sortModel, filterModel)
    return (
        <div>
            <div>
                <h1>Users page</h1>
            </div>
            <div>
                <Button onClick={addFilter}
                        startIcon={<AddIcon/>}
                        variant="contained"
                        sx={{borderRadius: "20px", px: 4}}
                        style={{margin: 10}}>Add filter</Button>
                {
                    filters.map((item) => (
                        <div>{item}</div>
                    )
                )}
                <DataGrid
                    columns={columns}
                    rows={rows}
                    disableColumnMenu={true}
                    pagination
                    pageSize={limitItems}
                    onPageSizeChange={(newPageSize) => setLimitItems(newPageSize)}
                    rowsPerPageOptions={[5, 10, 20]}
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
                    loading={loading}
                    autoHeight></DataGrid>
            </div>
        </div>
    );
}
