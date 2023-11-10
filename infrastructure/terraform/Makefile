init:
	rm -Rf .terraform
	terraform init -backend-config "configuration/partials/${DEPLOY_ENVIRONMENT}.tfbackend"
	terraform workspace select -or-create ${DEPLOY_ENVIRONMENT}

format:
	terraform fmt -recursive

plan:
	terraform plan -var-file="configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars"

apply:
	terraform apply -var-file="configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars"

apply-auto-approve:
	terraform apply -var-file="configuration/environments/${DEPLOY_ENVIRONMENT}.tfvars" -auto-approve
