# .NET application
build:
	dotnet restore src/Zev.Services.ComplianceCalculationService.Handler && dotnet build src/Zev.Services.ComplianceCalculationService.Handler

test:
	dotnet test tests/Zev.Services.ComplianceCalculationService.Tests

test-coverage:
	dotnet test tests/Zev.Services.ComplianceCalculationService.Tests /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=../CoverageResults/ /p:MergeWith="../CoverageResults/coverage.json" -m:1

build-test: build test