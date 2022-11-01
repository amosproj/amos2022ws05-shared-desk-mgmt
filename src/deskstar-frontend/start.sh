#!/bin/bash

rm -rdf node_modules .next
docker build -t deskstar-frontend:latest .
docker run -p 3000:3000 -it deskstar-frontend:latest 