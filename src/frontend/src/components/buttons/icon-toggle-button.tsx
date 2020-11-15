import React from "react";
import ToggleButton, { ToggleButtonProps } from "@material-ui/lab/ToggleButton";
import { Box, Typography } from "@material-ui/core";
import { SvgIconComponent } from "@material-ui/icons";

interface Props extends ToggleButtonProps {
  icon: SvgIconComponent;
  title: string;
}
export function IconToggleButton({ icon: Icon, title, ...props }: Props) {
  return (
    <ToggleButton {...props}>
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignContent="center"
      >
        <Box display="flex" pr={1}>
          <Icon />
        </Box>
        <Box display="flex">
          <Typography variant="h6">{title}</Typography>
        </Box>
      </Box>
    </ToggleButton>
  );
}
