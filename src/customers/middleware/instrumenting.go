package middleware

import (
	"time"

	"github.com/DwaynesWorld/LoadLogic/src/customers/application"
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/go-kit/kit/metrics"
)

type instrumentingService struct {
	requestCount   metrics.Counter
	requestLatency metrics.Histogram
	application.CustomersService
}

// NewInstrumentingService returns a new instance of a instrumenting Service.
func NewInstrumentingService(counter metrics.Counter, latency metrics.Histogram, s application.CustomersService) application.CustomersService {
	return &instrumentingService{
		requestCount:     counter,
		requestLatency:   latency,
		CustomersService: s,
	}
}

func (s *instrumentingService) GetCustomer(id uint64) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "GetCustomer").Add(1)
		s.requestLatency.With("method", "GetCustomer").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.CustomersService.GetCustomer(id)
}

func (s *instrumentingService) GetAllCustomers() (c []domain.Customer, err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "GetAllCustomers").Add(1)
		s.requestLatency.With("method", "GetAllCustomers").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.CustomersService.GetAllCustomers()
}

func (s *instrumentingService) CreateCustomer(firstname string, lastname string, email string, phone string) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "CreateCustomer").Add(1)
		s.requestLatency.With("method", "CreateCustomer").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.CustomersService.CreateCustomer(firstname, lastname, email, phone)
}

func (s *instrumentingService) UpdateCustomer(id uint64, firstname string, lastname string, email string, phone string) (c *domain.Customer, err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "UpdateCustomer").Add(1)
		s.requestLatency.With("method", "UpdateCustomer").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.CustomersService.UpdateCustomer(id, firstname, lastname, email, phone)
}

func (s *instrumentingService) DeleteCustomer(id uint64) (err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "DeleteCustomer").Add(1)
		s.requestLatency.With("method", "DeleteCustomer").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.CustomersService.DeleteCustomer(id)
}
