package domain

// Location entity.
type Location struct {
	ID               uint64 `gorm:"primaryKey;autoIncrement;not null" json:"id"`
	Name             string `gorm:"size:100;not null;"                json:"name"`
	Web              string `gorm:"size:100;not null;"                json:"web"`
	ContactFirstName string `gorm:"size:100;not null;"                json:"contact_first_name"`
	ContactLastName  string `gorm:"size:100;not null;"                json:"contact_last_name"`
	ContactEmail     string `gorm:"size:100;not null;"                json:"contact_email"`
	Phone1           string `gorm:"size:20;not null;"                 json:"phone1"`
	Phone2           string `gorm:"size:20;not null;"                 json:"phone2"`
	Address1         string `gorm:"size:100;not null;"                json:"address1"`
	Address2         string `gorm:"size:100;not null;"                json:"address2"`
	City             string `gorm:"size:50;not null;"                 json:"city"`
	County           string `gorm:"size:100;not null;"                json:"county"`
	State            string `gorm:"size:10;not null;"                 json:"state"`
	Country          string `gorm:"size:40;not null;"                 json:"country"`
	Zip              string `gorm:"size:20;not null;"                 json:"zip"`
}
