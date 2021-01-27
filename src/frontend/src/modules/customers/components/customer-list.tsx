import React, { useEffect, useState } from "react";
import Paper from "@material-ui/core/Paper";
import useSWR from "swr";

import { Customer } from "src/models/customer";
import { CustomersResponse, getAllCustomers } from "src/api";

import {
  Grid,
  Table,
  TableHeaderRow,
} from "@devexpress/dx-react-grid-material-ui";
import { AxiosResponse } from "axios";

const initColumns = [
  { name: "id", title: "ID" },
  { name: "name", title: "Name" },
  { name: "email", title: "Email" },
  { name: "phone", title: "Phone" },
];

export function CustomerList() {
  const { data, error } = useSWR<AxiosResponse<CustomersResponse>, unknown>(
    "/customers",
    getAllCustomers
  );

  const [columns] = useState(initColumns);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const loading = !error && !data;

  useEffect(() => {
    if (error) {
      return; // Handle
    }

    if (data) {
      setCustomers(data.data.customers);
    }
  }, [data, error]);

  if (loading) {
    return <div>Loading...</div>;
  }

  console.log(customers);

  return (
    <Paper>
      <Grid rows={customers} columns={columns}>
        <Table />
        <TableHeaderRow />
      </Grid>
    </Paper>
  );
}
