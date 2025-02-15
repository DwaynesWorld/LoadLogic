FROM golang:alpine

RUN apk update && apk add bash

# Set necessary environmet variables needed for our image
ENV GO111MODULE=on \
    CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=amd64

# Move to working directory /build
WORKDIR /build

# Copy go.mod and go.sum files to the workspace
COPY "locations/go.mod" . 
COPY "locations/go.sum" .

# Get dependancies
RUN go mod download

# Copy the code into the container
COPY "locations/" .

# Build the application
RUN go build 

# Move to /dist directory as the place for resulting binary folder
WORKDIR /dist

# Copy binary from build to main folder
RUN cp /build/locations .

# Copy seed data from build to main folder
RUN cp /build/locations.json .

# Export necessary port
EXPOSE 80

# Command to run when starting the container
ENTRYPOINT ["/dist/locations"]