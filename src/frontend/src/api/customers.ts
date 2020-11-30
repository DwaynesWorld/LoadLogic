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

export async function getAllCustomers(path: string) {
  const baseUrl = config.endpoints.customers_baseurl;
  const url = baseUrl + path;
  const response = await axios.get<CustomersResponse>(url);
  return response.data;
}
