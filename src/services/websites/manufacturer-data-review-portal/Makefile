build:
	cd src/DfT.ZEV.ManufacturerReview.Web && dotnet build	

build-no-restore:
	cd src/DfT.ZEV.ManufacturerReview.Web && dotnet build --no-restore

clean:
	cd src/DfT.ZEV.ManufacturerReview.Web && dotnet clean

clean-build:
	cd src/DfT.ZEV.ManufacturerReview.Web && clean build

docker-build:
	cd ../.. && docker build -f DfT.ZEV.ManufacturerReview.Website.Dockerfile -t template .

docker-run:
	docker run -p 5014:80 template

restore:
	cd src/DfT.ZEV.ManufacturerReview.Web && dotnet restore

test:
	cd tests/DfT.ZEV.ManufacturerReview.Tests && dotnet test /p:CollectCoverage=true

test-coverage-threshold:
	cd tests/DfT.ZEV.ManufacturerReview.Tests && dotnet test /p:CollectCoverage=true

update-dependencies:
	cd src/DfT.ZEV.ManufacturerReview.Web && dotnet outdated --version-lock major --upgrade
