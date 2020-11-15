package application

import (
	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
)

// LocationsService is the interface that provides customer operations.
type LocationsService interface {
	GetLocation(id uint64) (*domain.Location, error)
	GetAllLocations() ([]domain.Location, error)
	CreateLocation(firstname string, lastname string, email string, phone string) (*domain.Location, error)
	UpdateLocation(id uint64, firstname string, lastname string, email string, phone string) (*domain.Location, error)
	DeleteLocation(id uint64) error
}

// NewService creates a customer service with necessary dependencies.
func NewService(store domain.LocationStore) LocationsService {
	return &customerService{
		store: store,
	}
}

type customerService struct {
	store domain.LocationStore
}

func (s *customerService) GetLocation(id uint64) (*domain.Location, error) {
	return s.store.Find(id)
}

func (s *customerService) GetAllLocations() ([]domain.Location, error) {
	return s.store.FindAll()
}

func (s *customerService) CreateLocation(firstname string, lastname string, email string, phone string) (*domain.Location, error) {
	c := &domain.Location{
		ID:        0,
		FirstName: firstname,
		LastName:  lastname,
		Email:     email,
		Phone:     phone,
	}
	return s.store.Create(c)
}

func (s *customerService) UpdateLocation(id uint64, firstname string, lastname string, email string, phone string) (*domain.Location, error) {
	c := &domain.Location{
		ID:        id,
		FirstName: firstname,
		LastName:  lastname,
		Email:     email,
		Phone:     phone,
	}
	return s.store.Update(c)
}

func (s *customerService) DeleteLocation(id uint64) error {
	return s.store.Delete(id)
}
