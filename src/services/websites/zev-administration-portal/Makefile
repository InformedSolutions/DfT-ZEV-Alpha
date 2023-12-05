build:
	cd DfT.ZEV.Administration.Web && dotnet build

build-no-restore:
	cd DfT.ZEV.Administration.Web && dotnet build --no-restore

clean:
	cd DfT.ZEV.Administration.Web && dotnet clean

clean-build:
	cd DfT.ZEV.Administration.Web && clean build

docker-build:
	cd ../../../.. && docker build -f DfT.ZEV.Administration.Dockerfile -t zev-administration .

docker-push:
	docker tag zev-administration ${REGISTRY_URL}/zev-administration-portal:${SOURCE_COMMIT_HASH}
	docker push ${REGISTRY_URL}/zev-administration-portal:${SOURCE_COMMIT_HASH}
	docker tag zev-administration ${REGISTRY_URL}/zev-administration-portal:latest
	docker push ${REGISTRY_URL}/zev-administration-portal:latest

docker-run:
	docker run -p 5014:80 zev-administration-review

restore:
	cd DfT.ZEV.Administration.Web && dotnet restore

test-coverage:
	cd tests/DfT.ZEV.Administration.Tests && dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=\"cobertura,json\" /p:CoverletOutput=../CoverageResults/
