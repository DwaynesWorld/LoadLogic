package middleware

import (
	"context"

	"github.com/DwaynesWorld/LoadLogic/src/customers/application"
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/go-kit/kit/endpoint"
)

// EndpointSet is the set of all service endpoints
type EndpointSet struct {
	GetCustomerEndpoint     endpoint.Endpoint
	GetAllCustomersEndpoint endpoint.Endpoint
	CreateCustomerEndpoint  endpoint.Endpoint
	UpdateCustomerEndpoint  endpoint.Endpoint
	DeleteCustomerEndpoint  endpoint.Endpoint
}

// NewEndpoints creates set of enpoints to be used across one or more transports
func NewEndpoints(s application.CustomersService) EndpointSet {
	return EndpointSet{
		GetCustomerEndpoint:     makeGetCustomerEndpoint(s),
		GetAllCustomersEndpoint: makeGetAllCustomersEndpoint(s),
		CreateCustomerEndpoint:  makeCreateCustomerEndpoint(s),
		UpdateCustomerEndpoint:  makeUpdateCustomerEndpoint(s),
		DeleteCustomerEndpoint:  makeDeleteCustomerEndpoint(s),
	}
}

type getCustomerRequest struct {
	ID uint64 `json:"id"`
}

type getCustomerResponse struct {
	Customer *domain.Customer `json:"customer"`
}

func makeGetCustomerEndpoint(s application.CustomersService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(getCustomerRequest)
		customer, err := s.GetCustomer(req.ID)
		return getCustomerResponse{Customer: customer}, err
	}
}

type getAllCustomersRequest struct {
}

type getAllCustomersResponse struct {
	Customers []domain.Customer `json:"customers"`
}

func makeGetAllCustomersEndpoint(s application.CustomersService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		customers, err := s.GetAllCustomers()
		return getAllCustomersResponse{Customers: customers}, err
	}
}

type createCustomerRequest struct {
	FirstName string `json:"first_name"`
	LastName  string `json:"last_name"`
	Email     string `json:"email"`
	Phone     string `json:"phone"`
}

type createCustomerResponse struct {
	Customer *domain.Customer `json:"customer"`
}

func makeCreateCustomerEndpoint(s application.CustomersService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(createCustomerRequest)
		customer, err := s.CreateCustomer(req.FirstName, req.LastName, req.Email, req.Phone)
		return createCustomerResponse{Customer: customer}, err
	}
}

type updateCustomerRequest struct {
	ID        uint64 `json:"id"`
	FirstName string `json:"first_name"`
	LastName  string `json:"last_name"`
	Email     string `json:"email"`
	Phone     string `json:"phone"`
}
type updateCustomerResponse struct {
	Customer *domain.Customer `json:"customer"`
}

func makeUpdateCustomerEndpoint(s application.CustomersService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(updateCustomerRequest)
		customer, err := s.UpdateCustomer(req.ID, req.FirstName, req.LastName, req.Email, req.Phone)
		return updateCustomerResponse{Customer: customer}, err
	}
}

type deleteCustomerRequest struct {
	ID uint64 `json:"id"`
}

type deleteCustomerResponse struct {
}

func makeDeleteCustomerEndpoint(s application.CustomersService) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(deleteCustomerRequest)
		err := s.DeleteCustomer(req.ID)
		return deleteCustomerResponse{}, err
	}
}
