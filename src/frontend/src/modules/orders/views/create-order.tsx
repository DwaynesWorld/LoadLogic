import React, { useState } from "react";
import moment from "moment";
import { Box, Button, colors, Container, makeStyles } from "@material-ui/core";
import { Page } from "src/components";

import { Customer } from "src/models/customer";
import { Location } from "src/models/location";
import { OrderType } from "src/models/orders";
import { createOrder, CreateOrderApiRequest } from "src/api";
import { JobInfoSection } from "../components/create-order-job-info";
import { HaulingInfoSection } from "../components/create-order-hauling";

import {
  MaterialInfo,
  MaterialSection
} from "../components/create-order-material";

const INITIAL_MATERIAL_INFO: MaterialInfo = {
  name: undefined,
  quantity: undefined,
  unit: undefined,
  weight: undefined,
  length: undefined,
  safetyInfo: undefined
};

export function CreateOrder() {
  const styles = useStyles();
  const initialPickupDateTime = moment();
  const initialDeliveryDateTime = moment().add(4, "hours");

  const [orderType, setOrderType] = useState(OrderType.Haul);
  const [customer, setCustomer] = useState<Customer>();
  const [pickupLocation, setPickupLocation] = useState<Location>();
  const [deliveryLocation, setDeliveryLocation] = useState<Location>();
  const [pickupDate, setPickupDate] = useState(initialPickupDateTime);
  const [deliveryDate, setDeliveryDate] = useState(initialDeliveryDateTime);
  const [instructions, setInstructions] = useState("");
  const [materialInfo, setMaterialInfo] = useState(INITIAL_MATERIAL_INFO);

  async function handleCreateOrder() {
    if (!customer) return;
    if (!pickupLocation) return;
    if (!deliveryLocation) return;

    const newOrder: CreateOrderApiRequest = {
      type: orderType,
      customerId: customer.id,
      customerFirstName: customer.first_name,
      customerLastName: customer.last_name,
      customerEmail: customer.email,
      customerPhone: customer.phone,
      orderLineItems: [
        {
          route: {
            routeLegs: []
          },
          materialName: materialInfo.name ?? "",
          materialQuantity: materialInfo.quantity ?? 0,
          materialUnit: materialInfo.unit ?? "",
          materialWeight: materialInfo.weight ?? 0,
          materialDimensions: materialInfo.length ?? ""
        }
      ]
    };

    console.log("newOrder", newOrder);

    try {
      await createOrder(newOrder);
    } catch (error) {
      console.error(error);
    }
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
              onClick={handleCreateOrder}
            >
              Continue
            </Button>
          </Box>
        </Box>

        <Box display="flex" flexDirection="column">
          <JobInfoSection
            orderType={orderType}
            customer={customer}
            onOrderTypeChange={next => next && setOrderType(next)}
            onCustomerChange={next => setCustomer(next || undefined)}
          />
          <HaulingInfoSection
            pickupLocation={pickupLocation}
            pickupDate={pickupDate}
            deliveryLocation={deliveryLocation}
            deliveryDate={deliveryDate}
            instructions={instructions}
            onPickupDateChange={next => next && setPickupDate(next)}
            onPickupLocationChange={next =>
              setPickupLocation(next || undefined)
            }
            onDeliveryDateChange={next => next && setDeliveryDate(next)}
            onDeliveryLocationChange={next =>
              setDeliveryLocation(next || undefined)
            }
            onInstructionsChange={setInstructions}
          />
          <MaterialSection
            materialInfo={materialInfo}
            onMaterialInfoChange={setMaterialInfo}
          />
        </Box>
      </Container>
    </Page>
  );
}

export const useStyles = makeStyles(theme => ({
  saveButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing()
  },
  continueButton: {
    textTransform: "none",
    fontSize: 15,
    marginLeft: theme.spacing(),
    backgroundColor: colors.green[800]
  }
}));
