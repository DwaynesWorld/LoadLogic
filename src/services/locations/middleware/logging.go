package middleware

import (
	"time"

	"github.com/DwaynesWorld/LoadLogic/src/locations/application"
	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
	log "github.com/sirupsen/logrus"
)

type loggingService struct {
	logger *log.Entry
	application.LocationsService
}

// NewLoggingService returns a new instance of a logging Service.
func NewLoggingService(logger *log.Entry, s application.LocationsService) application.LocationsService {
	return &loggingService{logger, s}
}

func (s *loggingService) GetLocation(id uint64) (c *domain.Location, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method": "GetLocation",
			"Id":     id,
			"Took":   time.Since(begin),
			"Error":  err,
		}).Info("Method GetLocation invoked")
	}(time.Now())

	return s.LocationsService.GetLocation(id)
}

func (s *loggingService) GetAllLocations() (c []domain.Location, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method": "GetAllLocations",
			"Took":   time.Since(begin),
			"Error":  err,
		}).Info("Method GetAllLocations invoked")
	}(time.Now())

	return s.LocationsService.GetAllLocations()
}

func (s *loggingService) CreateLocation(
	name string, web string, contactFirstName string, contactLastName string,
	contactEmail string, phone1 string, phone2 string, address1 string, address2 string,
	city string, county string, state string, country string, zip string) (c *domain.Location, err error) {

	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method":           "CreateLocation",
			"Name":             name,
			"Web":              web,
			"Address1":         address1,
			"Address2":         address2,
			"City":             city,
			"County":           county,
			"State":            state,
			"Country":          country,
			"Zip":              zip,
			"ContactFirstName": contactFirstName,
			"ContactLastName":  contactLastName,
			"ContactEmail":     contactEmail,
			"Phone1":           phone1,
			"Phone2":           phone2,
			"Took":             time.Since(begin),
			"Error":            err,
		}).Info("Method CreateLocation invoked")
	}(time.Now())

	return s.LocationsService.CreateLocation(
		name, web, contactFirstName, contactLastName, contactEmail, phone1,
		phone2, address1, address2, city, county, state, country, zip)
}

func (s *loggingService) UpdateLocation(
	id uint64, name string, web string, contactFirstName string, contactLastName string,
	contactEmail string, phone1 string, phone2 string, address1 string, address2 string,
	city string, county string, state string, country string, zip string) (c *domain.Location, err error) {

	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method":           "UpdateLocation",
			"Id":               id,
			"Name":             name,
			"Web":              web,
			"Address1":         address1,
			"Address2":         address2,
			"City":             city,
			"County":           county,
			"State":            state,
			"Country":          country,
			"Zip":              zip,
			"ContactFirstName": contactFirstName,
			"ContactLastName":  contactLastName,
			"ContactEmail":     contactEmail,
			"Phone1":           phone1,
			"Phone2":           phone2,
			"Took":             time.Since(begin),
			"Error":            err,
		}).Info("Method UpdateLocation invoked")
	}(time.Now())

	return s.LocationsService.UpdateLocation(
		id, name, web, contactFirstName, contactLastName, contactEmail, phone1,
		phone2, address1, address2, city, county, state, country, zip)
}

func (s *loggingService) DeleteLocation(id uint64) (err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method": "DeleteLocation",
			"Id":     id,
			"Took":   time.Since(begin),
			"Error":  err,
		}).Info("Method DeleteLocation invoked")
	}(time.Now())

	return s.LocationsService.DeleteLocation(id)
}
