# Local Dev Setup

### 1. Build and open in Dev Container

### 2. Update the database

Inside the folder ./src/deskstar-backend/Deskstar execute the following command

```
dotnet ef database update
```

### 3. Start the backend

Inside the folder ./src/deskstar-backend/Deskstar execute the `run.sh` script

### 4. Start the frontend

Inside the folder ./src/deskstar-backend/Deskstar execute following command

```
yarn dev
```

You might need to run `yarn` before, to update all dependencies

### 5. (optional) Import dummy data

**Option 1 (use GUI):**
Use a GUI like TablePlus to import the `dummy-data.sql` file located in the deskstar-db folder

**Option 2 (use Terminal):**

Import content from sql dump (inital dummy data can be found in ```src/deskstar-db/dummy-data.sql)

```
docker exec -i amos2022ws05-shared-desk-mgmt_devcontainer-db-1 psql -U postgres < <path-to-sql-dump>

```

Export content dump (beware that it exports the migration table as well)

```
docker exec -t amos2022ws05-shared-desk-mgmt_devcontainer-db-1 pg_dump --column-inserts --data-only -U postgres > dump_`date +%d-%m-%Y"_"%H_%M_%S`.sql
```
