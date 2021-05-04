import React from "react";
import { Moment } from "moment";
import { KeyboardDateTimePicker } from "@material-ui/pickers";
import { ArrowForwardRounded } from "@material-ui/icons";
import { Box, colors, TextField } from "@material-ui/core";

import { Location } from "src/models/location";
import { LocationSelect, InputLabel } from "src/components";

const DATETIME_FORMAT = "MM/DD/yyyy h:mm a";

interface Props {
  pickupLocation?: Location;
  deliveryLocation?: Location;
  pickupDate?: Moment;
  deliveryDate?: Moment;
  instructions: string;
  onPickupLocationChange: (next: Location | null) => void;
  onDeliveryLocationChange: (next: Location | null) => void;
  onPickupDateChange: (next: Moment | null) => void;
  onDeliveryDateChange: (next: Moment | null) => void;
  onInstructionsChange: (next: string) => void;
}
export function HaulingInfoSection({
  pickupLocation,
  deliveryLocation,
  pickupDate,
  deliveryDate,
  instructions,
  onPickupLocationChange,
  onDeliveryLocationChange,
  onPickupDateChange,
  onDeliveryDateChange,
  onInstructionsChange
}: Props) {
  function handleInstructionsChange(
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    onInstructionsChange(e.target.value);
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
              <LocationSelect
                location={pickupLocation || null}
                onChange={onPickupLocationChange}
              />
            </Box>
          </Box>

          <Box pt={2}>
            <InputLabel title="Pick-up time" />
            <Box pt={1}>
              <KeyboardDateTimePicker
                ampm
                autoOk
                disablePast
                hideTabs
                format={DATETIME_FORMAT}
                style={{ width: 350 }}
                value={pickupDate || null}
                variant="inline"
                onChange={onPickupDateChange}
                TextFieldComponent={props => (
                  <TextField
                    {...props}
                    placeholder="Choose Pickup Time"
                    variant="outlined"
                    size="small"
                  />
                )}
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
              <LocationSelect
                location={deliveryLocation || null}
                onChange={onDeliveryLocationChange}
              />
            </Box>
          </Box>

          <Box pt={2}>
            <InputLabel title="Deliver time" />
            <Box pt={1}>
              <KeyboardDateTimePicker
                variant="inline"
                ampm
                value={deliveryDate || null}
                style={{ width: 350 }}
                disablePast
                disableToolbar
                autoOk
                format={DATETIME_FORMAT}
                onChange={onDeliveryDateChange}
                TextFieldComponent={props => (
                  <TextField
                    {...props}
                    placeholder="Choose Pickup Time"
                    variant="outlined"
                    size="small"
                  />
                )}
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
              value={instructions}
              onChange={handleInstructionsChange}
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
