import axios from "axios";
import config from "src/app.config";
import { OrderSummary } from "src/models/orders";

export async function getAllOrders(path = "/orders") {
  const baseUrl = config.endpoints.orders_baseurl;
  const url = baseUrl + path;
  const response = await axios.get<OrderSummary[]>(url);
  return response;
}
