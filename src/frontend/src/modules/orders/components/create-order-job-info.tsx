import React, { useState } from "react";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import { Customer } from "src/models/customer";
import { CustomerCreateDialog } from "src/modules/customers";

import {
  LocalShippingRounded,
  ThreeSixtyRounded,
  AllInclusiveRounded,
} from "@material-ui/icons";

import { Box, colors, makeStyles, Typography } from "@material-ui/core";
import { IconToggleButton, InputLabel, CustomerSelect } from "src/components";

type ReactMouseEvent = React.MouseEvent<HTMLElement, MouseEvent>;

export function JobInfoSection() {
  const styles = useStyles();
  const [orderType, setOrderType] = useState("haul");
  const [customer, setCustomer] = useState<Customer | null>(null);
  const [createCustomerIsOpen, setCreateCustomerIsOpen] = useState(false);

  function handleOrderTypeChange(event: ReactMouseEvent, newType: string) {
    if (newType !== null) setOrderType(newType);
  }

  function handleCustomerChange(newCustomer: Customer | null) {
    setCustomer(newCustomer);
  }

  function handleCustomerCreate(newCustomer: Customer) {
    setCustomer(newCustomer);
    setCreateCustomerIsOpen(false);
  }

  return (
    <>
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

        <Box pt={2}>
          <Box pt={1} mr={2}>
            <InputLabel title="Customer" />
            <Box pt={1}>
              <CustomerSelect
                value={customer}
                onChange={handleCustomerChange}
                onCreate={() => setCreateCustomerIsOpen(true)}
              />
            </Box>
          </Box>
        </Box>
      </Box>

      {createCustomerIsOpen && (
        <CustomerCreateDialog
          open
          onSaved={handleCustomerCreate}
          onClose={() => setCreateCustomerIsOpen(false)}
        />
      )}
    </>
  );
}

export const useStyles = makeStyles((theme) => ({
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
