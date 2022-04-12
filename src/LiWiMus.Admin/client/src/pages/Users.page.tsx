import React, {useState, useEffect} from "react";
import {DataGrid, GridRowsProp, GridColDef} from '@mui/x-data-grid'

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
        field: 'username', headerName: 'Username', flex:1
    },{
        field: 'email', headerName: 'Email', flex:1
    },  {
        field: 'phoneNumber', headerName: 'Phone Number', flex:1
    }, {
        field: 'enum', headerName: 'Gender', flex:1
    }, {
        field: 'emailConfirmed', headerName: 'Email Confirmed', flex:1
    }, {
        field: 'phoneConfirmed', headerName: 'Phone Confirmed',
    }, {
        field: 'balance', headerName: 'Balance', 
    }];

    return (
        <div>
            <div>
                <h1>Users page</h1>
            </div>
            <div>
                <DataGrid columns={columns} rows={rows} pageSize={8} checkboxSelection={true} autoHeight></DataGrid>
            </div>
        </div>
    );
}
