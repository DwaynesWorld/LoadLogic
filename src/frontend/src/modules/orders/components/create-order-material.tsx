import React from "react";
import { Box, colors, TextField } from "@material-ui/core";
import { InputLabel } from "src/components";

export function MaterialSection() {
  return (
    <Box
      display="flex"
      flexDirection="column"
      pt={2}
      bgcolor={colors.common.white}
      padding={2}
      mt={2}
      borderRadius={4}
    >
      <Box display="flex">
        <Box pt={1} mr={2}>
          <InputLabel title="Material" />
          <Box pt={1}>
            <TextField placeholder="Asphalt" variant="outlined" size="small" />
          </Box>
        </Box>

        <Box pt={1} mr={2}>
          <InputLabel title="Quantity" />
          <Box pt={1}>
            <TextField placeholder="Qty" variant="outlined" size="small" />
          </Box>
        </Box>

        <Box pt={1} mr={2}>
          <InputLabel title="Unit" />
          <Box pt={1}>
            <TextField placeholder="Each" variant="outlined" size="small" />
          </Box>
        </Box>

        <Box pt={1} mr={2}>
          <InputLabel title="Weight" />
          <Box pt={1}>
            <TextField placeholder="Lbs each" variant="outlined" size="small" />
          </Box>
        </Box>

        <Box pt={1}>
          <InputLabel title="Length" />
          <Box pt={1}>
            <TextField
              placeholder="L X W X H (inches)"
              variant="outlined"
              size="small"
            />
          </Box>
        </Box>
      </Box>

      <Box display="flex" pt={1}>
        <Box flex={1} pt={1}>
          <InputLabel title="Safety Information" />
          <Box pt={1}>
            <TextField
              rows={6}
              placeholder="Safety info..."
              multiline
              fullWidth
              variant="outlined"
              size="small"
            />
          </Box>
        </Box>
      </Box>
    </Box>
  );
}
