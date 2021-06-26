import axios from "axios";
import config from "src/app.config";
import { CreateOrderApiRequest } from "./create-order-api-request";
import { OrderSummaryApiResponse } from "./order-summary-api-response";

export async function getAllOrders(path = "/orders") {
  const url = config.endpoints.orders + path;
  const response = await axios.get<OrderSummaryApiResponse[]>(url);
  return response;
}

export async function createOrder(newOrder: CreateOrderApiRequest) {
  const url = `${config.endpoints.orders}/orders`;
  const response = await axios.post<string>(url, newOrder);
  return response;
}
