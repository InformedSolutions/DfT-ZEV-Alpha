# Define the root directory
ROOT_DIR ?= $(shell pwd)
export ROOT_DIR

build:
	cd DfT.ZEV.Core.Application && dotnet build

build-no-restore:
	cd DfT.ZEV.Core.Application && dotnet build --no-restore

clean:
	cd DfT.ZEV.Core.Application && dotnet clean

clean-build: clean build

restore:
	cd DfT.ZEV.Core.Application && dotnet restore

test-coverage:
	cd tests/DfT.ZEV.Core.Domain.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=$(ROOT_DIR)/CoverageResults/ /p:MergeWith="$(ROOT_DIR)/CoverageResults/coverage.json" -m:1
	cd tests/DfT.ZEV.Core.Infrastructure.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=$(ROOT_DIR)/CoverageResults/ /p:MergeWith="$(ROOT_DIR)/CoverageResults/coverage.json" -m:1
	cd tests/DfT.ZEV.Core.Application.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=$(ROOT_DIR)/CoverageResults/ /p:MergeWith="$(ROOT_DIR)/CoverageResults/coverage.json" -m:1
	cd tests/DfT.ZEV.Core.Architecture.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=$(ROOT_DIR)/CoverageResults/ /p:MergeWith="$(ROOT_DIR)/CoverageResults/coverage.json" -m:1