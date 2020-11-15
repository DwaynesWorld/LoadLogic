package middleware

import (
	"context"

	"github.com/DwaynesWorld/LoadLogic/src/locations/application"
	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
	"github.com/go-kit/kit/endpoint"
)

// EndpointSet is the set of all service endpoints
type EndpointSet struct {
	GetLocationEndpoint     endpoint.Endpoint
	GetAllLocationsEndpoint endpoint.Endpoint
	CreateLocationEndpoint  endpoint.Endpoint
	UpdateLocationEndpoint  endpoint.Endpoint
	DeleteLocationEndpoint  endpoint.Endpoint
}

// NewEndpoints creates set of enpoints to be used across one or more transports
func NewEndpoints(s application.LocationsService) EndpointSet {
	return EndpointSet{
		GetLocationEndpoint:     makeGetLocationEndpoint(s),
		GetAllLocationsEndpoint: makeGetAllLocationsEndpoint(s),
		CreateLocationEndpoint:  makeCreateLocationEndpoint(s),
		UpdateLocationEndpoint:  makeUpdateLocationEndpoint(s),
		DeleteLocationEndpoint:  makeDeleteLocationEndpoint(s),
	}
}

type getLocationRequest struct {
	ID uint64 `json:"id"`
}

type getLocationResponse struct {
	Location *domain.Location `json:"location"`
}

func makeGetLocationEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(getLocationRequest)
		location, err := s.GetLocation(req.ID)
		return getLocationResponse{Location: location}, err
	}
}

type getAllLocationsRequest struct {
}

type getAllLocationsResponse struct {
	Locations []domain.Location `json:"locations"`
}

func makeGetAllLocationsEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		locations, err := s.GetAllLocations()
		return getAllLocationsResponse{Locations: locations}, err
	}
}

type createLocationRequest struct {
	FirstName string `json:"first_name"`
	LastName  string `json:"last_name"`
	Email     string `json:"email"`
	Phone     string `json:"phone"`
}

type createLocationResponse struct {
	Location *domain.Location `json:"location"`
}

func makeCreateLocationEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(createLocationRequest)
		location, err := s.CreateLocation(req.FirstName, req.LastName, req.Email, req.Phone)
		return createLocationResponse{Location: location}, err
	}
}

type updateLocationRequest struct {
	ID        uint64 `json:"id"`
	FirstName string `json:"first_name"`
	LastName  string `json:"last_name"`
	Email     string `json:"email"`
	Phone     string `json:"phone"`
}
type updateLocationResponse struct {
	Location *domain.Location `json:"location"`
}

func makeUpdateLocationEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(updateLocationRequest)
		location, err := s.UpdateLocation(req.ID, req.FirstName, req.LastName, req.Email, req.Phone)
		return updateLocationResponse{Location: location}, err
	}
}

type deleteLocationRequest struct {
	ID uint64 `json:"id"`
}

type deleteLocationResponse struct {
}

func makeDeleteLocationEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(deleteLocationRequest)
		err := s.DeleteLocation(req.ID)
		return deleteLocationResponse{}, err
	}
}
