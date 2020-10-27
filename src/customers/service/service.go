package service

import (
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
)

// Service is the interface that provides customer operations.
type Service interface {
	GetCustomer(id uint64) (*domain.Customer, error)
	GetAllCustomers() ([]domain.Customer, error)
	CreateCustomer(firstname string, lastname string, email string, phone string) (*domain.Customer, error)
	UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (*domain.Customer, error)
	DeleteCustomer(id uint64) error
}

// NewService creates a customer service with necessary dependencies.
func NewService(store domain.CustomerStore) Service {
	return &service{
		store: store,
	}
}

type service struct {
	store domain.CustomerStore
}

func (s *service) GetCustomer(id uint64) (*domain.Customer, error) {
	return s.store.Find(id)
}

func (s *service) GetAllCustomers() ([]domain.Customer, error) {
	return s.store.FindAll()
}

func (s *service) CreateCustomer(firstname string, lastname string, email string, phone string) (*domain.Customer, error) {
	c := &domain.Customer{
		ID:        0,
		FirstName: firstname,
		LastName:  lastname,
		Email:     email,
		Phone:     phone,
	}
	return s.store.Create(c)
}

func (s *service) UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (*domain.Customer, error) {
	c := &domain.Customer{
		ID:        id,
		FirstName: firstname,
		LastName:  lastname,
		Email:     email,
		Phone:     phone,
	}
	return s.store.Update(c)
}

func (s *service) DeleteCustomer(id uint64) error {
	return s.store.Delete(id)
}
