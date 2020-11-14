import React, { useState } from "react";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";

import {
  LocalShippingRounded,
  ThreeSixtyRounded,
  AllInclusiveRounded,
} from "@material-ui/icons";

import {
  Box,
  Button,
  colors,
  Container,
  makeStyles,
  Typography,
} from "@material-ui/core";

import { IconToggleButton, Page } from "src/components";

type ReactMouseEvent = React.MouseEvent<HTMLElement, MouseEvent>;

export function CreateOrder() {
  const styles = useStyles();
  const [orderType, setOrderType] = useState("haul");

  const handleOrderTypeChange = (event: ReactMouseEvent, newType: string) => {
    if (newType !== null) {
      setOrderType(newType);
    }
  };

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

        <Box pt={1}>
          <Typography className={styles.typeSectionTitle}>Type</Typography>
          <ToggleButtonGroup
            className={styles.toggleGroup}
            size="large"
            value={orderType}
            exclusive
            onChange={handleOrderTypeChange}
          >
            <IconToggleButton
              icon={LocalShippingRounded}
              value="haul"
              title="One-Time Haul"
            />
            <IconToggleButton
              icon={ThreeSixtyRounded}
              value="onsite"
              title="On-Site Load/Dump"
            />
            <IconToggleButton
              icon={AllInclusiveRounded}
              value="multisite"
              title="Multi-Site Load/Dump"
            />
          </ToggleButtonGroup>
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
  typeSectionTitle: {
    color: colors.grey[600],
    fontSize: 14,
    fontWeight: theme.typography.fontWeightBold,
    textTransform: "uppercase",
  },
  toggleGroup: {
    paddingTop: theme.spacing(),
    "& > button": {
      color: colors.common.black,
      backgroundColor: colors.common.white,
      fontWeight: theme.typography.fontWeightBold,
      textTransform: "none",
      borderWidth: 1,
    },
    "& > button.Mui-selected": {
      color: colors.common.white,
      backgroundColor: colors.common.black,
    },
    "& > button.MuiToggleButtonGroup-groupedHorizontal:not(:first-child)": {
      borderLeft: `1px solid ${colors.grey[200]}`,
    },
    "& > button.MuiToggleButton-root:hover": {
      backgroundColor: colors.grey[100],
      transition: "0.5s",
    },
    "& > button.MuiToggleButton-root.Mui-selected:hover": {
      backgroundColor: "rgba(0, 0, 0, 0.85)",
    },
  },
}));
