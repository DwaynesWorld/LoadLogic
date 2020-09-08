import React from "react";
import { Page } from "src/components";
import { Container, Box, Button, makeStyles } from "@material-ui/core";
import { CustomerList } from "../components/customer-list";

export function Customers() {
  const styles = useStyles();
  return (
    <Page className={styles.page} title="Customers">
      <Container maxWidth={false}>
        <Box display="flex" flexDirection="row" justifyContent="space-between">
          <h2>Customers</h2>
          <Box display="flex" flexDirection="row">
            <Button
              className={styles.importButton}
              color="default"
              variant="text"
            >
              Import Customers
            </Button>
            <Button
              className={styles.addButton}
              color="primary"
              variant="contained"
            >
              Add Customer
            </Button>
          </Box>
        </Box>
      </Container>
      <Container maxWidth={false}>
        <Box pt={3}>
          <CustomerList />
        </Box>
      </Container>
    </Page>
  );
}

const useStyles = makeStyles((theme) => ({
  page: {
    backgroundColor: theme.palette.background.default,
    minHeight: "100%",
    paddingBottom: theme.spacing(3),
    paddingTop: theme.spacing(3),
  },
  importButton: {
    marginRight: theme.spacing(1),
    textTransform: "none",
    fontSize: 15,
    fontWeight: theme.typography.fontWeightBold,
  },
  addButton: {
    textTransform: "none",
    fontSize: 15,
    fontWeight: theme.typography.fontWeightMedium,
  },
}));
