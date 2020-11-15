import React, { useState } from "react";
import moment from "moment";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import { KeyboardDateTimePicker } from "@material-ui/pickers";

import {
  LocalShippingRounded,
  ThreeSixtyRounded,
  AllInclusiveRounded,
  ArrowForwardRounded,
} from "@material-ui/icons";

import {
  Box,
  Button,
  colors,
  Container,
  makeStyles,
  TextField,
  Typography,
} from "@material-ui/core";

import {
  IconToggleButton,
  LocationSelect,
  Location,
  Page,
} from "src/components";

type ReactMouseEvent = React.MouseEvent<HTMLElement, MouseEvent>;

export function CreateOrder() {
  const styles = useStyles();
  const [orderType, setOrderType] = useState("haul");
  const [pickupLocation, setPickupLocation] = useState<Location>();
  const [deliveryLocation, setDeliveryLocation] = useState<Location>();
  const [pickupDate, setPickupDate] = useState(moment().format());

  function handleOrderTypeChange(event: ReactMouseEvent, newType: string) {
    if (newType !== null) {
      setOrderType(newType);
    }
  }

  function handlePickupLocationChange(newLocation: Location | null) {
    setPickupLocation(newLocation || undefined);
  }

  function handleDeliveryLocationChange(newLocation: Location | null) {
    setDeliveryLocation(newLocation || undefined);
  }

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
          <Box
            display="flex"
            flexDirection="column"
            bgcolor={colors.common.white}
            padding={2}
            mt={2}
            borderRadius={4}
          >
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

          <Box
            display="flex"
            pt={2}
            bgcolor={colors.common.white}
            padding={2}
            mt={2}
            borderRadius={4}
          >
            <Box display="flex" flexDirection="column">
              <Box pt={1}>
                <Typography className={styles.typeSectionTitle}>
                  Pick-up from
                </Typography>
                <Box pt={1}>
                  <LocationSelect onChange={handlePickupLocationChange} />
                </Box>
              </Box>

              <Box pt={2}>
                <Typography className={styles.typeSectionTitle}>
                  Pick-up time
                </Typography>
                <Box pt={1}>
                  <KeyboardDateTimePicker
                    variant="inline"
                    ampm
                    value={pickupDate}
                    style={{ width: 350 }}
                    TextFieldComponent={(props) => {
                      return (
                        <TextField
                          {...props}
                          label="Choose Pickup Time"
                          variant="outlined"
                          size="small"
                        />
                      );
                    }}
                    onChange={(d: any, _) => setPickupDate(d)}
                    disablePast
                    format="MM/DD/yyyy HH:mm a"
                  />
                </Box>
              </Box>
            </Box>

            <Box display="flex" pt={1} mx={3}>
              <Box
                display="flex"
                justifyContent="center"
                alignItems="flex-start"
                pt={3}
              >
                <ArrowForwardRounded />
              </Box>
            </Box>

            <Box display="flex" flexDirection="column">
              <Box pt={1}>
                <Typography className={styles.typeSectionTitle}>
                  Deliver to
                </Typography>
                <Box pt={1}>
                  <LocationSelect onChange={handleDeliveryLocationChange} />
                </Box>
              </Box>

              <Box pt={2}>
                <Typography className={styles.typeSectionTitle}>
                  Delivery time
                </Typography>
                <Box pt={1}>
                  <KeyboardDateTimePicker
                    variant="inline"
                    ampm
                    value={pickupDate}
                    style={{ width: 350 }}
                    TextFieldComponent={(props) => {
                      return (
                        <TextField
                          {...props}
                          label="Choose Pickup Time"
                          variant="outlined"
                          size="small"
                        />
                      );
                    }}
                    onChange={(d: any, _) => setPickupDate(d)}
                    disablePast
                    format="MM/DD/yyyy HH:mm a"
                  />
                </Box>
              </Box>
            </Box>
          </Box>

          <Box
            display="flex"
            pt={2}
            bgcolor={colors.common.white}
            padding={2}
            mt={2}
            borderRadius={4}
          >
            Material
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
    marginLeft: theme.spacing(),
  },
  continueButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing(),
    backgroundColor: colors.green[800],
  },
  typeSectionTitle: {
    color: colors.grey[600],
    fontSize: 13,
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
