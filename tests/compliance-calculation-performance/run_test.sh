#!/bin/bash

set -e

usage() {
  cat <<EOM
    Usage:
    $(basename "$0") <ROWS_COUNT> <CHUNK_SIZE>

    ROWS_COUNT – count of rows in the CSV
    CHUNK_SIZE – size of the chunk used while processing the CSV

    e.g. $(basename "$0") 10 10
EOM
  exit 0
}

[ -z "$1" ] && { usage; }


REGION=europe-west1
FUNCTION_NAME=dev-zev-compliance-calculation-service

BUCKET_NAME=dev-zev-manufacturer-data
BUCKET_FOLDER=test_script_data

csv_filename=$(date '+%Y-%m-%d-%H-%M-%S').csv

mkdir -p test_data

echo "Generating $1 rows to file $csv_filename"

# Generate CSV
python3 csv_generator.py "test_data/$csv_filename" "$1"

# Upload the CSV to the storage bucket
gsutil cp "test_data/$csv_filename" "gs://$BUCKET_NAME/$BUCKET_FOLDER/"

echo "CSV uploaded successfully"
echo

# Run the Cloud Function
response=$(gcloud functions call "$FUNCTION_NAME" --region "$REGION" --data "{ \"FileName\": \"$BUCKET_FOLDER/$csv_filename\" }")
json_response=$(echo $response | tr -d "'")
exit_code=$?

# Parse response
echo "Function exited with code $exit_code and response $json_response"
echo

execution_id=$(jq -r '.ExecutionId' <<< "$json_response")
total_time=$(jq -r '.ExecutionTime' <<< "$json_response")
processing_time=$(jq -r '.ProcessingTime' <<< "$json_response")

# Print logs
echo "Execution logs:"
gcloud functions logs read "$FUNCTION_NAME" --region "$REGION" --execution-id "$execution_id"

echo
echo "Total function execution time:                      $total_time"
echo "Processing time (CSV processing and DB operations): $processing_time"
