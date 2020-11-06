import React from "react";
import { Page } from "src/components";
import { Box, Button, Container, makeStyles } from "@material-ui/core";

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
          </Box>
        </Box>
      </Container>
    </Page>
  );
}

const useStyles = makeStyles((theme) => ({
  saveButton: {
    textTransform: "none",
    fontSize: 15,
    fontWeight: theme.typography.fontWeightMedium,
  },
}));
