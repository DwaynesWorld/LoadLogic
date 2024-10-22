package middleware

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"
	"strconv"
	"time"

	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/etherlabsio/healthcheck"
	kithttp "github.com/go-kit/kit/transport/http"
	"github.com/gorilla/mux"
	"github.com/prometheus/client_golang/prometheus/promhttp"
	log "github.com/sirupsen/logrus"
)

// NewHTTPTransport creates an HTTP transport for the customer service.
func NewHTTPTransport(endpoints EndpointSet, httpLogger *log.Entry) {
	mux := http.NewServeMux()
	mux.Handle("/v1/", newHTTPHandler(endpoints, httpLogger))
	http.Handle("/", cors(mux))
	http.Handle("/metrics", promhttp.Handler())
	http.Handle("/health", newHealthCheckHandler())
}

func newHTTPHandler(endpoints EndpointSet, logger *log.Entry) http.Handler {
	opts := []kithttp.ServerOption{
		kithttp.ServerErrorHandler(newLogErrorHandler(logger)),
		kithttp.ServerErrorEncoder(encodeError),
	}

	getAllCustomersHandler := kithttp.NewServer(
		endpoints.GetAllCustomersEndpoint,
		decodeGetAllCustomersRequest,
		encodeResponse,
		opts...,
	)

	getCustomerHandler := kithttp.NewServer(
		endpoints.GetCustomerEndpoint,
		decodeGetCustomerRequest,
		encodeResponse,
		opts...,
	)

	createCustomerHandler := kithttp.NewServer(
		endpoints.CreateCustomerEndpoint,
		decodeCreateCustomerRequest,
		encodeResponse,
		opts...,
	)

	updateCustomerHandler := kithttp.NewServer(
		endpoints.UpdateCustomerEndpoint,
		decodeUpdateCustomerRequest,
		encodeResponse,
		opts...,
	)

	deleteCustomerHandler := kithttp.NewServer(
		endpoints.DeleteCustomerEndpoint,
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

func newHealthCheckHandler() http.Handler {
	r := mux.NewRouter()
	r.Handle("/health", healthcheck.Handler(
		// WithTimeout allows you to set a max overall timeout.
		healthcheck.WithTimeout(5*time.Second),

		// // Checkers fail the status in case of any error.
		// healthcheck.WithChecker(
		// 	"heartbeat", checkers.Heartbeat("$PROJECT_PATH/heartbeat"),
		// ),

		// healthcheck.WithChecker(
		// 	"database", healthcheck.CheckerFunc(
		// 		func(ctx context.Context) error {
		// 			return db.PingContext(ctx)
		// 		},
		// 	),
		// ),

		// // Observers do not fail the status in case of error.
		// healthcheck.WithObserver(
		// 	"diskspace", checkers.DiskSpace("/var/log", 90),
		// ),
	))

	return r
}

// ErrBadRequest is a http specific error
// representing a bad http request
var ErrBadRequest = errors.New("bad request")

func decodeGetAllCustomersRequest(_ context.Context, r *http.Request) (interface{}, error) {
	return getAllCustomersRequest{}, nil
}

func decodeGetCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	vars := mux.Vars(r)
	paramID, ok := vars["id"]

	if !ok {
		return nil, ErrBadRequest
	}

	id, err := strconv.ParseUint(paramID, 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid customer ID, it must be a uint64", paramID)
		return nil, errors.New(msg)
	}

	return getCustomerRequest{ID: id}, nil
}

func decodeCreateCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	customer := createCustomerRequest{}

	if err := json.NewDecoder(r.Body).Decode(&customer); err != nil {
		return nil, err
	}

	return customer, nil
}

func decodeUpdateCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	customer := updateCustomerRequest{}

	if err := json.NewDecoder(r.Body).Decode(&customer); err != nil {
		return nil, err
	}

	return customer, nil
}

func decodeDeleteCustomerRequest(_ context.Context, r *http.Request) (interface{}, error) {
	vars := mux.Vars(r)
	paramID, ok := vars["id"]

	if !ok {
		return nil, ErrBadRequest
	}

	id, err := strconv.ParseUint(paramID, 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid customer ID, it must be a uint64", paramID)
		return nil, errors.New(msg)
	}

	return deleteCustomerRequest{ID: id}, nil
}

func encodeResponse(ctx context.Context, w http.ResponseWriter, response interface{}) error {
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
	case ErrBadRequest:
		w.WriteHeader(http.StatusBadRequest)
	default:
		w.WriteHeader(http.StatusInternalServerError)
	}

	json.NewEncoder(w).Encode(map[string]interface{}{
		"error": err.Error(),
	})
}

type logErrorHandler struct {
	logger *log.Entry
}

func newLogErrorHandler(logger *log.Entry) *logErrorHandler {
	return &logErrorHandler{
		logger: logger,
	}
}

func (h *logErrorHandler) Handle(ctx context.Context, err error) {
	h.logger.Error(err)
}

func cors(h http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "*")
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS")
		w.Header().Set("Access-Control-Allow-Headers", "Origin, Content-Type")

		if r.Method == "OPTIONS" {
			return
		}

		h.ServeHTTP(w, r)
	})
}
