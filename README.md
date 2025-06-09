# Gotlandsrussen

<details close>
<summary>Project description</summary>
|| Project Description ||
  
Gotlandsrussen is a RESTful hotel booking API built using ASP.NET Core (.NET 8). It simulates a full booking flow, allowing guests to; Book, Update, Cancel and Query available rooms. They can also calculate prises based on date and guest count. The API supports both guest and administrative endpoints. 

This project was developed by a team of code students for a school assignment. Focus was on clean architecture and data validation using both MSTest and Postman for flow validation. 

---
|| Architecture Overview ||

The project is structured into the following main components: 

## GotlandsrussenAPI
Handles HTTP requests and acts as the web layer. It includes: 

Controllers
- GuestController.cs: Endpoints for guests (create booking, cancel bookings, etc.)
- ManagementController.cs: Admin operations (view bookings, update booking, etc.)

Repositories
- Implements the repository pattern to abstract data acces (BookingRepository, RoomRepository, etc.)

Data
- HotelDbContext.cs: EF Core DbContext
- SeedData.cs: Populates data with initial data

Migrations
- Entity Framework migration tracking

## HotelGotlandsrussenLIBRARY
Contains core models and DTOs used across the entire application:

- Models: Domain entities (Booking, Room, RoomType, Guest)
- DTOs: Input/output data transfer objects (UpdateBookingDto, RoomDto, etc.)

## HotelGotlandsRussenTESTS
Handles automated unit testing using MSTest: 

- TestSetup: Base setup for shared tests objects
- Tests: Contains all controller and repository test files

## Functional Flow (Postman)
A full end-to-end test suite was created using Postman. 
It covers following: 

- Create a new guest
- Create booking (with or without breakfast)
- Add Breakfast to a booking
- Update Booking
- Cancel a booking
- Get total price
- Check available rooms by dates and guests
- Delete guest and booking

  ## Tech Stack
  - ASP.NET Core (.NET 8)
  - Entity Framework Core (EF Core)
  - PostgreSQL
  - MSTest (Unit testing)
  - Swagger (API docs)
  - Postman (Integration testing)

</details>

<details close>
<summary>API documentation</summary>

---
|      | Endpoint                                           | Parameters                                                        | Description                                        |
|------|----------------------------------------------------|-------------------------------------------------------------------|----------------------------------------------------|
| GET  | `/api/Guest/GetAllGuests`                          |                                                                   | Gets all guests                                    |
| GET  | `/api/Guest/AvailableRooms`                        | startDate, endDate                                                | Gets available rooms for a specific period of days |
| PUT  | `/api/Guest/AddBreakfast`                          | BookingId                                                         | Adds breakfast to a specific booking               |
| PUT  | `/api/Guest/CancelBooking`                         | bookingId                                                         | Cancels a specific booking                         |
| POST | `/api/Guest/CreateGuest`                           | FirstName, LastName, Email, Phone                                 | Creates a new guest                                |
| DEL  | `/api/Guest/DeleteGuest`                           | guestId                                                           | Deletes a guest by id                              |
|      |                                                    |                                                                   |                                                    |
| GET  | `/api/Management/GetAllFutureBookings`             |                                                                   | Gets all future bookings                           |
| GET  | `/api/Management/GetBookingsGroupedByWeek`         |                                                                   | Gets future bookings grouped by week               |
| GET  | `/api/Management/GetBookingsGroupedByMonth`        |                                                                   | Gets future bookings grouped by month              |
| GET  | `/api/Management/GetBookingById`                   | id                                                                | Gets a booking with specific id                    |
| GET  | `/api/Management/GetTotalPrice`                    | BookingId                                                         | Gets the total sum for a specific booking          |
| GET  | `/api/Management/GetAvailableRoomsByDateAndGuests` | fromDate, toDate, adults, children                                | Gets available rooms by specific conditions        |
| GET  | `/api/Management/GetBookingHistory`                |                                                                   | Gets all past bookings                             |
| PUT  | `/api/Management/UpdateBooking`                    | Id, FromDate, ToDate, NumberOfAdults, NumberOfChildren, Breakfast | Updates a booking                                  |
| POST | `/api/Management/CreateBooking`                    | roomId, guestId, fromDate, toDate, adults, children, breakfast    | Creates a new booking                              |
| DEL  | `/api/Management/DeleteBooking`                    | bookingId                                                         | Deletes a booking                                  |
---
<details close>
<summary>See all guests</summary>
<br>
  
````
[GET] /api/Guest/GetAllGuests
````
**Request URL**
````
https://localhost:7072/api/Guest/GetAllGuests
````

**Example Response**
````
[
    {
        "id": 2,
        "firstName": "Bob",
        "lastName": "Bengtsson",
        "email": "bob@example.com",
        "phone": "0702345678",
        "bookings": null
    }
]
````
</details>

<details close>
<summary>See available rooms</summary>
<br>
  
````
[GET] /api/Guest/GetAvailableRooms
````
**Example Request URL**
````
https://localhost:7047/api/Guest/available-rooms?startDate=2025-08-01&endDate=2025-08-03
````

**Example Response**
````
[
    {
        "id": 5,
        "roomName": "105",
        "roomTypeName": "Single",
        "numberOfBeds": 1,
        "pricePerNight": 500.00
    }
]
````
</details>

<details close>
<summary>Add breakfast to a booking</summary>
<br>
  
````
[PUT] /api/Guest/AddBreakfast
````
**Example Request URL**
````
https://localhost:7047/api/Guest/AddBreakfast?BookingId=18
````
**Example Response**
````
{
    "bookingId": 18,
    "breakfast": true,
    "message": "Breakfast has been added to the booking."
}
````

</details>

<details close>
<summary>Cancel a booking</summary>
<br>
  
````
[PUT] /api/Guest/CancelBooking
````
**Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Add a guest</summary>
<br>
  
````
[POST] /api/Guest/CreateGuest
````
**Example Request URL**
````
https://localhost:7047/api/Guest/CreateGuest?FirstName=Test&LastName=Testsson&Email=test%40testmail.com&Phone=555-444333
````

**Example Response**
````
{
  "id": 23,
  "firstName": "Test",
  "lastName": "Testsson",
  "email": "test@testmail.com",
  "phone": "555-444333",
  "bookings": null
}
````
</details>


<details close>
<summary>Delete a guest</summary>
<br>
  
````
[DEL] /api/Guest/DeleteGuest
````
**Example Request URL**
````
https://localhost:7047/api/Guest/DeleteGuest?guestId=5
````

**Example Response**
````
No response body
````
</details>

<details close>
<summary>See all future bookings</summary>
<br>
  
````
[GET] /api/Management/GetAllFutureBookings
````

**Request URL**
````
https://localhost:7047/api/Management/GetAllFutureBookings
````

**Example Response**
````
[
    {
        "id": 3,
        "guestName": "Larsson, Tom",
        "roomNames": [
            "103"
        ],
        "bookedFromDate": "2025-06-11",
        "bookedToDate": "2025-06-13",
        "numberOfAdults": 1,
        "numberOfChildren": 0
    }
]

````
</details>

<details close>
<summary>See all future bookings grouped by week</summary>
<br>
  
````
[GET] /api/Management/GetBookingsGroupedByWeek
````
**Request URL**
````
https://localhost:7047/api/Management/GetBookingsGroupedByWeek
````

**Example Response**
````
[
    {
        "year": 2025,
        "week": 25,
        "bookings": [
            {
                "id": 4,
                "guestName": "Helgesson, Lisa",
                "roomNames": [
                    "106"
                ],
                "bookedFromDate": "2025-06-20",
                "bookedToDate": "2025-06-25",
                "numberOfAdults": 1,
                "numberOfChildren": 0
            },
            {
                "id": 7,
                "guestName": "Johansson, Oscar",
                "roomNames": [
                    "109"
                ],
                "bookedFromDate": "2025-06-16",
                "bookedToDate": "2025-06-18",
                "numberOfAdults": 2,
                "numberOfChildren": 0
            }
        ]
    }
]
````
</details>

<details close>
<summary>See all future bookings grouped by month</summary>
<br>
  
````
[GET] /api/Management/GetBookingsGroupedByMonth
````
**Request URL**
````
https://localhost:7047/api/Management/GetBookingsGroupedByMonth
````

**Example Response**
````
[
    {
        "year": 2025,
        "month": 6,
        "bookings": [
            {
                "id": 3,
                "guestName": "Larsson, Tom",
                "roomNames": [
                    "103"
                ],
                "bookedFromDate": "2025-06-11",
                "bookedToDate": "2025-06-13",
                "numberOfAdults": 1,
                "numberOfChildren": 0
            },
            {
                "id": 4,
                "guestName": "Helgesson, Lisa",
                "roomNames": [
                    "106"
                ],
                "bookedFromDate": "2025-06-20",
                "bookedToDate": "2025-06-25",
                "numberOfAdults": 1,
                "numberOfChildren": 0
            },
        ]
    }
]
   

````
</details>

<details close>
<summary>Get a booking by id</summary>
<br>
  
````
[GET] /api/Management/GetBookingById
````
**Example Request URL**
````
https://localhost:7047/api/Management/GetBookingById?id=10
````

**Example Response**
````
{
  "id": 10,
  "guestId": 10,
  "fromDate": "2025-07-01",
  "toDate": "2025-07-05",
  "numberOfAdults": 2,
  "numberOfChildren": 2,
  "isCancelled": true,
  "breakfast": false,
  "guest": {
    "id": 10,
    "firstName": "Maja",
    "lastName": "Gustafsson",
    "email": "maja@example.com",
    "phone": "0702345686",
    "bookings": [
      null
    ]
  },
  "bookingRooms": [
    {
      "id": 10,
      "bookingId": 10,
      "booking": null,
      "roomId": 13,
      "room": {
        "id": 13,
        "name": "113",
        "roomTypeId": 3,
        "roomType": {
          "id": 3,
          "name": "Family",
          "numberOfBeds": 4,
          "pricePerNight": 1500,
          "rooms": [
            null
          ]
        },
        "bookingRooms": [
          null
        ]
      }
    }
  ]
}

````
</details>

<details close>
<summary>See the total price for a booking</summary>
<br>
  
````
[GET] /api/Management/GetTotalPrice?BookingId=3
````
**Example Request URL**
````
https://localhost:7047/api/Management/GetTotalPrice?BookingId=3
````

**Example Response**
````
{
  "bookingId": 3,
  "rooms": [
    {
      "roomType": "Single",
      "pricePerNight": 500
    }
  ],
  "numberOfNights": 2,
  "numberOfGuests": 1,
  "numberOfBreakfasts": 0,
  "breakfastPrice": 50,
  "totalPrice": 1000
}
````
</details>

<details close>
<summary>See available rooms by date and number of guests</summary>
<br>
  
````
[GET] /api/Management/GetAvailableRoomByDateAndGuests
````
**Example Request URL**
````
https://localhost:7047/api/Management/GetAvailableRoomByDateAndGuests?fromDate=2025-09-12&toDate=2025-09-15&adults=2&children=2
````

**Example Response**
````
[
  {
    "id": 20,
    "roomName": "120",
    "roomTypeName": "Family",
    "numberOfBeds": 4,
    "pricePerNight": 1500
  },
  {
    "id": 19,
    "roomName": "119",
    "roomTypeName": "Family",
    "numberOfBeds": 4,
    "pricePerNight": 1500
  }
]

````
</details>


<details close>
<summary>See the booking history</summary>
<br>
  
````
[GET] /api/Management/GetBookingHistory
````
**Request URL**
````
https://localhost:7047/api/Management/GetBookingHistory
````

**Example Response**
````
[
  {
    "id": 1,
    "guestName": "Andersson, Anna",
    "roomNames": [
      "102"
    ],
    "bookedFromDate": "2025-06-05",
    "bookedToDate": "2025-06-06",
    "numberOfAdults": 1,
    "numberOfChildren": 0
  }
]

````
</details>

<details close>
<summary>Update a booking</summary>
<br>
  
````
[PUT] /api/Management/UpdateBooking
````
**Example Request URL**
````
https://localhost:7047/api/Management/UpdateBooking?Id=4&FromDate=2026-08-17&ToDate=2026-08-18&NumberOfAdults=2&NumberOfChildren=0&Breakfast=true
````

**Example Response**
````
{
  "id": 4,
  "guestId": 4,
  "fromDate": "2026-08-17",
  "toDate": "2026-08-18",
  "numberOfAdults": 2,
  "numberOfChildren": 0,
  "isCancelled": false,
  "breakfast": true,
  "guest": null,
  "bookingRooms": [
    {
      "id": 4,
      "bookingId": 4,
      "booking": null,
      "roomId": 6,
      "room": {
        "id": 6,
        "name": "106",
        "roomTypeId": 2,
        "roomType": {
          "id": 2,
          "name": "Double",
          "numberOfBeds": 2,
          "pricePerNight": 900,
          "rooms": [
            null
          ]
        },
        "bookingRooms": [
          null
        ]
      }
    }
  ]
}
````
</details>

<details close>
<summary>Create a new booking</summary>
<br>
  
````
[POST] /api/Management/CreateBooking
````
**Example Request URL**
````
https://localhost:7047/api/Management/CreateBooking?roomId=8&guestId=9&fromDate=2027-08-10&toDate=2027-08-13&adults=2&children=0&breakfast=true
````

**Example Response**
````
{
  "newBooking": {
    "bookingId": 21,
    "guestId": 9,
    "fromDate": "2027-08-10",
    "toDate": "2027-08-13",
    "numberOfAdults": 2,
    "numberOfChildren": 0,
    "breakfast": true,
    "roomIds": [
      8
    ]
  }
}

````
</details>

<details close>
<summary>Delete a booking</summary>
<br>
  
````
[DEL] /api/Management/DeleteBooking
````
**Example Request URL**
````
https://localhost:7047/api/Management/DeleteBooking?bookingId=5
````

**Example Response**
````
No response body
````
</details>
</details>

<details close>
<summary>Test strategy</summary>
  
---
  
### Unit Tests for Controller
To test our controllers, we used Moq to mock dependencies from the repository classes. We could simulate various scenarios and ensure that the appropriate HTTP responses were returned.

---
### Unit Tests for Repositories
For repository testing we used an in-memory database (InMemoryDatabase from Entity Framework). This allowed us to verify data database communication without depending on a real database.

---
### Integration Tests
For integration testing we used Postman by sending real HTTP requests to the API. This helped us verify that all endpoints worked as expected, including correct status codes, response bodies, and error handling. We used Postman to test full flows, such as creating bookings, finding available rooms, and cancelling bookings.

---
### Test Results
A few bugs were detected during testing. One example is that breakfast costs were included in the total booking amount, even when the customer had chosen to book without breakfast. These bugs were corrected during the testing phase.

We do not have full test coverage for all methods, mainly due to time constraints. Several new methods were added toward the end of the project, and we did not have enough time to implement tests for all of them. However, all members of the project group contributed to writing both controller and repository tests, and we prioritized testing our initial methods.


</details>
