package middleware

import (
	"context"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"
	"strconv"

	kithttp "github.com/go-kit/kit/transport/http"
	"github.com/prometheus/client_golang/prometheus/promhttp"
	log "github.com/sirupsen/logrus"

	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
	"github.com/gorilla/mux"
)

// NewHTTPTransport creates an HTTP transport for the location service.
func NewHTTPTransport(endpoints EndpointSet, httpLogger *log.Entry) {
	mux := http.NewServeMux()
	mux.Handle("/v1/", newHTTPHandler(endpoints, httpLogger))

	http.Handle("/", cors(mux))
	http.Handle("/metrics", promhttp.Handler())
}

func newHTTPHandler(endpoints EndpointSet, logger *log.Entry) http.Handler {
	opts := []kithttp.ServerOption{
		kithttp.ServerErrorHandler(newLogErrorHandler(logger)),
		kithttp.ServerErrorEncoder(encodeError),
	}

	getAllLocationsHandler := kithttp.NewServer(
		endpoints.GetAllLocationsEndpoint,
		decodeGetAllLocationsRequest,
		encodeResponse,
		opts...,
	)

	getLocationHandler := kithttp.NewServer(
		endpoints.GetLocationEndpoint,
		decodeGetLocationRequest,
		encodeResponse,
		opts...,
	)

	createLocationHandler := kithttp.NewServer(
		endpoints.CreateLocationEndpoint,
		decodeCreateLocationRequest,
		encodeResponse,
		opts...,
	)

	updateLocationHandler := kithttp.NewServer(
		endpoints.UpdateLocationEndpoint,
		decodeUpdateLocationRequest,
		encodeResponse,
		opts...,
	)

	deleteLocationHandler := kithttp.NewServer(
		endpoints.DeleteLocationEndpoint,
		decodeDeleteLocationRequest,
		encodeResponse,
		opts...,
	)

	r := mux.NewRouter()

	r.Handle("/v1/locations", createLocationHandler).Methods(http.MethodPost)
	r.Handle("/v1/locations", getAllLocationsHandler).Methods(http.MethodGet)
	r.Handle("/v1/locations/{id:[0-9]+}", getLocationHandler).Methods(http.MethodGet)
	r.Handle("/v1/locations/{id:[0-9]+}", updateLocationHandler).Methods(http.MethodPut)
	r.Handle("/v1/locations/{id:[0-9]+}", deleteLocationHandler).Methods(http.MethodDelete)

	return r
}

// ErrBadRequest is a http specific error
// representing a bad http request
var ErrBadRequest = errors.New("bad request")

func decodeGetAllLocationsRequest(_ context.Context, r *http.Request) (interface{}, error) {
	return getAllLocationsRequest{}, nil
}

func decodeGetLocationRequest(_ context.Context, r *http.Request) (interface{}, error) {
	vars := mux.Vars(r)
	paramID, ok := vars["id"]

	if !ok {
		return nil, ErrBadRequest
	}

	id, err := strconv.ParseUint(paramID, 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid location ID, it must be a uint64", paramID)
		return nil, errors.New(msg)
	}

	return getLocationRequest{ID: id}, nil
}

func decodeCreateLocationRequest(_ context.Context, r *http.Request) (interface{}, error) {
	location := createLocationRequest{}

	if err := json.NewDecoder(r.Body).Decode(&location); err != nil {
		return nil, err
	}

	return location, nil
}

func decodeUpdateLocationRequest(_ context.Context, r *http.Request) (interface{}, error) {
	location := updateLocationRequest{}

	if err := json.NewDecoder(r.Body).Decode(&location); err != nil {
		return nil, err
	}

	return location, nil
}

func decodeDeleteLocationRequest(_ context.Context, r *http.Request) (interface{}, error) {
	vars := mux.Vars(r)
	paramID, ok := vars["id"]

	if !ok {
		return nil, ErrBadRequest
	}

	id, err := strconv.ParseUint(paramID, 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid location ID, it must be a uint64", paramID)
		return nil, errors.New(msg)
	}

	return deleteLocationRequest{ID: id}, nil
}

func encodeResponse(ctx context.Context, w http.ResponseWriter, response interface{}) error {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")
	return json.NewEncoder(w).Encode(response)
}

func encodeError(_ context.Context, err error, w http.ResponseWriter) {
	w.Header().Set("Content-Type", "application/json; charset=utf-8")

	switch err {
	case domain.ErrLocationNotFound:
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
