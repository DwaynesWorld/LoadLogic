import { CreateRouteApiRequest } from "./create-route-api-request";

export interface CreateOrderLineItemApiRequest {
  route: CreateRouteApiRequest;
  materialName: string;
  materialUnit: string;
  materialQuantity: number;
  materialWeight: number;
  materialDimensions: string;
}
