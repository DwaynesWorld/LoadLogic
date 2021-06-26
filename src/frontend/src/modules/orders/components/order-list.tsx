import useSWR from "swr";
import React, { useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import { AxiosError, AxiosResponse } from "axios";
import { Column } from "@devexpress/dx-react-grid";

import {
  Grid,
  Table,
  TableHeaderRow
} from "@devexpress/dx-react-grid-material-ui";

import { getAllOrders, OrderSummaryApiResponse } from "src/api/orders";

const initColumns: Column[] = [
  { name: "id", title: "ID" },
  { name: "orderNo", title: "Order #" },
  { name: "jobName", title: "Job Name" },
  {
    name: "customer",
    title: "Customer",
    getCellValue: (o: OrderSummaryApiResponse) =>
      `${o.customerFirstName} ${o.customerLastName}`
  },
  { name: "email", title: "Email" },
  { name: "phone", title: "Phone" }
];

export function OrderList() {
  const [columns] = useState(initColumns);
  const [orders, setOrders] = useState<OrderSummaryApiResponse[]>([]);
  const { data, error } = useSWR<
    AxiosResponse<OrderSummaryApiResponse[]>,
    AxiosError
  >("/orders", getAllOrders);

  const loading = !error && !data;

  useEffect(() => {
    if (error) {
      // eslint-disable-next-line no-console
      console.error(error);
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
