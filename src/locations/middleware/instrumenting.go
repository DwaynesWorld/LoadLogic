package middleware

import (
	"time"

	"github.com/DwaynesWorld/LoadLogic/src/locations/application"
	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
	"github.com/go-kit/kit/metrics"
)

type instrumentingService struct {
	requestCount   metrics.Counter
	requestLatency metrics.Histogram
	application.LocationsService
}

// NewInstrumentingService returns a new instance of a instrumenting Service.
func NewInstrumentingService(counter metrics.Counter, latency metrics.Histogram, s application.LocationsService) application.LocationsService {
	return &instrumentingService{
		requestCount:     counter,
		requestLatency:   latency,
		LocationsService: s,
	}
}

func (s *instrumentingService) GetLocation(id uint64) (c *domain.Location, err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "GetLocation").Add(1)
		s.requestLatency.With("method", "GetLocation").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.LocationsService.GetLocation(id)
}

func (s *instrumentingService) GetAllLocations() (c []domain.Location, err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "GetAllLocations").Add(1)
		s.requestLatency.With("method", "GetAllLocations").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.LocationsService.GetAllLocations()
}

func (s *instrumentingService) CreateLocation(
	name string, web string, contactFirstName string, contactLastName string,
	contactEmail string, phone1 string, phone2 string, address1 string, address2 string,
	city string, county string, state string, country string, zip string) (c *domain.Location, err error) {

	defer func(begin time.Time) {
		s.requestCount.With("method", "CreateLocation").Add(1)
		s.requestLatency.With("method", "CreateLocation").Observe(time.Since(begin).Seconds())
	}(time.Now())

	return s.LocationsService.CreateLocation(
		name, web, contactFirstName, contactLastName, contactEmail, phone1,
		phone2, address1, address2, city, county, state, country, zip)
}

func (s *instrumentingService) UpdateLocation(
	id uint64, name string, web string, contactFirstName string, contactLastName string,
	contactEmail string, phone1 string, phone2 string, address1 string, address2 string,
	city string, county string, state string, country string, zip string) (c *domain.Location, err error) {

	defer func(begin time.Time) {
		s.requestCount.With("method", "UpdateLocation").Add(1)
		s.requestLatency.With("method", "UpdateLocation").Observe(time.Since(begin).Seconds())
	}(time.Now())

	return s.LocationsService.UpdateLocation(
		id, name, web, contactFirstName, contactLastName, contactEmail, phone1,
		phone2, address1, address2, city, county, state, country, zip)
}

func (s *instrumentingService) DeleteLocation(id uint64) (err error) {
	defer func(begin time.Time) {
		s.requestCount.With("method", "DeleteLocation").Add(1)
		s.requestLatency.With("method", "DeleteLocation").Observe(time.Since(begin).Seconds())
	}(time.Now())
	return s.LocationsService.DeleteLocation(id)
}
