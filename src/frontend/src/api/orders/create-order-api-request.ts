import { OrderType } from "src/models/orders";
import { CreateOrderLineItemApiRequest } from "./create-order-line-item-api-request";

export interface CreateOrderApiRequest {
  type: OrderType;
  customerId: number;
  customerFirstName: string;
  customerLastName: string;
  customerEmail: string;
  customerPhone: string;
  orderLineItems: Array<CreateOrderLineItemApiRequest>;
}
