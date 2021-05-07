package domain

// Customer entity.
type Customer struct {
	ID        uint64 `gorm:"primaryKey;autoIncrement;not null"  json:"id"`
	FirstName string `gorm:"size:100;not null;"                 json:"first_name"`
	LastName  string `gorm:"size:100;not null;"                 json:"last_name"`
	Email     string `gorm:"size:100;not null;"                 json:"email"`
	Phone     string `gorm:"size:20;not null;"                  json:"phone"`
}
