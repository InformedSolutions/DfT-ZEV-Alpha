build:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet build	

build-no-restore:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet build --no-restore

clean:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet clean

clean-build:
	cd DfT.ZEV.ManufacturerReview.Web && clean build

docker-build:
	cd ../../../.. && docker build -f DfT.ZEV.ManufacturerReview.Dockerfile -t zev-manufacturer-review .

docker-run:
	docker run -p 5014:80 zev-manufacturer-review

restore:
	cd DfT.ZEV.ManufacturerReview.Web && dotnet restore

test-coverage:
	cd tests/DfT.ZEV.ManufacturerReview.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=../CoverageResults/