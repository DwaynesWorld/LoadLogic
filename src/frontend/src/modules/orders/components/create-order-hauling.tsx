import React, { useState } from "react";
import moment from "moment";
import { KeyboardDateTimePicker } from "@material-ui/pickers";
import { ArrowForwardRounded } from "@material-ui/icons";
import { Box, colors, TextField } from "@material-ui/core";
import { Location } from "src/models/location";
import { LocationSelect, InputLabel } from "src/components";

export function HaulingInfoSection() {
  const [pickupLocation, setPickupLocation] = useState<Location>();
  const [deliveryLocation, setDeliveryLocation] = useState<Location>();
  const [pickupDate, setPickupDate] = useState(moment().format());

  function handlePickupLocationChange(newLocation: Location | null) {
    setPickupLocation(newLocation || undefined);
  }

  function handleDeliveryLocationChange(newLocation: Location | null) {
    setDeliveryLocation(newLocation || undefined);
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
        <Box display="flex" flexDirection="column">
          <Box pt={1}>
            <InputLabel title="Pick-up from" />
            <Box pt={1}>
              <LocationSelect onChange={handlePickupLocationChange} />
            </Box>
          </Box>

          <Box pt={2}>
            <InputLabel title="Pick-up time" />
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
                      placeholder="Choose Pickup Time"
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
            <InputLabel title="Deliver to" />
            <Box pt={1}>
              <LocationSelect onChange={handleDeliveryLocationChange} />
            </Box>
          </Box>

          <Box pt={2}>
            <InputLabel title="Deliver time" />
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
                      placeholder="Choose Pickup Time"
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

      <Box display="flex" pt={2}>
        <Box flex={1} pt={1}>
          <InputLabel title="Special Instructions" />
          <Box pt={1}>
            <TextField
              rows={6}
              placeholder=""
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
