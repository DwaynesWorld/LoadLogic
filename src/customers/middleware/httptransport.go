package middleware

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"
	"strconv"

	kitlog "github.com/go-kit/kit/log"
	kitxport "github.com/go-kit/kit/transport"
	kithttp "github.com/go-kit/kit/transport/http"

	"github.com/DwaynesWorld/LoadLogic/src/customers/application"
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/gorilla/mux"
)

// MakeHandler returns a handler for the customer service.
func MakeHandler(s application.CustomersService, logger kitlog.Logger) http.Handler {
	opts := []kithttp.ServerOption{
		kithttp.ServerErrorHandler(kitxport.NewLogErrorHandler(logger)),
		kithttp.ServerErrorEncoder(encodeError),
	}

	getAllCustomersHandler := kithttp.NewServer(
		makeGetAllCustomersEndpoint(s),
		decodeGetAllCustomersRequest,
		encodeResponse,
		opts...,
	)

	getCustomerHandler := kithttp.NewServer(
		makeGetCustomerEndpoint(s),
		decodeGetCustomerRequest,
		encodeResponse,
		opts...,
	)

	createCustomerHandler := kithttp.NewServer(
		makeCreateCustomerEndpoint(s),
		decodeCreateCustomerRequest,
		encodeResponse,
		opts...,
	)

	updateCustomerHandler := kithttp.NewServer(
		makeUpdateCustomerEndpoint(s),
		decodeUpdateCustomerRequest,
		encodeResponse,
		opts...,
	)

	deleteCustomerHandler := kithttp.NewServer(
		makeDeleteCustomerEndpoint(s),
		decodeDeleteCustomerRequest,
		encodeResponse,
		opts...,
	)

	r := mux.NewRouter()

	r.Handle("/v1/customers", createCustomerHandler).Methods(http.MethodPost)
	r.Handle("/v1/customers", getAllCustomersHandler).Methods(http.MethodGet)
	r.Handle("/v1/customers/{id:[0-9]+}", getCustomerHandler).Methods(http.MethodGet)
	r.Handle("/v1/customers/{id:[0-9]+}", updateCustomerHandler).Methods(http.MethodPut)
	r.Handle("/v1/customers/{id:[0-9]+}", deleteCustomerHandler).Methods(http.MethodDelete)

	return r
}

func decodeGetAllCustomersRequest(_ context.Context, r *http.Request) (interface{}, error) {
	return getAllCustomersRequest{}, nil
}

func decodeGetCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	vars := mux.Vars(r)
	paramID, ok := vars["id"]

	if !ok {
		return nil, errors.New("Bad Request")
	}

	id, err := strconv.ParseUint(paramID, 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid customer ID, it must be a uint64", paramID)
		return nil, errors.New(msg)
	}

	return getCustomerRequest{ID: id}, nil
}

func decodeCreateCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	customer := &createCustomerRequest{}

	if err := json.NewDecoder(r.Body).Decode(&customer); err != nil {
		return nil, err
	}

	return customer, nil
}

func decodeUpdateCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	customer := &updateCustomerRequest{}

	if err := json.NewDecoder(r.Body).Decode(&customer); err != nil {
		return nil, err
	}

	return customer, nil
}

func decodeDeleteCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	vars := mux.Vars(r)
	paramID, ok := vars["id"]

	if !ok {
		return nil, errors.New("Bad Request")
	}

	id, err := strconv.ParseUint(paramID, 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid customer ID, it must be a uint64", paramID)
		return nil, errors.New(msg)
	}

	return deleteCustomerRequest{ID: id}, nil
}

type errorer interface {
	error() error
}

func encodeResponse(ctx context.Context, w http.ResponseWriter, response interface{}) error {
	if e, ok := response.(errorer); ok && e.error() != nil {
		encodeError(ctx, e.error(), w)
		return nil
	}

	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	return json.NewEncoder(w).Encode(response)
}

func encodeError(_ context.Context, err error, w http.ResponseWriter) {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")

	switch err {
	case domain.ErrCustomerNotFound:
		w.WriteHeader(http.StatusNotFound)
	case domain.ErrUnknownDatabaseError:
		w.WriteHeader(http.StatusFailedDependency)
	default:
		w.WriteHeader(http.StatusInternalServerError)
	}

	json.NewEncoder(w).Encode(map[string]interface{}{
		"error": err.Error(),
	})
}
