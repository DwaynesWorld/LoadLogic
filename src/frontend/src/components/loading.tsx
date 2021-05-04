import React from "react";
import { Box, Container, Typography } from "@material-ui/core";

export function FullPageLoading() {
  return (
    <Container>
      <Box>
        <Typography>LoadLogic</Typography>
      </Box>
      <Box>Loading</Box>
    </Container>
  );
}
