# Mayhem Tournament Management System

## Overview

The Mayhem Tournament Management System is a comprehensive solution for managing tournaments, including user statistics, game codes, and rewards distribution. The system integrates with Ethereum blockchain for secure and transparent reward distribution.

## Features

- Tournament creation and management
- User statistics tracking
- Game code management
- Reward distribution using Ethereum blockchain
- RESTful API for interaction with the system

## Technologies Used

- C# 12.0
- .NET 8
- Entity Framework Core
- AutoMapper
- FluentValidation
- Nethereum
- ASP.NET Core
- Microsoft SQL Server

## Requirements

- .NET 8 SDK
- Microsoft SQL Server
- Node.js (for Nethereum)
- Ethereum node (e.g., Infura)

## Installation

1. Clone the repository:
git clone https://github.com/yourusername/mayhem-tournament-management.git
cd mayhem-tournament-management

2. Set up the database:
dotnet ef database update

3. Configure the application settings:
    - Update the `appsettings.json` file with your database connection string and Ethereum node details.

4. Restore the dependencies:
dotnet restore

## Running the Application

1. Build the application:
dotnet build

2. Run the application:
dotnet run --project WebApplication1

3. The application will be available at `https://localhost:5001`.

## API Endpoints

### Tournaments

- `POST /api/tournaments` - Create a new tournament
- `GET /api/tournaments/active` - Get the active tournament
- `GET /api/tournaments/archived` - Get archived tournaments
- `PUT /api/tournaments/{id}` - Update a tournament
- `POST /api/tournaments/end` - End the active tournament

### User Statistics

- `GET /api/userstatistics` - Get user statistics
- `POST /api/userstatistics` - Add user statistics

### Game Codes

- `POST /api/gamecodes` - Create a new game code
- `GET /api/gamecodes/active` - Get active game codes for a tournament
- `DELETE /api/gamecodes/{id}` - Remove a game code

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. The license belongs to Mayhem Games OÜ.