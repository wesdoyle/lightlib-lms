# Project Variables
PROJECT_NAME ?= LightLib
ORG_NAME ?= Riverrun

rootdir = $(realpath .)
.PHONY: migrations db build release

migrations:
	- cd ./LightLib.Data \
	    && dotnet ef --startup-project ../LightLib.Web/ migrations add $(mname) \
	    && cd ..

db:
	- cd ./LightLib.Data \
	    && dotnet ef --startup-project ../LightLib.Web/ database update \
	    && cd ..

build:
	- dotnet clean && dotnet build

run-sonar:
	- sonar-scanner

release:
	- dotnet publish ./LightLib.sln -c Release -o release
