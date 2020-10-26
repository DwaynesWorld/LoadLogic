import React from "react";
import { Page } from "src/components";
import { Container } from "@material-ui/core";

export function Dashboard() {
  return (
    <Page title="Dashboard">
      <Container maxWidth={false}>
        <h2>Dashboard</h2>
      </Container>
    </Page>
  );
}
