package persistence

import (
	"fmt"
	"strings"
	"time"

	"gorm.io/driver/sqlite"
	"gorm.io/driver/sqlserver"
	"gorm.io/gorm"

	"github.com/DwaynesWorld/LoadLogic/src/customers/domain"
)

// NewSQL creates a new sqlserver db reference
func NewSQL(dsn string) (*gorm.DB, error) {
	var db *gorm.DB
	var err error

	retries := 1

	for retries < 5 {
		db, err = gorm.Open(sqlserver.Open(dsn), &gorm.Config{})

		if err != nil {
			if strings.Contains(err.Error(), "Unable to open tcp connection") {
				fmt.Println("attempting to retry database connection")
				time.Sleep(time.Duration(500*retries) * time.Millisecond)
				retries++
			} else {
				return nil, err
			}
		}

		if db != nil {
			return db, nil
		}
	}

	return nil, err
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
