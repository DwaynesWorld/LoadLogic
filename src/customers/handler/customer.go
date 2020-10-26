package handler

import (
	"fmt"
	"net/http"
	"strconv"

	"github.com/DwaynesWorld/LoadLogic/src/customers/models"
	"github.com/labstack/echo"
)

// GetCustomers returns all customers
func (h *Handler) GetCustomers(c echo.Context) error {
	customers, err := h.customerStore.GetAllCustomers()
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, err.Error())
	}

	return c.JSON(http.StatusOK, customers)
}

// GetCustomer returns a customer with the matching id
func (h *Handler) GetCustomer(c echo.Context) error {
	id, err := strconv.ParseUint(c.Param("id"), 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid customer ID, it must be a uuid", c.Param("id"))
		return echo.NewHTTPError(http.StatusBadRequest, msg)
	}

	customer, err := h.customerStore.GetCustomer(id)
	if err != nil {
		return echo.NewHTTPError(http.StatusNotFound, err.Error())
	}

	return c.JSON(http.StatusOK, customer)
}

// AddCustomer adds a new customer to the database
func (h *Handler) AddCustomer(c echo.Context) error {
	customer := &models.Customer{}

	if err := c.Bind(customer); err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, err.Error())
	}

	customer, err := h.customerStore.AddCustomer(customer)
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, err.Error())
	}

	return c.JSON(http.StatusCreated, customer)
}

// UpdateCustomer updates an existing customer in the database
func (h *Handler) UpdateCustomer(c echo.Context) error {
	customer := &models.Customer{}
	if err := c.Bind(customer); err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, err.Error())
	}

	customer, err := h.customerStore.UpdateCustomer(customer)
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, err.Error())
	}

	return c.JSON(http.StatusCreated, customer)
}

// DeleteCustomer deletes the customer with the matching id
func (h *Handler) DeleteCustomer(c echo.Context) error {
	id, err := strconv.ParseUint(c.Param("id"), 0, 0)
	if err != nil {
		msg := fmt.Sprintf("%s is not a valid customer ID, it must be a uuid", c.Param("id"))
		return echo.NewHTTPError(http.StatusBadRequest, msg)
	}

	err = h.customerStore.DeleteCustomer(id)
	if err != nil {
		return echo.NewHTTPError(http.StatusBadRequest, err.Error())
	}

	return c.NoContent(http.StatusOK)
}
