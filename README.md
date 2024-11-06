Project Overview
This project was designed using an N-Tier Architecture model.

API Documentation: The API is documented using Swagger.
Authentication: JWT (JSON Web Token) is used for authentication processes.
Database Operations: Managed through Entity Framework Core.
Framework: Built on ASP.NET Core Web API infrastructure.
User Interface: The front-end utilizes basic HTML, JavaScript, and CSS.

Usage

API Management: The project can be managed via Swagger. After running database migrations, initial seed data entries include "superadmin," "apple," and "banana."
Login: Access to the admin panel requires login. The password for the "superadmin" user (stored as a hash in the database) is also set to "superadmin."
Token Generation: Upon login, a token is generated for the user. This token, along with the "Bearer " prefix, must be entered in Swagger's Authorize button (located at the top-right) to gain access to admin-only endpoints.
User Interface
For the UI, an automatic token generation is set up for the superadmin user when a login request is sent via JavaScript.

Local Access: Run the project locally by accessing:

http://localhost:{port}/html/index.html

Features
User Roles: Three user roles are defined within the application: Superadmin, Admin, and Customer. Role-based authorization is applied across the project.
Superadmin: Has the highest privileges and access to all endpoints.
Admin: Has access to all endpoints except for those exclusively accessible to the Superadmin.
Token Expiration: Tokens assigned to users expire and are removed 15 minutes after issuance.
