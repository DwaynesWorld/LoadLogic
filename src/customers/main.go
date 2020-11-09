package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
	"os/signal"
	"strings"
	"syscall"

	kitlog "github.com/go-kit/kit/log"
	kitprometheus "github.com/go-kit/kit/metrics/prometheus"
	stdprometheus "github.com/prometheus/client_golang/prometheus"

	"github.com/DwaynesWorld/LoadLogic/src/customers/application"
	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
	"github.com/DwaynesWorld/LoadLogic/src/customers/middleware"
	"github.com/DwaynesWorld/LoadLogic/src/customers/persistence"
	"github.com/prometheus/client_golang/prometheus/promhttp"
)

const defaultHost = "localhost"
const defaultPort = "8080"
const defaultDsn = "sqlserver://sa:Pass@word@localhost:1433?database=LoadLogic_CustomersDB"

func main() {
	dsn := envString("DatabaseConnection", defaultDsn, true)
	host := envString("HOST", defaultHost, true)
	port := envString("PORT", defaultPort, true)
	addr := fmt.Sprintf("%s:%s", host, port)

	var logger kitlog.Logger
	logger = kitlog.NewLogfmtLogger(kitlog.NewSyncWriter(os.Stderr))
	logger = kitlog.With(logger, "ts", kitlog.DefaultTimestampUTC)
	httpLogger := kitlog.With(logger, "component", "http")

	db, err := persistence.NewSQL(dsn)
	if err != nil {
		log.Fatal(err)
	}

	persistence.AutoMigrate(db)

	fieldKeys := []string{"method"}

	var cs application.CustomersService
	cs = application.NewService(domain.NewCustomerStore(db))
	cs = middleware.NewLoggingService(kitlog.With(logger, "component", "customers"), cs)
	cs = middleware.NewInstrumentingService(
		kitprometheus.NewCounterFrom(stdprometheus.CounterOpts{
			Namespace: "api",
			Subsystem: "customers_service",
			Name:      "request_count",
			Help:      "Number of requests received.",
		}, fieldKeys),
		kitprometheus.NewSummaryFrom(stdprometheus.SummaryOpts{
			Namespace: "api",
			Subsystem: "customers_service",
			Name:      "request_latency_microseconds",
			Help:      "Total duration of requests in microseconds.",
		}, fieldKeys),
		cs,
	)

	mux := http.NewServeMux()
	mux.Handle("/v1/", middleware.MakeHandler(cs, httpLogger))

	http.Handle("/", cors(mux))
	http.Handle("/metrics", promhttp.Handler())
	startServer(addr, logger)
}

func startServer(addr string, logger kitlog.Logger) {
	errs := make(chan error, 2)

	go func() {
		logger.Log("transport", "http", "address", addr, "msg", "listening")
		errs <- http.ListenAndServe(addr, nil)
	}()

	go func() {
		c := make(chan os.Signal)
		signal.Notify(c, syscall.SIGINT)
		errs <- fmt.Errorf("%s", <-c)
	}()

	logger.Log("Server terminated.", <-errs)
}

func cors(h http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Access-Control-Allow-Origin", "*")
		w.Header().Set("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS")
		w.Header().Set("Access-Control-Allow-Headers", "Origin, Content-Type")

		if r.Method == "OPTIONS" {
			return
		}

		h.ServeHTTP(w, r)
	})
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
