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
	Name             string `json:"name"`
	Web              string `json:"web"`
	ContactFirstName string `json:"contact_first_name"`
	ContactLastName  string `json:"contact_last_name"`
	ContactEmail     string `json:"contact_email"`
	Phone1           string `json:"phone1"`
	Phone2           string `json:"phone2"`
	Address1         string `json:"address1"`
	Address2         string `json:"address2"`
	City             string `json:"city"`
	County           string `json:"county"`
	State            string `json:"state"`
	Country          string `json:"country"`
	Zip              string `json:"zip"`
}

type createLocationResponse struct {
	Location *domain.Location `json:"location"`
}

func makeCreateLocationEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		r := request.(createLocationRequest)
		location, err := s.CreateLocation(
			r.Name, r.Web, r.ContactFirstName, r.ContactLastName, r.ContactEmail, r.Phone1,
			r.Phone2, r.Address1, r.Address2, r.City, r.County, r.State, r.Country, r.Zip)
		return createLocationResponse{Location: location}, err
	}
}

type updateLocationRequest struct {
	ID               uint64 `json:"id"`
	Name             string `json:"name"`
	Web              string `json:"web"`
	ContactFirstName string `json:"contact_first_name"`
	ContactLastName  string `json:"contact_last_name"`
	ContactEmail     string `json:"contact_email"`
	Phone1           string `json:"phone1"`
	Phone2           string `json:"phone2"`
	Address1         string `json:"address1"`
	Address2         string `json:"address2"`
	City             string `json:"city"`
	County           string `json:"county"`
	State            string `json:"state"`
	Country          string `json:"country"`
	Zip              string `json:"zip"`
}
type updateLocationResponse struct {
	Location *domain.Location `json:"location"`
}

func makeUpdateLocationEndpoint(s application.LocationsService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		r := request.(updateLocationRequest)
		location, err := s.UpdateLocation(
			r.ID, r.Name, r.Web, r.ContactFirstName, r.ContactLastName, r.ContactEmail,
			r.Phone1, r.Phone2, r.Address1, r.Address2, r.City, r.County, r.State, r.Country, r.Zip)
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
