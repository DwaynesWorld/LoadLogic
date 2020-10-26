package db

import (
	"github.com/DwaynesWorld/LoadLogic/src/customers/models"
	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"
)

// New creates a new db reference
func New(dsn string) (*gorm.DB, error) {
	db, err := gorm.Open(sqlserver.Open(dsn), &gorm.Config{})
	if err != nil {
		return nil, err
	}

	return db, nil
}

// AutoMigrate runs auto migration for given models
func AutoMigrate(db *gorm.DB) error {
	return db.AutoMigrate(&models.Customer{})
}
