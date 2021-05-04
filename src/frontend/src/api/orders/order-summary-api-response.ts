import { OrderType } from "src/models/orders";

export interface OrderSummaryApiResponse {
  id: number;
  orderNo: string;
  type: OrderType;
  customerId: number;
  customerFirstName: string;
  customerLastName: string;
  customerEmail: string;
  customerPhone: string;
}
