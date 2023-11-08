# Github repository credential monitoring solution

This repository includes an automated solution to detect credentials staged for commit to Github.

## Reusable CredentialDetection Workflow - Setup

If working on source code housed in a Github repository, the Informed.Repo.CredentialScan.Verified.CI.yml file in the .github/workflows directory simply needs mirrored to the target repository to avail of automatic scanning whenever a push or pull request event is detected (note the directory structure must remain intact).

## Local Development Requirements

The pre-commit solution included in this repository requires all users have docker installed on their development environment.

## Local Development setup and use

To help avoid sensitive credentials being deposited to source control, a pre-configured git pre-commit hook can be registered by running the command `git config --local core.hooksPath .githooks/` in the root directory of this repository. Alternatively, the command `make git-credential-config` can be run if Make is installed.

When staging content for a commit, files will be automatically scanned and if credentials verified, the commit will be aborted.

## Makefile content

A shorthand for running credential scans manually at any time is included in this solution's source. To trigger a git (verified only) scan, run the command `make credential-scan-git-verified`. Please see Makefile contents for other supported scan types.

## Filtering false positives

On issuing the `make git-credential-config` command, a `credential-scan-exclusions.txt` file will be created in the root of your repository directory (if it does not already exist). If a .gitignore file is found in the repository root, this will be used as the template with newlines removed. Regex filepaths can then be added to this file (newline separated) to filter out false positives on a git commit command being issued.
