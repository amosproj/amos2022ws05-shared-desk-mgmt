# Local Setup

## Local production run

1. Copy the `.env.template` file to `.env.local`:
```bash
cp .env.template .env.local
```

2. Add the necessary settings to the `.env.local` or change it. The `EMAIL__PSW` is needed to run the backend. Please change the mail settings accordingly.

3. Inside the `src` folder run:

```bash
./start.sh
```

4. The frontend should now be reachable on `http://localhost:3000`. After that you should be able to register, login and use the software.

5. OPTIONAL: If you want to fill the database with our dummy-data you can use the following command:
```bash
docker exec -i deskstar_postgres psql -U postgres deskstar < ./deskstar-db/dummy-data.sql
```
The dummy data already contain some companies you can use and also some test users. You can find the login data at the bottom of this README.

## Local Dev Setup

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

### 6. Login

There are currently 2 test user in the database and 1 company. In order to login you can use the following data:

**Company:** 8745f064-c658-4a32-83fb-9d7d7e6d8f17

**Email:** bob.employee@acme.com (normal user) or alice.admin@acme.com (admin user)

**Password:** test123
