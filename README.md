# Task Management API

## Overview
This project is a simple **Task Management API** that provides CRUD operations for managing tasks. Additionally, the application includes an automated reminder system that sends notifications for tasks with upcoming due dates. The scheduling of these reminders is handled using **Hangfire**.

## Features
- **CRUD Operations:** Create, Read, Update, and Delete tasks.
- **Task Reminders:** Automated notifications for tasks nearing their deadline.
- **Background Processing:** Hangfire is used for scheduling reminders.

## Technologies Used
- **ASP.NET Core** (for API development)
- **Entity Framework Core** (for database management)
- **Hangfire** (for task scheduling and background processing)

## Installation

### Prerequisites
- .NET 9 SDK

### Steps to Set Up
1. **Clone the repository:**
   ```sh
   git clone https://github.com/s3ba-b/tasks-api
   cd tasks-api
   ```
4. **Start the application:**
   ```sh
   dotnet run --launch-profile https
   ```
5. **Access API Documentation:**
   - Open `http://localhost:<port>/swagger` in your browser.

## API Endpoints

| Method | Endpoint             | Description                 |
|--------|----------------------|-----------------------------|
| GET    | `/api/tasks`         | Get all tasks              |
| GET    | `/api/tasks/{id}`    | Get a task by ID           |
| POST   | `/api/tasks`         | Create a new task          |
| PUT    | `/api/tasks/{id}`    | Update a task              |
| DELETE | `/api/tasks/{id}`    | Delete a task              |

## Task Reminder System
- The system periodically checks for tasks with upcoming deadlines.
- If a task is due within **2 hours**, a reminder is sent.
- Hangfire manages scheduled jobs for sending these notifications.

## Contributing
1. Fork the repository
2. Create a new branch (`feature-branch`)
3. Commit your changes
4. Push to your branch
5. Create a Pull Request

## License
This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Contact
For any inquiries or issues, please open an issue on GitHub.