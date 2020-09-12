package main

import (
	"fmt"
	"log"
	"os"
	"strings"

	"github.com/labstack/echo"
	"github.com/labstack/echo/middleware"
)

const defaultHost = "localhost"
const defaultPort = "8080"
const defaultDsn = "sqlserver://sa:Pass@word@localhost:5433?database=LoadLogic.Customers"

// Global DB context reference
var db *Context

func main() {
	dsn := strings.TrimSpace(os.Getenv("DatabaseConnection"))
	if dsn == "" {
		dsn = defaultDsn
	}

	fmt.Println("DatabaseConnection:", dsn)

	var err error
	db, err = NewContext(dsn)

	if err != nil {
		log.Fatal(err)
	}

	db.AutoMigrate()

	e := echo.New()

	e.Use(middleware.Logger())
	e.Use(middleware.Recover())

	e.GET("/customers", GetCustomers)
	e.GET("/customers/:id", GetCustomer)
	e.POST("/customers", AddCustomer)
	e.PUT("/customers/:id", UpdateCustomer)
	e.DELETE("/customers/:id", DeleteCustomer)

	host := strings.TrimSpace(os.Getenv("HOST"))
	if host == "" {
		host = defaultHost
	}

	port := strings.TrimSpace(os.Getenv("PORT"))
	if port == "" {
		port = defaultPort
	}

	addr := fmt.Sprintf("%s:%s", host, port)

	fmt.Println(fmt.Sprintf("The server is running at http://%s.", addr))
	e.Logger.Fatal(e.Start(addr))
}
