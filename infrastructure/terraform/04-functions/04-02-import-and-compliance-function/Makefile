apply:
	terraform apply -var-file="../../configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars"

apply-auto-approve:
	terraform apply -var-file="../../configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars" --auto-approve

format:
	terraform fmt -recursive

validate:
	terraform validate

init:
	terraform init -backend-config "../../configuration/partials/${DEPLOY_ENVIRONMENT}.tfbackend"
	terraform workspace select -or-create ${DEPLOY_ENVIRONMENT}

init-upgrade:
	rm -rf .terraform
	terraform init -upgrade -backend-config "../../configuration/partials/${DEPLOY_ENVIRONMENT}.tfbackend"
	terraform workspace select -or-create ${DEPLOY_ENVIRONMENT}

plan:
	terraform plan -var-file="../../configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars"

pre-commit:	format validate
