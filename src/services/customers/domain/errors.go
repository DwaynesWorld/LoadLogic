package domain

import "errors"

// ErrCustomerNotFound is a domain specific error
// representing a customer not found
var ErrCustomerNotFound = errors.New("not found")

// ErrUnknownDatabaseError represents an unknown database error.
var ErrUnknownDatabaseError = errors.New("unknown database error has occurred")
