package domain

import "errors"

// ErrLocationNotFound is a domain specific error
// representing a location not found
var ErrLocationNotFound = errors.New("not found")

// ErrUnknownDatabaseError represents an unknown database error.
var ErrUnknownDatabaseError = errors.New("unknown database error has occurred")
