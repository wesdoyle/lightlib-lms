# Project Variables
PROJECT_NAME ?= LightLib
ORG_NAME ?= LightLib
REPO_NAME ?= LightLib

# Filenames
DEV_COMPOSE_FILE := docker-compose.yml

.PHONY: test build release

test:
	echo "test stage"

build:
	echo "build stage"

release:
	echo "release stage"
