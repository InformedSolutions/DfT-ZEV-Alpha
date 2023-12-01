build:
	dotnet build	

build-no-restore:
	dotnet build --no-restore

build-test: build test-coverage

clean:
	dotnet clean

clean-build:
	clean build

restore:
	dotnet restore

test-coverage:
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=../CoverageResults/