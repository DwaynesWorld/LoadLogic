import axios from "axios";
import config from "src/config";

export interface Location {
  id?: number;
  address1: string;
  address2?: string;
  city: string;
  contact_email: string;
  contact_first_name: string;
  contact_last_name: string;
  county: string;
  name: string;
  phone1: string;
  phone2: string;
  state: string;
  country?: string;
  web: string;
  zip: string;
}

interface LocationsResponse {
  locations: Location[];
}

export async function getAllLocations(path: string) {
  const baseUrl = config.endpoints.locations_baseurl;
  const url = baseUrl + path;
  const response = await axios.get<LocationsResponse>(url);
  return response.data;
}
