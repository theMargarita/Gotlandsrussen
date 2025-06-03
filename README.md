# Gotlandsrussen

---

## Översikt
|      | Endpoint                                           | Parameters                                                        | Description                                        |
|------|----------------------------------------------------|-------------------------------------------------------------------|----------------------------------------------------|
| GET  | `/api/Guest/GetAllGuests`                          |                                                                   | Gets all guests                                    |
| GET  | `/api/Guest/GetBookingById`                        | bookingId - TA BORT?                                              | Gets a booking with specific id                    |
| GET  | `/api/Guest/GetAvailableRooms`                     | startDate, endDate                                                | Gets available rooms for a specific period of days |
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
| POST | `/api/Management/CreateBooking`                    | xxxxxxxx                                                          | Creates a new booking                              |
| DEL  | `/api/Management/DeleteBooking`                    | bookingId                                                         | Deletes a booking                                  |
---
<details close>
<summary>Get All Guests</summary>
<br>
  
````
[GET] /api/Guest/GetAllGuests
````
**Request URL**
````
https://localhost:7072/api/Guest/GetAllGuests
````

**Example Response:**
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
<summary>Get Available Rooms</summary>
<br>
  
````
[GET] /api/Guest/GetAvailableRooms
````
**Example Request URL**
````
https://localhost:7047/api/Guest/available-rooms?startDate=2025-08-01&endDate=2025-08-03
````

**Example Response:**
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
<summary>Add Breakfast</summary>
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
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Create Guest</summary>
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
<summary>Delete Guest</summary>
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
No response
````
</details>

<details close>
<summary>GetAllFutureBookings</summary>
<br>
  
````
[GET] /api/Management/GetAllFutureBookings
````

**Example Request URL**
````
https://localhost:7047/api/Management/GetAllFutureBookings
````

**Example Response:**
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
<summary>Get future bookings grouped by week</summary>
<br>
  
````
[GET] /api/Management/GetBookingsGroupedByWeek
````
**Request URL**
````
https://localhost:7047/api/Management/GetBookingsGroupedByWeek
````

**Example Response:**
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
<summary>Get future bookings grouped by month</summary>
<br>
  
````
[GET] /api/Management/GetBookingsGroupedByMonth
````
**Request URL**
````
https://localhost:7047/api/Management/GetBookingsGroupedByMonth
````

**Example Response:**
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
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Lägg till en URL</summary>
<br>
  
````
[POST] /api/Person/{personId}/interests/{InterestId}/add-link
````
**Request URL**
````
https://localhost:7072//api/Person/{personId}/interests/{InterestId}/add-link
````
**Request**
````
{
  "url": "string"
}
````
**Beskrivning**
````
Lägger till en ny länk till ett intresse som är kopplat till en person.
````

**Exempel Response**
````
{
  "id": 21,
  "personInterestId": 7,
  "personInterest": {
    "id": 7,
    "personId": 5,
    "person": {
      "id": 5,
      "firstName": "Max",
      "lastName": "Bengtsson",
      "telefonnummer": "0701234564",
      "email": "max@example.com",
      "personInterests": [
        null
      ]
    },
    "interestId": 2,
    "interest": null,
    "link": [
      null
    ]
  },
  "url": "www.fz.com"
}
````
</details>

<details close>
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>


<details close>
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Lägg till en URL</summary>
<br>
  
````
[POST] /api/Person/{personId}/interests/{InterestId}/add-link
````
**Request URL**
````
https://localhost:7072//api/Person/{personId}/interests/{InterestId}/add-link
````
**Request**
````
{
  "url": "string"
}
````
**Beskrivning**
````
Lägger till en ny länk till ett intresse som är kopplat till en person.
````

**Exempel Response**
````
{
  "id": 21,
  "personInterestId": 7,
  "personInterest": {
    "id": 7,
    "personId": 5,
    "person": {
      "id": 5,
      "firstName": "Max",
      "lastName": "Bengtsson",
      "telefonnummer": "0701234564",
      "email": "max@example.com",
      "personInterests": [
        null
      ]
    },
    "interestId": 2,
    "interest": null,
    "link": [
      null
    ]
  },
  "url": "www.fz.com"
}
````
</details>

<details close>
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Lägg till en URL</summary>
<br>
  
````
[POST] /api/Person/{personId}/interests/{InterestId}/add-link
````
**Request URL**
````
https://localhost:7072//api/Person/{personId}/interests/{InterestId}/add-link
````
**Request**
````
{
  "url": "string"
}
````
**Beskrivning**
````
Lägger till en ny länk till ett intresse som är kopplat till en person.
````

**Exempel Response**
````
{
  "id": 21,
  "personInterestId": 7,
  "personInterest": {
    "id": 7,
    "personId": 5,
    "person": {
      "id": 5,
      "firstName": "Max",
      "lastName": "Bengtsson",
      "telefonnummer": "0701234564",
      "email": "max@example.com",
      "personInterests": [
        null
      ]
    },
    "interestId": 2,
    "interest": null,
    "link": [
      null
    ]
  },
  "url": "www.fz.com"
}
````
</details>

<details close>
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Lägg till en URL</summary>
<br>
  
````
[POST] /api/Person/{personId}/interests/{InterestId}/add-link
````
**Request URL**
````
https://localhost:7072//api/Person/{personId}/interests/{InterestId}/add-link
````
**Request**
````
{
  "url": "string"
}
````
**Beskrivning**
````
Lägger till en ny länk till ett intresse som är kopplat till en person.
````

**Exempel Response**
````
{
  "id": 21,
  "personInterestId": 7,
  "personInterest": {
    "id": 7,
    "personId": 5,
    "person": {
      "id": 5,
      "firstName": "Max",
      "lastName": "Bengtsson",
      "telefonnummer": "0701234564",
      "email": "max@example.com",
      "personInterests": [
        null
      ]
    },
    "interestId": 2,
    "interest": null,
    "link": [
      null
    ]
  },
  "url": "www.fz.com"
}
````
</details>

<details close>
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Lägg till en URL</summary>
<br>
  
````
[POST] /api/Person/{personId}/interests/{InterestId}/add-link
````
**Request URL**
````
https://localhost:7072//api/Person/{personId}/interests/{InterestId}/add-link
````
**Request**
````
{
  "url": "string"
}
````
**Beskrivning**
````
Lägger till en ny länk till ett intresse som är kopplat till en person.
````

**Exempel Response**
````
{
  "id": 21,
  "personInterestId": 7,
  "personInterest": {
    "id": 7,
    "personId": 5,
    "person": {
      "id": 5,
      "firstName": "Max",
      "lastName": "Bengtsson",
      "telefonnummer": "0701234564",
      "email": "max@example.com",
      "personInterests": [
        null
      ]
    },
    "interestId": 2,
    "interest": null,
    "link": [
      null
    ]
  },
  "url": "www.fz.com"
}
````
</details>

<details close>
<summary>Cancel Booking</summary>
<br>
  
````
[POST] /api/Guest/CancelBooking
````
** Example Request URL**
````
https://localhost:7047/api/Guest/CancelBooking?bookingId=10
````

**Example Response:**
````
{
    "message": "Booking is cancelled"
}

````
</details>

<details close>
<summary>Lägg till en URL</summary>
<br>
  
````
[POST] /api/Person/{personId}/interests/{InterestId}/add-link
````
**Request URL**
````
https://localhost:7072//api/Person/{personId}/interests/{InterestId}/add-link
````
**Request**
````
{
  "url": "string"
}
````
**Beskrivning**
````
Lägger till en ny länk till ett intresse som är kopplat till en person.
````

**Exempel Response**
````
{
  "id": 21,
  "personInterestId": 7,
  "personInterest": {
    "id": 7,
    "personId": 5,
    "person": {
      "id": 5,
      "firstName": "Max",
      "lastName": "Bengtsson",
      "telefonnummer": "0701234564",
      "email": "max@example.com",
      "personInterests": [
        null
      ]
    },
    "interestId": 2,
    "interest": null,
    "link": [
      null
    ]
  },
  "url": "www.fz.com"
}
````
</details>

---

# Markdown Cheat Sheet

Thanks for visiting [The Markdown Guide](https://www.markdownguide.org)!

This Markdown cheat sheet provides a quick overview of all the Markdown syntax elements. It can’t cover every edge case, so if you need more information about any of these elements, refer to the reference guides for [basic syntax](https://www.markdownguide.org/basic-syntax/) and [extended syntax](https://www.markdownguide.org/extended-syntax/).

## Basic Syntax

These are the elements outlined in John Gruber’s original design document. All Markdown applications support these elements.

### Heading

# H1
## H2
### H3

### Bold

**bold text**

### Italic

*italicized text*

### Blockquote

> blockquote

### Ordered List

1. First item
2. Second item
3. Third item

### Unordered List

- First item
- Second item
- Third item

### Code

`code`

### Horizontal Rule

---

### Link

[Markdown Guide](https://www.markdownguide.org)

### Image

![alt text](https://www.markdownguide.org/assets/images/tux.png)

## Extended Syntax

These elements extend the basic syntax by adding additional features. Not all Markdown applications support these elements.

### Table

| Syntax | Description |
| ----------- | ----------- |
| Header | Title |
| Paragraph | Text |

### Fenced Code Block

```
{
  "firstName": "John",
  "lastName": "Smith",
  "age": 25
}
```

### Footnote

Here's a sentence with a footnote. [^1]

[^1]: This is the footnote.

### Heading ID

### My Great Heading {#custom-id}

### Definition List

term
: definition

### Strikethrough

~~The world is flat.~~

### Task List

- [x] Write the press release
- [ ] Update the website
- [ ] Contact the media

### Emoji

That is so funny! :joy:

(See also [Copying and Pasting Emoji](https://www.markdownguide.org/extended-syntax/#copying-and-pasting-emoji))

### Highlight

I need to highlight these ==very important words==.

### Subscript

H~2~O

### Superscript

X^2^
