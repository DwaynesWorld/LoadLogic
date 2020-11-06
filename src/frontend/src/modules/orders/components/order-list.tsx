import React, { useState } from "react";
import Paper from "@material-ui/core/Paper";
import { Column } from "@devexpress/dx-react-grid";

import {
  Grid,
  Table,
  TableHeaderRow,
} from "@devexpress/dx-react-grid-material-ui";

const initColumns: Column[] = [
  { name: "id", title: "ID" },
  { name: "orderNo", title: "Order #" },
  { name: "job", title: "Job" },
  { name: "customer", title: "Customer" },
  { name: "email", title: "Email" },
  { name: "phone", title: "Phone" },
];

const initRows = [
  {
    id: 0,
    orderNo: 10001,
    job: "Highway 6 Lane Expansion",
    customer: "Dan Brooks",
    email: "dan.brooks@example.com",
    phone: "(232) 809-4432",
  },
  {
    id: 1,
    orderNo: 10002,
    job: "I-10 Section 320 Re-Paving",
    customer: "Martin Shully",
    email: "martin.shully@example.com",
    phone: "(723) 809-102",
  },
];

export function OrderList() {
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
