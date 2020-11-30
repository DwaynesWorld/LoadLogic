import React from "react";
import { colors, makeStyles, Typography } from "@material-ui/core";

interface Props {
  title: string;
}
export function InputLabel({ title }: Props) {
  const styles = useStyles();

  return <Typography className={styles.label}>{title}</Typography>;
}

const useStyles = makeStyles((theme) => ({
  label: {
    color: colors.grey[600],
    fontSize: 13,
    fontWeight: theme.typography.fontWeightBold,
    textTransform: "uppercase",
  },
}));
