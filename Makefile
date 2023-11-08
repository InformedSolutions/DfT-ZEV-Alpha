git-credential-config: create-credential-exclusions-file-if-not-exists bind-pre-commit

bind-pre-commit:
	chmod -R +x .githooks
	git config --local core.hooksPath .githooks/

create-credential-exclusions-file-if-not-exists:
ifeq (,$(wildcard credential-scan-exclusions.txt))
	echo "# File contents should include line separated regex filepaths (analagous to a gitignore file)" > credential-scan-exclusions.txt
endif

credential-scan-git-verified:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --only-verified --fail

credential-scan-git-unverified:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --fail --exclude-paths /opt/credential-scan-exclusions.txt

credential-scan-git-verified-no-update-no-log:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --only-verified --no-update --fail > /dev/null

credential-scan-git-unverified-no-update-no-log:
	docker run --rm -it -v $(PWD):/opt trufflesecurity/trufflehog:latest git file:///opt --no-update --fail > /dev/null
