import axios from "axios";
import config from "src/app.config";
import { Customer } from "../models/customer";

export interface CustomersResponse {
  customers: Customer[];
}

export async function getAllCustomers(path = "/customers") {
  const baseUrl = config.endpoints.customers;
  const url = baseUrl + path;
  const response = await axios.get<CustomersResponse>(url);
  return response;
}

export interface CreateCustomerResponse {
  customer: Customer;
}
export async function createCustomer(customer: Customer) {
  const baseUrl = config.endpoints.customers;
  const url = `${baseUrl}/customers`;
  const response = await axios.post<CreateCustomerResponse>(url, customer);
  return response;
}
