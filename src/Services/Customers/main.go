package main

import (
	"fmt"
	"log"
	"net/http"

	"github.com/julienschmidt/httprouter"
)

// ServerAddr defines the http host and port of the beer server
const ServerAddr = "localhost:8080"

// dsn defines the MSSQL Server connection string
const dsn = "sqlserver://sa:Pass@word@sqlserver?database=LoadLogic.Customers"

var db *Context
var router *httprouter.Router

func init() {

	config := DBConfig{
		Scheme:       "sqlserver",
		Hostname:     "localhost",
		Port:         5433,
		DatabaseName: "LoadLogic.Customers",
		Username:     "sa",
		Password:     "Pass@word",
	}

	var err error
	db, err = NewContext(&config)

	if err != nil {
		log.Fatal(err)
	}

	db.AutoMigrate()

	router = httprouter.New()

	router.GET("/customers", GetCustomers)
	router.GET("/customers/:id", GetCustomer)

	router.POST("/customers", AddCustomer)
	router.POST("/customers/:id", UpdateCustomer)
	router.DELETE("/customers/:id", DeleteCustomer)
}

func main() {
	fmt.Println("The server is running at http://localhost:8080.")
	log.Fatal(http.ListenAndServe(ServerAddr, router))
}
