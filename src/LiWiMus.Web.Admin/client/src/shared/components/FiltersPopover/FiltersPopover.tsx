import {MenuItem, Popover, Select, TextField} from "@mui/material";
import React from "react";

type FiltersPopoverProps = {
    id: "simple-popover" | undefined,
    open: boolean,
    anchorEl: HTMLButtonElement | null,
    onClose: () => void,
    columnsOptions: any[],
    operatorsOptions: string[],
    onOperatorChange: (event: any) => void,
    onFilterValueChange: (event: any) => void,
    inputFilterValues: (string | number)[],
    inputFilterOperators: (string | number)[]
}

function FiltersPopover({id, open, anchorEl, onClose, columnsOptions, operatorsOptions, onOperatorChange, onFilterValueChange, inputFilterValues, inputFilterOperators}: FiltersPopoverProps) {
    return (<Popover
        id={id}
        open={open}
        anchorEl={anchorEl}
        onClose={() => (onClose())}
        anchorOrigin={{
            vertical: 'bottom',
            horizontal: 'right',
        }}
        transformOrigin={{
            vertical: 'top',
            horizontal: 'left'
        }}>
        {
            columnsOptions.map((column, number) =>
                <div style={{padding:"5px"}} id={number.toString()}>
                    <div>
                       <div style={{width:"50px", padding: "2px"}}><text  id={number.toString()}>{column}</text></div>
                        <Select value={inputFilterOperators[number]} id={number.toString()} native={true} onChange={(event: any) => onOperatorChange(event)}>
                            <option id={number.toString()} value=""><em>None</em></option>
                            {operatorsOptions.map((operator, num) =>
                                <option key={num} id={number.toString()} value={operator}>{operator}</option>)}
                        </Select>
                        <TextField inputProps={{maxLength:"50"}} value={inputFilterValues[number]} id={number.toString()} onChange={(event: any) => onFilterValueChange(event)}></TextField>
                    </div>
                </div>)
        }
    </Popover>)
}

export default FiltersPopover;