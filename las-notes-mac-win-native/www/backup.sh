#!/bin/bash
set -e

if ! [ `which 7z` ]; then
  echo "Please install 7-zip"
  exit 1
fi

cleanup() {
  rm -r bkp/
}
trap cleanup EXIT

# MySQL
PWD=`cat .env | awk -F '[=]' '{print $2}'`
docker compose exec -i db mysqldump -uroot -p$PWD --databases wordpress > dump.sql

# Wordpress
mkdir -p bkp/
cp -r wpdata/ bkp/
find bkp/ -name "*.dmg" | xargs rm -f
find bkp/ -name "*.exe" | xargs rm -f
7z a wpdata-no-uploads.7z bkp/wpdata/

# done
echo "Done! Upload and remove the following files:"
echo "scp root@lasnotes.com:/root/wpdata-no-uploads.7z ."
echo "scp root@lasnotes.com:/root/dump.sql ."
