package middleware

import (
	"time"

	"github.com/DwaynesWorld/LoadLogic/src/customers/application"
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	log "github.com/sirupsen/logrus"
)

type loggingService struct {
	logger *log.Entry
	application.CustomersService
}

// NewLoggingService returns a new instance of a logging Service.
func NewLoggingService(logger *log.Entry, s application.CustomersService) application.CustomersService {
	return &loggingService{logger, s}
}

func (s *loggingService) GetCustomer(id uint64) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method": "GetCustomer",
			"Id":     id,
			"Took":   time.Since(begin),
			"Error":  err,
		}).Info("Method GetCustomer invoked")
	}(time.Now())
	return s.CustomersService.GetCustomer(id)
}

func (s *loggingService) GetAllCustomers() (c []domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method": "GetAllCustomers",
			"Took":   time.Since(begin),
			"Error":  err,
		}).Info("Method GetAllCustomers invoked")
	}(time.Now())
	return s.CustomersService.GetAllCustomers()
}

func (s *loggingService) CreateCustomer(firstname string, lastname string, email string, phone string) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method":    "CreateCustomer",
			"Firstname": firstname,
			"Lastname":  lastname,
			"Email":     email,
			"Phone":     phone,
			"Took":      time.Since(begin),
			"Error":     err,
		}).Info("Method CreateCustomer invoked")
	}(time.Now())
	return s.CustomersService.CreateCustomer(firstname, lastname, email, phone)
}

func (s *loggingService) UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method":    "UpdateCustomer",
			"Id":        id,
			"Firstname": firstname,
			"lastname":  lastname,
			"Email":     email,
			"Phone":     phone,
			"Took":      time.Since(begin),
			"Error":     err,
		}).Info("Method UpdateCustomer invoked")
	}(time.Now())
	return s.CustomersService.UpdateCustomer(id, firstname, lastname, email, phone)
}

func (s *loggingService) DeleteCustomer(id uint64) (err error) {
	defer func(begin time.Time) {
		s.logger.WithFields(log.Fields{
			"Method": "DeleteCustomer",
			"Id":     id,
			"Took":   time.Since(begin),
			"Error":  err,
		}).Info("Method DeleteCustomer invoked")
	}(time.Now())
	return s.CustomersService.DeleteCustomer(id)
}
