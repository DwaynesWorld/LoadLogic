package main

import (
	"encoding/json"
	"fmt"
	"net/http"
	"strconv"

	"github.com/julienschmidt/httprouter"
)

// GetCustomers returns all customers
func GetCustomers(w http.ResponseWriter, r *http.Request, _ httprouter.Params) {
	w.Header().Set("Content-Type", "application/json")
	customers, err := db.Customer.GetAllCustomers()
	if err != nil {
		http.Error(w, err.Error(), http.StatusBadRequest)
		return
	}

	json.NewEncoder(w).Encode(customers)
	return
}

// GetCustomer returns a customer with the matching id
func GetCustomer(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	id, err := strconv.ParseUint(ps.ByName("id"), 0, 0)
	if err != nil {
		http.Error(w, fmt.Sprintf("%s is not a valid customer ID, it must be a number", ps.ByName("id")), http.StatusBadRequest)
		return
	}

	customer, err := db.Customer.GetCustomer(id)
	if err != nil {
		http.Error(w, err.Error(), http.StatusNotFound)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(customer)
	return
}

// AddCustomer adds a new customer to the database
func AddCustomer(w http.ResponseWriter, r *http.Request, _ httprouter.Params) {
	customer := &Customer{
		ID:        1,
		FirstName: "Joe",
		LastName:  "Dirt",
		Email:     "joe.dirt@example.com",
		Phone:     "2813308004",
	}

	customer, err := db.Customer.AddCustomer(customer)
	if err != nil {
		http.Error(w, err.Error(), http.StatusNotFound)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(customer)
	return
}

// UpdateCustomer updates an existing customer in the database
func UpdateCustomer(w http.ResponseWriter, r *http.Request, _ httprouter.Params) {
	customer := &Customer{
		ID:        1,
		FirstName: "Joe",
		LastName:  "Dumars",
		Email:     "joe.dumars@example.com",
		Phone:     "2813308004",
	}

	customer, err := db.Customer.UpdateCustomer(customer)
	if err != nil {
		http.Error(w, err.Error(), http.StatusNotFound)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	json.NewEncoder(w).Encode(customer)
	return
}

// DeleteCustomer deletes the customer with the matching id
func DeleteCustomer(w http.ResponseWriter, r *http.Request, ps httprouter.Params) {
	id, err := strconv.ParseUint(ps.ByName("id"), 0, 0)
	if err != nil {
		http.Error(w, fmt.Sprintf("%s is not a valid customer ID, it must be a number", ps.ByName("id")), http.StatusBadRequest)
		return
	}

	err = db.Customer.DeleteCustomer(id)
	if err != nil {
		http.Error(w, err.Error(), http.StatusNotFound)
		return
	}

	return
}
