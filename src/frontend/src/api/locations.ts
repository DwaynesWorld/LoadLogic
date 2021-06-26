import axios from "axios";
import config from "src/app.config";
import { Location } from "../models/location";

export interface LocationsResponse {
  locations: Location[];
}

export async function getAllLocations(path = "/locations") {
  const baseUrl = config.endpoints.locations;
  const url = baseUrl + path;
  const response = await axios.get<LocationsResponse>(url);
  return response;
}
