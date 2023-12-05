#!/bin/bash
check="false"
attempts=0
max_attempts=30

# Note that with the max_attempts being set at 30 if the build is not
# found in 15 minutes, a non-zero exit code will be returned failing the build

if [ -z "$build_id_url" ];
then
    echo "Error: Build ID URL variable not set"
    exit 1
fi

if [ -z "$build_id" ];
then
    echo "Error: Build ID variable not set"
    exit 1
fi

echo "Remote url $build_id_url"
echo "Checking for build id ${build_id} on remote host"

until [ $check = "true" ]
do

  if [ "$attempts" -eq "$max_attempts" ];
  then
      echo "Build id not found after ${max_attempts} attempts"
      exit 1
  fi

  if curl -s "$build_id_url" | grep -w "$build_id";
  then
      echo "Build id found"
      exit
  else
      sleep 30;
      attempts=$(( $attempts + 1 ))
      echo "Waiting for build id ${build_id} to be returned by remote host...";
  fi

done