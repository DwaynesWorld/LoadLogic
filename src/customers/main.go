package main

import (
	"fmt"
	"net/http"
	"os"
	"os/signal"
	"strings"
	"syscall"

	kitprometheus "github.com/go-kit/kit/metrics/prometheus"
	"github.com/nullseed/logruseq"
	"github.com/prometheus/client_golang/prometheus"
	log "github.com/sirupsen/logrus"
	"gorm.io/gorm"

	"github.com/DwaynesWorld/LoadLogic/src/customers/application"
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/DwaynesWorld/LoadLogic/src/customers/middleware"
	"github.com/DwaynesWorld/LoadLogic/src/customers/persistence"
)

const (
	applicationContext = "LoadLogic.Services.Customers"
	defaultHost        = "localhost"
	defaultPort        = "8080"
	defaultDsn         = "sqlserver://sa:Pass@word@localhost:1433?database=LoadLogic_CustomersDB"
	defaultSeq         = "http://seq"
)

func main() {
	logger, httpLogger := setupLoggers()
	db := setupPersistence(logger)
	setupCustomerService(db, logger, httpLogger)
	startServer(logger)
}

func startServer(logger *log.Entry) {
	host := envString("HOST", defaultHost, true)
	port := envString("PORT", defaultPort, true)
	addr := fmt.Sprintf("%s:%s", host, port)

	errs := make(chan error, 2)

	go func() {
		logger.WithFields(log.Fields{
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

func setupLoggers() (*log.Entry, *log.Entry) {
	seq := envString("SEQ_SERVER_URL", defaultSeq, false)

	base := log.New()
	base.AddHook(logruseq.NewSeqHook(seq))
	base.SetFormatter(&log.TextFormatter{
		DisableColors: false,
		FullTimestamp: true,
	})

	logger := base.WithField("ApplicationContext", applicationContext)
	httpLogger := logger.WithField("SourceContext", "http")

	return logger, httpLogger
}

func setupPersistence(logger *log.Entry) *gorm.DB {
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

	return db
}

func setupCustomerService(db *gorm.DB, logger *log.Entry, httpLogger *log.Entry) {
	serviceLogger := logger.WithField("SourceContext", "LoggingService")
	var cs application.CustomersService
	cs = application.NewService(domain.NewCustomerStore(db))
	cs = middleware.NewLoggingService(serviceLogger, cs)
	cs = middleware.NewInstrumentingService(
		kitprometheus.NewCounterFrom(prometheus.CounterOpts{
			Namespace: "api",
			Subsystem: "customers_service",
			Name:      "request_count",
			Help:      "Number of requests received.",
		}, []string{"method"}),
		kitprometheus.NewSummaryFrom(prometheus.SummaryOpts{
			Namespace: "api",
			Subsystem: "customers_service",
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
