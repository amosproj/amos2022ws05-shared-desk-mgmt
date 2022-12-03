# API Specification

Sinnvolle Error Messages sind gefordert.

## Type of routes
**resource route**
-> skip and n parameter for pagination
response:
```json
{
    "total": 100,
    "items": [
        {object},
        {object}
    ]
}
```

## Bookings

### Read
`GET /bookings/range -> rename to /bookings`
>resource route

```json
{
    "bookingId": "id of the booking", # NEEDED
    "startTime": "Start time of booking",
    "endTime": "end time of booking",
    "timestamp": "timestamp when booking is created",
    "deskId": "id of desk", # NEEDED
    "deskName": "name of desk",
    "buildingId": "id of building", # NEEDED
    "buildingName": "name of building",
    "floorId": "id of floor", # NEEDED
    "floorName": "name of floor",
    "roomId": "id of room", # NEEDED
    "roomName": "name of room"
}
```

### Create
`POST /bookings`

Request:
```json
{
    "startTime": "2022-12-02T09:43:04Z",
    "endTime": "1669974184",
    "deskId": "UUID"
}
```

Response:
```json
{
    "bookingId": "id of the booking", # NEEDED
    "startTime": "Start time of booking",
    "endTime": "end time of booking",
    "timestamp": "timestamp when booking is created",
    "deskId": "id of desk", # NEEDED
    "deskName": "name of desk",
    "buildingId": "id of building", # NEEDED
    "buildingName": "name of building",
    "floorId": "id of floor", # NEEDED
    "floorName": "name of floor",
    "roomId": "id of room", # NEEDED
    "roomName": "name of room"
}
```

### Update
`POST /bookings/{bookingId}`

Request:
```json
{
    "startTime": "2022-12-02T09:43:04Z",
    "endTime": "1669974184",
    "deskId": "UUID"
}
```

Response:
```json
{
    "bookingId": "id of the booking", # NEEDED
    "startTime": "Start time of booking",
    "endTime": "end time of booking",
    "timestamp": "timestamp when booking is created / updated", # NEEDED to refresh on update
    "deskId": "id of desk", # NEEDED
    "deskName": "name of desk",
    "buildingId": "id of building", # NEEDED
    "buildingName": "name of building",
    "floorId": "id of floor", # NEEDED
    "floorName": "name of floor",
    "roomId": "id of room", # NEEDED
    "roomName": "name of room"
}
```

### /bookings/recent (deprecated) --> remove


## Authentication

### /auth/createToken

### /auth/register


## User Management

`GET /users/me`
```json
{
    "FirstName",
    "LastName",
    "mailaddress",
    "company": {
        "companyId",
        "companyName",
        "companyLogo"
    },
    "isCompanyAdmin",
}
```

Get user information with a api call


**Admin Routes**
`GET /users`
>resource route

Get a list of users

`GET /users/{userId}`

`POST /users/{userId}`

`DELETE /users/{userId}`

`POST /users/{userId}/makeAdmin`

`POST /users/{userId}/approve`

`POST /users/{userId}/decline`

## Resource Management

### Read

#### All Buildings
`GET /resources/buildings`
```json
[
    {
        "buildingId",
        "buildingName",
        "location"
    }
]
```

#### All Floors per Building
`GET /resources/buildings/{buildingId}/floors`
```json
[
    {
        "floorId",
        "floorName"
    }
]
```

#### All rooms of a floor
`GET /resources/floors/{floorId}/rooms`
```json
[
    {
        "roomId",
        "roomName",
    }
]
```

#### All desks of a room
`GET /resources/rooms/{roomId}/desks`
```json
[
    {
        "deskId": "UUID of desk",
        "deskName": "",
        "bookedAt": [
            {
                "from": "timestamp",
                "to": "timestamp",
                "userName": "Max Mustermann"
            }, ....
        ]
    }
]
```

#### All information of a desk
`GET /resources/desks/{deskId}?from=&to=`
```json
{
    "deskId": "UUID of desk",
    "deskName": "",
    "bookedAt": [
        {
            "from": "timestamp",
            "to": "timestamp",
            "userName": "Max Mustermann"
        }, ....
    ]
}
```

### Create
`POST /resources/buildings`
```json
{
    "buildingName": "NAME",
    "location": "Test"
}
```

`POST /resources/floors`
```json
{
    "floorName": "NAME",
    "buildingId": "UUID"
}
```

`POST /resources/rooms`
```json
{
    "roomName": "NAME",
    "floorId": "UUID"
}
```

`POST /resources/desks`
```json
{
    "deskName": "NAME",
    "roomId": "UUID"
}
```

### Update (optional)
`POST /resources/buildings/{buildingId}`
```json
{
    "buildingName": "NAME",
    "location": "Test"
}
```

`POST /resources/floors/{floorId}`
```json
{
    "floorName": "NAME",
    "buildingId": "UUID"
}
```

`POST /resources/rooms/{roomId}`
```json
{
    "roomName": "NAME",
    "floorId": "UUID"
}
```

`POST /resources/desks/{desksId}`
```json
{
    "deskName": "NAME",
    "roomId": "UUID"
}
```

### Delete

`DELETE /resources/buildings/{buildingId}`

`DELETE /resources/floors/{floorId}`

`DELETE /resources/rooms/{roomId}`

`DELETE /resources/desks/{desksId}`



## Health Check

### /

### /withToken

### /admin
