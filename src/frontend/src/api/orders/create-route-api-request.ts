import { CreateRouteLegApiRequest } from "./create-route-leg-api-request";

export interface CreateRouteApiRequest {
  routeLegs: Array<CreateRouteLegApiRequest>;
}
