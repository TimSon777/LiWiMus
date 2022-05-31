import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
// @ts-ignore
import dateFormat from "dateformat";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { Button, Fab } from "@mui/material";
import ArrowForwardIcon from "@mui/icons-material/ArrowForward";
import AddIcon from "@mui/icons-material/Add";
import { usePlanService } from "../PlanService.hook";

export default function PlansPage() {
  const planService = usePlanService();

  const [rows, setRows] = useState([]);
  const navigate = useNavigate();
  const columns: GridColDef[] = [
    { field: "id", hide: true },
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
            endIcon={<ArrowForwardIcon />}
            sx={{ borderRadius: "20px", px: 4 }}
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
      const response = await planService.getPlans();

      if (!active) {
        return;
      }
      setRows(response);
    })();
    // @ts-ignore

    return () => {
      active = false;
    };
  }, []);

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
        <Fab
          color="primary"
          sx={{ position: "fixed", bottom: 50, right: 50 }}
          aria-label="add"
          component={Link}
          to="/admin/plans/create"
        >
          <AddIcon />
        </Fab>
      </div>
    </div>
  );
}
