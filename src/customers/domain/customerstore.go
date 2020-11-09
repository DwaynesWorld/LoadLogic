package domain

import (
	"errors"
	"strings"

	"gorm.io/gorm"
)

// CustomerStore is an interface for performing persistence operations.
type CustomerStore interface {
	Find(uint64) (*Customer, error)
	FindAll() ([]Customer, error)
	Create(*Customer) (*Customer, error)
	Update(*Customer) (*Customer, error)
	Delete(uint64) error
}

type sqlCustomerStore struct {
	db *gorm.DB
}

// NewCustomerStore create a new instance of CustomerStore
func NewCustomerStore(db *gorm.DB) CustomerStore {
	return &sqlCustomerStore{db}
}

// Find returns a customer by ID
func (s *sqlCustomerStore) Find(id uint64) (*Customer, error) {
	var customer Customer
	err := s.db.Debug().Where("id = ?", id).Take(&customer).Error
	if err != nil {
		if strings.Contains(err.Error(), "record not found") {
			return nil, ErrCustomerNotFound
		}

		return nil, ErrUnknownDatabaseError
	}

	return &customer, nil
}

// FindAll returns all customers
func (s *sqlCustomerStore) FindAll() ([]Customer, error) {
	var customers []Customer
	err := s.db.Debug().Limit(100).Find(&customers).Error
	if err != nil {
		return nil, err
	}

	return customers, nil
}

// Create persists a customer entity to the DB
func (s *sqlCustomerStore) Create(customer *Customer) (*Customer, error) {
	err := s.db.Debug().Create(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// Update updates a customer entity in the DB
func (s *sqlCustomerStore) Update(customer *Customer) (*Customer, error) {
	err := s.db.Debug().Save(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// Delete deletes a customer entity in the DB
func (s *sqlCustomerStore) Delete(id uint64) error {
	var customer Customer
	err := s.db.Debug().Where("id = ?", id).Delete(&customer).Error
	if err != nil {
		return errors.New("database error, please try again")
	}
	return nil
}
