import { Address } from "src/models/address";
import { OrderType } from "src/models/orders";

export interface CreateRouteLegApiRequest {
  type: OrderType;
  legOrder: number;
  address: Address;
  timestamp: Date;
}
