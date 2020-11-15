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

func (s *loggingService) CreateLocation(firstname string, lastname string, email string, phone string) (c *domain.Location, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method":    "CreateLocation",
			"Firstname": firstname,
			"Lastname":  lastname,
			"Email":     email,
			"Phone":     phone,
			"Took":      time.Since(begin),
			"Error":     err,
		}).Info("Method CreateLocation invoked")
	}(time.Now())
	return s.LocationsService.CreateLocation(firstname, lastname, email, phone)
}

func (s *loggingService) UpdateLocation(id uint64, firstname string, lastname string, email string, phone string) (c *domain.Location, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method":    "UpdateLocation",
			"Id":        id,
			"Firstname": firstname,
			"lastname":  lastname,
			"Email":     email,
			"Phone":     phone,
			"Took":      time.Since(begin),
			"Error":     err,
		}).Info("Method UpdateLocation invoked")
	}(time.Now())
	return s.LocationsService.UpdateLocation(id, firstname, lastname, email, phone)
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
