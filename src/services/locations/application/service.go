package application

import (
	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
)

// LocationsService is the interface that provides location operations.
type LocationsService interface {
	GetLocation(id uint64) (*domain.Location, error)
	GetAllLocations() ([]domain.Location, error)
	CreateLocation(name string, web string, contactFirstName string, contactLastName string, contactEmail string, phone1 string, phone2 string, address1 string, address2 string, city string, county string, state string, country string, zip string) (*domain.Location, error)
	UpdateLocation(id uint64, name string, web string, contactFirstName string, contactLastName string, contactEmail string, phone1 string, phone2 string, address1 string, address2 string, city string, county string, state string, country string, zip string) (*domain.Location, error)
	DeleteLocation(id uint64) error
}

// NewService creates a location service with necessary dependencies.
func NewService(store domain.LocationStore) LocationsService {
	return &locationService{
		store: store,
	}
}

type locationService struct {
	store domain.LocationStore
}

func (s *locationService) GetLocation(id uint64) (*domain.Location, error) {
	return s.store.Find(id)
}

func (s *locationService) GetAllLocations() ([]domain.Location, error) {
	return s.store.FindAll()
}

func (s *locationService) CreateLocation(name string, web string, contactFirstName string, contactLastName string, contactEmail string, phone1 string, phone2 string, address1 string, address2 string, city string, county string, state string, country string, zip string) (*domain.Location, error) {
	c := &domain.Location{
		ID:               0,
		Name:             name,
		Web:              web,
		Address1:         address1,
		Address2:         address2,
		City:             city,
		County:           county,
		State:            state,
		Country:          country,
		Zip:              zip,
		ContactFirstName: contactFirstName,
		ContactLastName:  contactLastName,
		ContactEmail:     contactEmail,
		Phone1:           phone1,
		Phone2:           phone2,
	}
	return s.store.Create(c)
}

func (s *locationService) UpdateLocation(id uint64, name string, web string, contactFirstName string, contactLastName string, contactEmail string, phone1 string, phone2 string, address1 string, address2 string, city string, county string, state string, country string, zip string) (*domain.Location, error) {
	c := &domain.Location{
		ID:               id,
		Name:             name,
		Web:              web,
		Address1:         address1,
		Address2:         address2,
		City:             city,
		County:           county,
		State:            state,
		Country:          country,
		Zip:              zip,
		ContactFirstName: contactFirstName,
		ContactLastName:  contactLastName,
		ContactEmail:     contactEmail,
		Phone1:           phone1,
		Phone2:           phone2,
	}
	return s.store.Update(c)
}

func (s *locationService) DeleteLocation(id uint64) error {
	return s.store.Delete(id)
}
