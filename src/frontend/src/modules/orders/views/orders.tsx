import React from "react";
import { Page } from "src/components";
import { Box, Button, Container, makeStyles } from "@material-ui/core";
import { Link } from "react-router-dom";

import { OrderList } from "../components/order-list";

export function Orders() {
  const styles = useStyles();

  return (
    <Page title="Orders">
      <Container maxWidth={false}>
        <Box
          display="flex"
          flexDirection="row"
          justifyContent="space-between"
          pt={2}
        >
          <h2>Orders</h2>
          <Box display="flex" flexDirection="row">
            <Link to="create">
              <Button
                className={styles.createButton}
                color="primary"
                variant="contained"
              >
                Create Order
              </Button>
            </Link>
          </Box>
        </Box>
      </Container>
      <Container maxWidth={false}>
        <Box pt={3}>
          <OrderList />
        </Box>
      </Container>
    </Page>
  );
}

const useStyles = makeStyles((theme) => ({
  createButton: {
    textTransform: "none",
    fontSize: 15,
    fontWeight: theme.typography.fontWeightMedium,
  },
}));
