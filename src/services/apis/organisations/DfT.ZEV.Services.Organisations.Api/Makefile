build:
	dotnet build

build-no-restore:
	dotnet build --no-restore

build-test: build test-coverage

clean:
	dotnet clean

clean-build:
	clean build

docker-build:
	cd ../../../.. && docker build -f DfT.ZEV.Organisations.API.Dockerfile -t zev-organisation-api .

docker-push:
	docker tag zev-organisation-api ${REGISTRY_URL}/zev-organisation-api:${SOURCE_COMMIT_HASH}
	docker push ${REGISTRY_URL}/zev-organisation-api:${SOURCE_COMMIT_HASH}
	docker tag zev-administration ${REGISTRY_URL}/zev-organisation-api:latest
	docker push ${REGISTRY_URL}/zev-organisation-api:latest

restore:
	dotnet restore

test-coverage:
	dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=../CoverageResults/
