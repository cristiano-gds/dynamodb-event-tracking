#!/bin/sh
cd /aws-services

# Wait just in case localstack delays the start
sleep 7s

# Create table in dynamoDB
aws dynamodb create-table --endpoint-url http://localstack:4566 --cli-input-json file://create-table.json

