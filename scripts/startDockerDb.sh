#!/bin/bash

if docker ps | grep -s -q "socialwork-mariadb-container"
    then
        echo "Database is up and running on port 3306"
        exit 0
    else
        docker start socialwork-mariadb-container
fi

until docker ps | grep -s -q "socialwork-mariadb-container"
    do
        sleep 0.1;
done

echo "Database is up and running on port 3306"