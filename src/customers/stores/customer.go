package stores

import (
	"errors"

	"github.com/DwaynesWorld/LoadLogic/src/customers/models"

	"gorm.io/gorm"
)

// CustomerStore is an interface for performing persistence operations.
type CustomerStore interface {
	GetCustomer(uint64) (*models.Customer, error)
	GetAllCustomers() ([]models.Customer, error)
	AddCustomer(*models.Customer) (*models.Customer, error)
	UpdateCustomer(*models.Customer) (*models.Customer, error)
	DeleteCustomer(uint64) error
}

type sqlCustomerStore struct {
	db *gorm.DB
}

// NewCustomerStore create a new instance of CustomerStore
func NewCustomerStore(db *gorm.DB) *CustomerStore {
	return &sqlCustomerStore{db}
}

// GetCustomer returns a customer by ID
func (s *sqlCustomerStore) GetCustomer(id uint64) (*models.Customer, error) {
	var customer models.Customer
	err := s.db.Debug().Where("id = ?", id).Take(&customer).Error
	if err != nil {
		return nil, errors.New("database error, please try again")
	}

	return &customer, nil
}

// GetAllCustomers returns all customers
func (s *sqlCustomerStore) GetAllCustomers() ([]models.Customer, error) {
	var customers []models.Customer
	err := s.db.Debug().Limit(100).Find(&customers).Error
	if err != nil {
		return nil, err
	}

	return customers, nil
}

// AddCustomer persists a customer entity to the DB
func (s *sqlCustomerStore) AddCustomer(customer *models.Customer) (*models.Customer, error) {
	err := s.db.Debug().Create(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// UpdateCustomer updates a customer entity in the DB
func (s *sqlCustomerStore) UpdateCustomer(customer *models.Customer) (*models.Customer, error) {
	err := s.db.Debug().Save(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// DeleteCustomer deletes a customer entity in the DB
func (s *sqlCustomerStore) DeleteCustomer(id uint64) error {
	var customer models.Customer
	err := s.db.Debug().Where("id = ?", id).Delete(&customer).Error
	if err != nil {
		return errors.New("database error, please try again")
	}
	return nil
}
