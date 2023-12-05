build:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet build	

build-no-restore:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet build --no-restore

build-test: build test-coverage

clean:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet clean

clean-build:
	cd DfT.ZEV.ManufacturerReview.Web && clean build

docker-build:
	cd ../../../.. && docker build -f DfT.ZEV.ManufacturerReview.Dockerfile -t zev-manufacturer-portal .

docker-push:
	docker tag zev-manufacturer-portal ${REGISTRY_URL}/zev-manufacturer-portal:${SOURCE_COMMIT_HASH}
	docker push ${REGISTRY_URL}/zev-manufacturer-portal:${SOURCE_COMMIT_HASH}
	docker tag zev-manufacturer-portal ${REGISTRY_URL}/zev-manufacturer-portal:latest
	docker push ${REGISTRY_URL}/zev-manufacturer-portal:latest

docker-run:
	docker run -p 5014:80 zev-manufacturer-review

restore:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet restore

test-coverage:
	cd tests/DfT.ZEV.ManufacturerReview.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=../CoverageResults/