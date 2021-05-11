package main

import (
	"fmt"
	"net/http"
	neturl "net/url"
	"os"
	"os/signal"
	"strings"
	"syscall"

	kitprometheus "github.com/go-kit/kit/metrics/prometheus"
	"github.com/olivere/elastic/v7"
	"github.com/prometheus/client_golang/prometheus"
	"github.com/sirupsen/logrus"
	"gopkg.in/sohlich/elogrus.v7"
	"gorm.io/gorm"

	"github.com/DwaynesWorld/LoadLogic/src/locations/application"
	"github.com/DwaynesWorld/LoadLogic/src/locations/domain"
	"github.com/DwaynesWorld/LoadLogic/src/locations/middleware"
	"github.com/DwaynesWorld/LoadLogic/src/locations/persistence"
)

const (
	applicationContext = "LoadLogic.Services.Locations"
	defaultHost        = "localhost"
	defaultPort        = "8080"
	defaultDsn         = "sqlserver://sa:Pass@word@localhost:1433?database=LoadLogic_LocationsDB"
	defaultES          = "http://localhost:9200"
)

func main() {
	logger, httpLogger := setupLoggers()
	db := setupPersistence(logger)
	setupLocationService(db, logger, httpLogger)
	startServer(logger)
}

func startServer(logger *logrus.Entry) {
	host := envString("HOST", defaultHost, true)
	port := envString("PORT", defaultPort, true)
	addr := fmt.Sprintf("%s:%s", host, port)

	errs := make(chan error, 2)

	go func() {
		logger.WithFields(logrus.Fields{
			"Transport":    "http",
			"Host":         host,
			"Port":         port,
			"Address":      addr,
			"EventContext": "listening",
		}).Info(fmt.Sprintf("Server listening started at %s", addr))
		errs <- http.ListenAndServe(addr, nil)
	}()

	go func() {
		c := make(chan os.Signal)
		signal.Notify(c, syscall.SIGINT)
		errs <- fmt.Errorf("%s", <-c)
	}()

	logger.Info("Server terminated.", <-errs)
}

func setupLoggers() (*logrus.Entry, *logrus.Entry) {
	url := envString("ES_SERVER_URL", defaultES, false)

	base := logrus.New()
	client, err := elastic.NewClient(elastic.SetURL(url))
	if err != nil {
		logrus.Panic(err)
	}

	uri, err := neturl.Parse(url)
	if err != nil {
		logrus.Panic(err)
	}

	hook, err := elogrus.NewAsyncElasticHook(client, uri.Host, logrus.DebugLevel, "logs-locations")
	if err != nil {
		logrus.Panic(err)
	}

	base.AddHook(hook)
	base.SetFormatter(&logrus.TextFormatter{
		DisableColors: false,
		FullTimestamp: true,
	})

	logger := base.WithField("ApplicationContext", applicationContext)
	httpLogger := logger.WithField("SourceContext", "http")

	return logger, httpLogger
}

func setupPersistence(logger *logrus.Entry) *gorm.DB {
	dsn := envString("DATABASE_CONNECTION_STRING", defaultDsn, true)

	db, err := persistence.NewSQL(dsn)
	if err != nil {
		logger.Fatal("An error occurred connecting to the database.", err)
	}

	logger.Info("Attempting database migrations...")
	err = persistence.AutoMigrate(db)

	if err != nil {
		logger.Fatal("An error occurred migrating the database.", err)
	} else {
		logger.Info("Attempted database migration executed successfully.")
	}

	err = persistence.Seed(db)
	if err != nil {
		logger.Fatal("An error occurred seeding the database.", err)
	} else {
		logger.Info("Attempted database seeding executed successfully.")
	}

	return db
}

func setupLocationService(db *gorm.DB, logger *logrus.Entry, httpLogger *logrus.Entry) {
	serviceLogger := logger.WithField("SourceContext", "LoggingService")
	var cs application.LocationsService
	cs = application.NewService(domain.NewLocationStore(db))
	cs = middleware.NewLoggingService(serviceLogger, cs)
	cs = middleware.NewInstrumentingService(
		kitprometheus.NewCounterFrom(prometheus.CounterOpts{
			Namespace: "api",
			Subsystem: "locations_service",
			Name:      "request_count",
			Help:      "Number of requests received.",
		}, []string{"method"}),
		kitprometheus.NewSummaryFrom(prometheus.SummaryOpts{
			Namespace: "api",
			Subsystem: "locations_service",
			Name:      "request_latency_microseconds",
			Help:      "Total duration of requests in microseconds.",
		}, []string{"method"}),
		cs,
	)

	endpoints := middleware.NewEndpoints(cs)
	middleware.NewHTTPTransport(endpoints, httpLogger)
}

func envString(env, fallback string, log bool) string {
	var e string
	e = strings.TrimSpace(os.Getenv(env))

	if e == "" {
		e = fallback
	}

	if log {
		fmt.Println(env, e)
	}

	return e
}
