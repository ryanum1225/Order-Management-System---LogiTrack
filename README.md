# Order-Management-System---LogiTrack
A system to manage inventory items and customer orders across multiple fulfilment centers.

## Features
- Add and remove items in the inventory
- Create orders
- Implemented ASP.Net Identity for authentication/authorization control.

## Challenges
- Implementing ASP.Net Identity was a massive struggle that necessitated me going back to a pervious version of the LogiTrack API.
- Using DTOs to send data from the API without exposing sensitive data took a while to set up.

## Buisness Logic
The API uses models to represent InventoryItems, Orders, ApplicationUsers, and Roles. Orders have a one-to-many relationship with InventoryItems. ApplicationUsers have a one-to-one relationship with Roles.

## API Design
The API has a controller for each of the models specified above. Most of these endpoints are only accessible by registered users, except for the API endpoint that handles user login.

## Security
The Sqlite database is already configured with an admin user and a list of Roles. Program.cs implements the main policies that are used to secure API endpoints: UserPolicy - which are accessible by any Role, EditorPolicy - which are accessible only by Managers and Admins, and AdminPolicy - which are accessible only by Admins.

## Caching
In-Memory Caching was used in the InventoryItem Controller and the Order Controller, specifically to prevent overusing resources. Caching is also how the API handles state management.
