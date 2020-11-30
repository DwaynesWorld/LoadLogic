import axios from "axios";
import config from "src/config";

export interface Customer {
  id?: number;
  first_name: string;
  last_name: string;
  phone: string;
  email: string;
}

interface CustomersResponse {
  customers: Customer[];
}

export async function getAllCustomers(path = "/customers") {
  const baseUrl = config.endpoints.customers_baseurl;
  const url = baseUrl + path;
  const response = await axios.get<CustomersResponse>(url);
  return response;
}

interface CreateCustomerResponse {
  customer: Customer;
}
export async function createCustomer(customer: Customer) {
  const baseUrl = config.endpoints.customers_baseurl;
  const url = `${baseUrl}/customers`;
  const response = await axios.post<CreateCustomerResponse>(url, customer);
  return response;
}
