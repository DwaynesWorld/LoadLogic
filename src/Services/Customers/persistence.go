package main

import (
	"errors"
	"fmt"
	"net/url"

	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
)

// DBConfig params for new context
type DBConfig struct {
	Scheme       string
	Hostname     string
	Port         int
	DatabaseName string
	Username     string
	Password     string
}

// Context :
type Context struct {
	Customer CustomerRepository
	db       *gorm.DB
}

// SQLCustomerRepository :
type SQLCustomerRepository struct {
	db *gorm.DB
}

// NewContext : Creates a new db access context
func NewContext(config *DBConfig) (*Context, error) {
	query := url.Values{}
	query.Add("database", config.DatabaseName)

	u := &url.URL{
		Scheme:   config.Scheme,
		User:     url.UserPassword(config.Username, config.Password),
		Host:     fmt.Sprintf("%s:%d", config.Hostname, config.Port),
		RawQuery: query.Encode(),
	}

	dsn := u.String()
	fmt.Println(dsn)

	db, err := gorm.Open(sqlserver.Open(dsn), &gorm.Config{})
	if err != nil {
		return nil, err
	}

	return &Context{
		Customer: NewCustomerRepository(db),
		db:       db,
	}, nil
}

// AutoMigrate : run auto migration for given models
func (ctx *Context) AutoMigrate() error {
	return ctx.db.AutoMigrate(&Customer{})
}

var _ CustomerRepository = &SQLCustomerRepository{}

// NewCustomerRepository :
func NewCustomerRepository(db *gorm.DB) *SQLCustomerRepository {
	return &SQLCustomerRepository{db}
}

// GetCustomer :
func (r *SQLCustomerRepository) GetCustomer(id uint64) (*Customer, error) {
	var customer Customer
	err := r.db.Debug().Where("id = ?", id).Take(&customer).Error
	if err != nil {
		return nil, errors.New("database error, please try again")
	}

	return &customer, nil
}

// GetAllCustomers :
func (r *SQLCustomerRepository) GetAllCustomers() ([]Customer, error) {
	var customers []Customer
	err := r.db.Debug().Limit(100).Find(&customers).Error
	if err != nil {
		return nil, err
	}

	return customers, nil
}

// AddCustomer :
func (r *SQLCustomerRepository) AddCustomer(customer *Customer) (*Customer, error) {
	err := r.db.Debug().Create(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// UpdateCustomer :
func (r *SQLCustomerRepository) UpdateCustomer(customer *Customer) (*Customer, error) {
	err := r.db.Debug().Save(&customer).Error
	if err != nil {
		return nil, err
	}
	return customer, nil
}

// DeleteCustomer :
func (r *SQLCustomerRepository) DeleteCustomer(id uint64) error {
	var customer Customer
	err := r.db.Debug().Where("id = ?", id).Delete(&customer).Error
	if err != nil {
		return errors.New("database error, please try again")
	}
	return nil
}
