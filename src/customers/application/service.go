package application

import (
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
)

// CustomersService is the interface that provides customer operations.
type CustomersService interface {
	GetCustomer(id uint64) (*domain.Customer, error)
	GetAllCustomers() ([]domain.Customer, error)
	CreateCustomer(firstname string, lastname string, email string, phone string) (*domain.Customer, error)
	UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (*domain.Customer, error)
	DeleteCustomer(id uint64) error
}

// NewService creates a customer service with necessary dependencies.
func NewService(store domain.CustomerStore) CustomersService {
	return &customerService{
		store: store,
	}
}

type customerService struct {
	store domain.CustomerStore
}

func (s *customerService) GetCustomer(id uint64) (*domain.Customer, error) {
	return s.store.Find(id)
}

func (s *customerService) GetAllCustomers() ([]domain.Customer, error) {
	return s.store.FindAll()
}

func (s *customerService) CreateCustomer(firstname string, lastname string, email string, phone string) (*domain.Customer, error) {
	c := &domain.Customer{
		ID:        0,
		FirstName: firstname,
		LastName:  lastname,
		Email:     email,
		Phone:     phone,
	}
	return s.store.Create(c)
}

func (s *customerService) UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (*domain.Customer, error) {
	c := &domain.Customer{
		ID:        id,
		FirstName: firstname,
		LastName:  lastname,
		Email:     email,
		Phone:     phone,
	}
	return s.store.Update(c)
}

func (s *customerService) DeleteCustomer(id uint64) error {
	return s.store.Delete(id)
}
