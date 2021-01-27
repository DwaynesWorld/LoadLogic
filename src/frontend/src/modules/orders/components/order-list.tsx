import React, { useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import { Column } from "@devexpress/dx-react-grid";

import {
  Grid,
  Table,
  TableHeaderRow,
} from "@devexpress/dx-react-grid-material-ui";
import { OrderSummary } from "src/models/orders";
import useSWR from "swr";
import { getAllOrders } from "src/api/orders";
import { AxiosResponse } from "axios";

const initColumns: Column[] = [
  { name: "id", title: "ID" },
  { name: "orderNo", title: "Order #" },
  { name: "jobName", title: "Job Name" },
  {
    name: "customer",
    title: "Customer",
    getCellValue: (o: OrderSummary) =>
      `${o.customerFirstName} ${o.customerLastName}`,
  },
  { name: "email", title: "Email" },
  { name: "phone", title: "Phone" },
];

export function OrderList() {
  const { data, error } = useSWR<AxiosResponse<OrderSummary[]>, unknown>(
    "/orders",
    getAllOrders
  );

  const [columns] = useState(initColumns);
  const [orders, setOrders] = useState<OrderSummary[]>([]);
  const loading = !error && !data;

  useEffect(() => {
    if (error) {
      return; // Handle
    }

    if (data) {
      setOrders(data.data);
    }
  }, [data, error]);

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <Paper>
      <Grid rows={orders} columns={columns}>
        <Table />
        <TableHeaderRow />
      </Grid>
    </Paper>
  );
}
