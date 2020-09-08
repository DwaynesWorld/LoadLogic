package main

// Customer entity.
type Customer struct {
	ID        uint64 `gorm:"primary_key;auto_increment" json:"id"`
	FirstName string `gorm:"size:100;not null;" json:"first_name"`
	LastName  string `gorm:"size:100;not null;" json:"last_name"`
	Email     string `gorm:"size:100;not null;" json:"email"`
	Phone     string `gorm:"size:20;not null;" json:"phone"`
}

// CustomerRepository is an interface for performing persistence operations.
type CustomerRepository interface {
	GetCustomer(uint64) (*Customer, error)
	GetAllCustomers() ([]Customer, error)
	AddCustomer(*Customer) (*Customer, error)
	UpdateCustomer(*Customer) (*Customer, error)
	DeleteCustomer(uint64) error
}
