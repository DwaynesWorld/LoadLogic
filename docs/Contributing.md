# Contributing

Each bounded context in this application is divided into a microservice consiting of an api. It attempts to follow DDD (Domain Driven Design). For .NET projects it also follows the [Microsoft Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/).

### Domain Layer

Responsible for representing concepts of the business, information about the business situation and business rules. State that reflects the business situation is controlled and used here, even though the technical details of storing it are delegated to the infrastructure. This layer is the heart of business software.

### Application Layer

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure Layer

The infrastructure layer is how the data that is initially held in domain entities (in memory) is persisted in databases or another persistent store. An example is using Entity Framework Core code to implement the Repository pattern classes that use a DBContext to persist data in a relational database. It also handles the implementation of any external services.

### API Layer

This layer is kept thin, it does not contain business rules or knowledge, but only coordinates tasks and delegates work to collaborations of domain objects in the next layer down. It does not have state reflecting the business situation, but it can have state that reflects the progress of a task for the user or the program.

## Development Environment Requirements

```
.Net Standard 2.1
.Net Core 5.0
Language is C# 8.0
Golang 1.15.4
Docker
protoc 3.13.0
yarn
```

## Getting Started

`Checkout repo`

```
git clone
cd into src
```

`Start Services`

```
docker-compose up --build
```
