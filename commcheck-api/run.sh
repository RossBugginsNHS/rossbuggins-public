#!/bin/sh
cd src
docker buildx build -t commcheckapi .
docker compose up