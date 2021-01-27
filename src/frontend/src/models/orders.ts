import { Address } from "./address";

export interface OrderType {
  id: number;
  name: string;
}

export interface OrderSummary {
  id: number;
  orderNo: number;
  orderStatus: number;
  type: OrderType;
  customerId: number;
  customerFirstName: string;
  customerLastName: string;
  customerEmail: string;
  customerPhone: string;
  jobName: string;
  jobDescription: string;
  jobAddress: Address;
  jobStartDate: Date;
}
