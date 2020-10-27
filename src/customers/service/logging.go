package service

import (
	"time"

	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/go-kit/kit/log"
)

type loggingService struct {
	logger log.Logger
	Service
}

// NewLoggingService returns a new instance of a logging Service.
func NewLoggingService(logger log.Logger, s Service) Service {
	return &loggingService{logger, s}
}

func (s *loggingService) GetCustomer(id uint64) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.Log(
			"method", "GetCustomer",
			"id", id,
			"took", time.Since(begin),
			"err", err,
		)
	}(time.Now())
	return s.Service.GetCustomer(id)
}

func (s *loggingService) GetAllCustomers() (c []domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.Log(
			"method", "GetAllCustomers",
			"took", time.Since(begin),
			"err", err,
		)
	}(time.Now())
	return s.Service.GetAllCustomers()
}

func (s *loggingService) CreateCustomer(firstname string, lastname string, email string, phone string) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.Log(
			"method", "CreateCustomer",
			"firstname", firstname,
			"lastname", lastname,
			"email", email,
			"phone", phone,
			"took", time.Since(begin),
			"err", err,
		)
	}(time.Now())
	return s.Service.CreateCustomer(firstname, lastname, email, phone)
}

func (s *loggingService) UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.logger.Log(
			"method", "UpdateCustomer",
			"id", id,
			"firstname", firstname,
			"lastname", lastname,
			"email", email,
			"phone", phone,
			"took", time.Since(begin),
			"err", err,
		)
	}(time.Now())
	return s.Service.UpdateCustomer(id, firstname, lastname, email, phone)
}

func (s *loggingService) DeleteCustomer(id uint64) (err error) {
	defer func(begin time.Time) {
		s.logger.Log(
			"method", "DeleteCustomer",
			"id", id,
			"took", time.Since(begin),
			"err", err,
		)
	}(time.Now())
	return s.Service.DeleteCustomer(id)
}
