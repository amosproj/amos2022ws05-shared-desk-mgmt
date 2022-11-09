## Create dockerimg

```
docker build -t deskstardb:v1
```

## Run Docker

```
docker run --name=deskstar -p 5432:5432 -e POSTGRES_DB=deskstar -e POSTGRES_PASSWORD=root deskstardb:v1
```

## Restore DB

### Mac

```
cat deskstar_dump.sql | docker exec -i deskstar psql -U postgres
```

### Windows

```
type deskstar_dump.sql | docker exec -i deskstar psql -U postgres
```

## Create Backup

```
docker exec -t deskstar pg_dumpall -c -U postgres > deskstar_dump.sql
```
