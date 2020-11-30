import React from "react";
import {
  Box,
  colors,
  makeStyles,
  TextField,
  TextFieldProps,
  Typography,
} from "@material-ui/core";

// prettier-ignore
type GridSize = boolean | "auto" | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | undefined;

export interface BaseFormField {
  xs: GridSize;
  sm?: GridSize;
  required?: boolean;
  title: string;
  id: string;
  name: string;
  placeholder: string;
  autoComplete?: string;
}

export type InputFieldProps = TextFieldProps & {
  showLabel?: boolean;
  title?: string;
};

export function InputField({
  title = "",
  showLabel,
  ...rest
}: InputFieldProps) {
  const styles = useStyles();

  return (
    <>
      {showLabel && <Typography className={styles.label}>{title}</Typography>}
      <Box pt={1}>
        <TextField {...rest} fullWidth variant="outlined" size="small" />
      </Box>
    </>
  );
}

const useStyles = makeStyles((theme) => ({
  label: {
    color: colors.grey[600],
    fontSize: 13,
    fontWeight: theme.typography.fontWeightBold,
    textTransform: "uppercase",
  },
}));
