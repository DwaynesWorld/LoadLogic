package service

import (
	"context"

	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/go-kit/kit/endpoint"
)

type getCustomerRequest struct {
	ID uint64 `json:"id"`
}

type getCustomerResponse struct {
	Customer *domain.Customer `json:"customer"`
	Err      error            `json:"error,omitempty"`
}

func makeGetCustomerEndpoint(s Service) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(getCustomerRequest)
		customer, err := s.GetCustomer(req.ID)
		return getCustomerResponse{Customer: customer, Err: err}, nil
	}
}

type getAllCustomersRequest struct {
}

type getAllCustomersResponse struct {
	Customers []domain.Customer `json:"customers"`
	Err       error             `json:"error,omitempty"`
}

func makeGetAllCustomersEndpoint(s Service) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		customers, err := s.GetAllCustomers()
		return getAllCustomersResponse{Customers: customers, Err: err}, nil
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
	Err      error            `json:"error,omitempty"`
}

func makeCreateCustomerEndpoint(s Service) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(createCustomerRequest)
		customer, err := s.CreateCustomer(req.FirstName, req.LastName, req.Email, req.Phone)
		return createCustomerResponse{Customer: customer, Err: err}, nil
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
	Err      error            `json:"error,omitempty"`
}

func makeUpdateCustomerEndpoint(s Service) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(updateCustomerRequest)
		customer, err := s.UpdateCustomer(req.ID, req.FirstName, req.LastName, req.Email, req.Phone)
		return updateCustomerResponse{Customer: customer, Err: err}, nil
	}
}

type deleteCustomerRequest struct {
	ID uint64 `json:"id"`
}

type deleteCustomerResponse struct {
	Err error `json:"error,omitempty"`
}

func makeDeleteCustomerEndpoint(s Service) endpoint.Endpoint {
	return func(ctx context.Context, request interface{}) (interface{}, error) {
		req := request.(deleteCustomerRequest)
		err := s.DeleteCustomer(req.ID)
		return deleteCustomerResponse{Err: err}, nil
	}
}
