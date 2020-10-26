package main

import (
	"errors"

	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
)

// Context  instance represents a session with the database
// and can be used to query and save instances of your entities
type Context struct {
	Customer CustomerRepository
	db       *gorm.DB
}

// NewContext Creates a new db access context
func NewContext(dsn string) (*Context, error) {
	db, err := gorm.Open(sqlserver.Open(dsn), &gorm.Config{})
	if err != nil {
		return nil, err
	}

	return &Context{
		Customer: newCustomerRepository(db),
		db:       db,
	}, nil
}

// AutoMigrate runs auto migration for given models
func (ctx *Context) AutoMigrate() error {
	return ctx.db.AutoMigrate(&Customer{})
}

// sqlCustomerRepository implements the CustomerRepository interface
type sqlCustomerRepository struct {
	db *gorm.DB
}

func newCustomerRepository(db *gorm.DB) *sqlCustomerRepository {
	return &sqlCustomerRepository{db}
}

// GetCustomer returns a customer by ID
func (r *sqlCustomerRepository) GetCustomer(id uint64) (*Customer, error) {
	var customer Customer
	err := r.db.Debug().Where("id = ?", id).Take(&customer).Error
	if err != nil {
		return nil, errors.New("database error, please try again")
	}

	return &customer, nil
}

// GetAllCustomers returns all customers
func (r *sqlCustomerRepository) GetAllCustomers() ([]Customer, error) {
	var customers []Customer
	err := r.db.Debug().Limit(100).Find(&customers).Error
	if err != nil {
		return nil, err
	}

	return customers, nil
}

// AddCustomer persists a customer entity to the DB
func (r *sqlCustomerRepository) AddCustomer(customer *Customer) (*Customer, error) {
	err := r.db.Debug().Create(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// UpdateCustomer updates a customer entity in the DB
func (r *sqlCustomerRepository) UpdateCustomer(customer *Customer) (*Customer, error) {
	err := r.db.Debug().Save(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// DeleteCustomer deletes a customer entity in the DB
func (r *sqlCustomerRepository) DeleteCustomer(id uint64) error {
	var customer Customer
	err := r.db.Debug().Where("id = ?", id).Delete(&customer).Error
	if err != nil {
		return errors.New("database error, please try again")
	}
	return nil
}
