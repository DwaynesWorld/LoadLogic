package handler

import "github.com/DwaynesWorld/LoadLogic/src/customers/stores"

// Handler encapsulates endpoint logic
type Handler struct {
	customerStore stores.CustomerStore
}

// NewHandler creates a new Handler
func NewHandler(cs stores.CustomerStore) *Handler {
	return &Handler{
		customerStore: cs,
	}
}
