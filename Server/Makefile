# Variables
SRC = server.ts
DIST = dist/server.js

# Targets
build:
	tsc

run: build
	node $(DIST)

clean:
	rm -rf dist

# Default target
all: run
