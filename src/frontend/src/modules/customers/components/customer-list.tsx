import React, { useState } from "react";
import Paper from "@material-ui/core/Paper";

import {
  Grid,
  Table,
  TableHeaderRow,
} from "@devexpress/dx-react-grid-material-ui";

const initColumns = [
  { name: "id", title: "ID" },
  { name: "name", title: "Name" },
  { name: "email", title: "Email" },
  { name: "phone", title: "Phone" },
];

const initRows = [
  {
    id: 0,
    name: "Dan Brooks",
    email: "dan.brooks@example.com",
    phone: "(232) 809-4432",
  },
  {
    id: 1,
    name: "Martin Shully",
    email: "martin.shully@example.com",
    phone: "(723) 809-102",
  },
];

export function CustomerList() {
  const [columns] = useState(initColumns);
  const [rows] = useState(initRows);

  return (
    <Paper>
      <Grid rows={rows} columns={columns}>
        <Table />
        <TableHeaderRow />
      </Grid>
    </Paper>
  );
}
