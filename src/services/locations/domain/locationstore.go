package domain

import (
	"errors"
	"strings"

	"gorm.io/gorm"
)

// LocationStore is an interface for performing persistence operations.
type LocationStore interface {
	Find(uint64) (*Location, error)
	FindAll() ([]Location, error)
	Create(*Location) (*Location, error)
	Update(*Location) (*Location, error)
	Delete(uint64) error
}

type sqlLocationStore struct {
	db *gorm.DB
}

// NewLocationStore create a new instance of LocationStore
func NewLocationStore(db *gorm.DB) LocationStore {
	return &sqlLocationStore{db}
}

// Find returns a location by ID
func (s *sqlLocationStore) Find(id uint64) (*Location, error) {
	var location Location
	err := s.db.Debug().Where("id = ?", id).Take(&location).Error
	if err != nil {
		if strings.Contains(err.Error(), "record not found") {
			return nil, ErrLocationNotFound
		}

		return nil, ErrUnknownDatabaseError
	}

	return &location, nil
}

// FindAll returns all locations
func (s *sqlLocationStore) FindAll() ([]Location, error) {
	var locations []Location
	err := s.db.Debug().Limit(100).Find(&locations).Error
	if err != nil {
		return nil, err
	}

	return locations, nil
}

// Create persists a location entity to the DB
func (s *sqlLocationStore) Create(location *Location) (*Location, error) {
	err := s.db.Debug().Create(&location).Error
	if err != nil {
		return nil, err
	}
	return location, nil
}

// Update updates a location entity in the DB
func (s *sqlLocationStore) Update(location *Location) (*Location, error) {
	err := s.db.Debug().Save(&location).Error
	if err != nil {
		return nil, err
	}
	return location, nil
}

// Delete deletes a location entity in the DB
func (s *sqlLocationStore) Delete(id uint64) error {
	var location Location
	err := s.db.Debug().Where("id = ?", id).Delete(&location).Error
	if err != nil {
		return errors.New("database error, please try again")
	}
	return nil
}
