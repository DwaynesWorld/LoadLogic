import React from "react";
import { Box, colors, TextField } from "@material-ui/core";
import { InputLabel } from "src/components";

export interface MaterialInfo {
  name?: string;
  quantity?: number;
  unit?: string;
  weight?: number;
  length?: string;
  safetyInfo?: string;
}
interface Props {
  materialInfo: MaterialInfo;
  onMaterialInfoChange: (next: MaterialInfo) => void;
}
export function MaterialSection({ materialInfo, onMaterialInfoChange }: Props) {
  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    onMaterialInfoChange({ ...materialInfo, [e.target.name]: e.target.value });
  }

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
            <TextField
              placeholder="Asphalt"
              name="name"
              value={materialInfo.name}
              onChange={handleChange}
              variant="outlined"
              size="small"
            />
          </Box>
        </Box>

        <Box pt={1} mr={2}>
          <InputLabel title="Quantity" />
          <Box pt={1}>
            <TextField
              placeholder="Qty"
              name="quantity"
              value={materialInfo.quantity}
              onChange={handleChange}
              variant="outlined"
              size="small"
            />
          </Box>
        </Box>

        <Box pt={1} mr={2}>
          <InputLabel title="Unit" />
          <Box pt={1}>
            <TextField
              placeholder="Each"
              name="unit"
              value={materialInfo.unit}
              onChange={handleChange}
              variant="outlined"
              size="small"
            />
          </Box>
        </Box>

        <Box pt={1} mr={2}>
          <InputLabel title="Weight" />
          <Box pt={1}>
            <TextField
              placeholder="Lbs each"
              name="weight"
              value={materialInfo.weight}
              onChange={handleChange}
              variant="outlined"
              size="small"
            />
          </Box>
        </Box>

        <Box pt={1}>
          <InputLabel title="Length" />
          <Box pt={1}>
            <TextField
              placeholder="L X W X H (inches)"
              name="length"
              value={materialInfo.length}
              onChange={handleChange}
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
              name="safetyInfo"
              value={materialInfo.safetyInfo}
              onChange={handleChange}
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
