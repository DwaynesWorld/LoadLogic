package persistence

import (
	"gorm.io/driver/sqlite"
	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"

	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
)

// NewSQL creates a new sqlserver db reference
func NewSQL(dsn string) (*gorm.DB, error) {
	db, err := gorm.Open(sqlserver.Open(dsn), &gorm.Config{})
	if err != nil {
		return nil, err
	}

	return db, nil
}

// NewInMem creates a new inmemory sqlite db reference
func NewInMem() (*gorm.DB, error) {
	db, err := gorm.Open(sqlite.Open("file::memory:?cache=shared"), &gorm.Config{})
	if err != nil {
		return nil, err
	}

	return db, nil
}

// AutoMigrate runs auto migration for given models
func AutoMigrate(db *gorm.DB) error {
	return db.AutoMigrate(&domain.Customer{})
}
