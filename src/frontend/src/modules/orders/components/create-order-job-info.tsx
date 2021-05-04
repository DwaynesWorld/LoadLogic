import React, { useState } from "react";
import ToggleButtonGroup from "@material-ui/lab/ToggleButtonGroup";
import { Box, colors, makeStyles, Typography } from "@material-ui/core";

import {
  LocalShippingRounded,
  ThreeSixtyRounded,
  AllInclusiveRounded
} from "@material-ui/icons";

import { OrderType } from "src/models/orders";
import { Customer } from "src/models/customer";
import { CustomerCreateDialog } from "src/modules/customers";
import { IconToggleButton, InputLabel, CustomerSelect } from "src/components";

type ReactMouseEvent = React.MouseEvent<HTMLElement, MouseEvent>;

interface Props {
  orderType: OrderType;
  customer?: Customer;
  onOrderTypeChange: (next: OrderType) => void;
  onCustomerChange: (next: Customer | null) => void;
}
export function JobInfoSection({
  orderType,
  customer,
  onOrderTypeChange,
  onCustomerChange
}: Props) {
  const styles = useStyles();
  const [createCustomerIsOpen, setCreateCustomerIsOpen] = useState(false);

  function handleOrderTypeChange(_: ReactMouseEvent, next: OrderType) {
    if (next !== null) onOrderTypeChange(next);
  }

  function handleCustomerChange(next: Customer | null) {
    onCustomerChange(next);
  }

  function handleCustomerCreate(next: Customer) {
    onCustomerChange(next);
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
            value={OrderType.Haul}
            title="One-Time Haul"
          />
          <IconToggleButton
            icon={ThreeSixtyRounded}
            value={OrderType.OnSiteLoadDump}
            title="On-Site Load/Dump"
          />
          <IconToggleButton
            icon={AllInclusiveRounded}
            value={OrderType.MultiSiteLoadDump}
            title="Multi-Site Load/Dump"
          />
        </ToggleButtonGroup>

        <Box pt={2}>
          <Box pt={1} mr={2}>
            <InputLabel title="Customer" />
            <Box pt={1}>
              <CustomerSelect
                value={customer || null}
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

export const useStyles = makeStyles(theme => ({
  typeSectionTitle: {
    color: colors.grey[600],
    fontSize: 13,
    fontWeight: theme.typography.fontWeightBold,
    textTransform: "uppercase"
  },
  toggleGroup: {
    paddingTop: theme.spacing(),
    "& > button": {
      color: colors.common.black,
      backgroundColor: colors.common.white,
      fontWeight: theme.typography.fontWeightBold,
      textTransform: "none",
      borderWidth: 1
    },
    "& > button.Mui-selected": {
      color: colors.common.white,
      backgroundColor: colors.common.black
    },
    "& > button.MuiToggleButtonGroup-groupedHorizontal:not(:first-child)": {
      borderLeft: `1px solid ${colors.grey[200]}`
    },
    "& > button.MuiToggleButton-root:hover": {
      backgroundColor: colors.grey[100],
      transition: "0.5s"
    },
    "& > button.MuiToggleButton-root.Mui-selected:hover": {
      backgroundColor: "rgba(0, 0, 0, 0.85)"
    }
  }
}));
