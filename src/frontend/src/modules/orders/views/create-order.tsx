import React from "react";
import { Box, Button, colors, Container, makeStyles } from "@material-ui/core";
import { Page } from "src/components";
import { JobInfoSection } from "../components/create-order-job-info";
import { HaulingInfoSection } from "../components/create-order-hauling";
import { MaterialSection } from "../components/create-order-material";

export function CreateOrder() {
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
          <h2>Create Order</h2>
          <Box display="flex" flexDirection="row">
            <Button
              className={styles.saveButton}
              color="primary"
              variant="contained"
            >
              Save Draft
            </Button>
            <Button
              className={styles.continueButton}
              color="primary"
              variant="contained"
            >
              Continue
            </Button>
          </Box>
        </Box>

        <Box display="flex" flexDirection="column">
          <JobInfoSection />
          <HaulingInfoSection />
          <MaterialSection />
        </Box>
      </Container>
    </Page>
  );
}

export const useStyles = makeStyles((theme) => ({
  saveButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing(),
  },
  continueButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing(),
    backgroundColor: colors.green[800],
  },
}));
