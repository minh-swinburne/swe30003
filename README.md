# SWE30003 - Software Architectures and Design

New User Registration Request Body:
```json
{
  "firstName": "Minh",
  "lastName": "Nguyen",
  "email": "minh@gmail.com",
  "phone": "+84 123 456 789",
  "password": "minh1234!",
  "picture": ""
}
```

Login Request Body:
```json
{
  "email": "minh@gmail.com",
  "password": "minh1234!"
}
```

Create Ride Request:
```json
{
  "passengerId": "<USER_ID>",
  "pickupAddress": "Empire State Building, 20 W 34th St., New York, NY 10001, United States",
  "pickupLatitude": 40.7487166425943,
  "pickupLongitude": -73.9858545105651,
  "destinationAddress": "Ichiran, 374 Johnson Ave, Brooklyn, NY 11206, United States",
  "destinationLatitude": 40.70879863918965,
  "destinationLongitude": -73.93306202092786,
  "vehicleType": 2,
  "rideType": 1,
  "paymentMethod": 3,
  "notes": "Wait at the south gate"
}
```