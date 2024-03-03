# SantderTestWebAPI
 
# Hacker News API Integration

This project is an ASP.NET Core Web API(.Net 8.0) that retrieves the details of the first n "best stories" from the Hacker News API.

## How to Run the Application

### Prerequisites
- .NET Core SDK installed on your machine.
- Visual Studio 2022 or Visual Studio Code or any other IDE.

###Git repository
https://github.com/sanjaykumarporeddi/SantanderTestWebAPI
-SSH key   git@github.com:sanjaykumarporeddi/SantanderTestWebAPI.git

### Steps
1. Clone this repository.
2. Open the solution file (`SantanderTestWebAPI.sln`) in Visual Studio or your preferred IDE.
3. Build the solution.
4. Run the application.

## Usage

The API provides the following endpoint to retrieve the first n best stories:

GET /api/beststories/{count}


Replace `{count}` with the desired number of stories to retrieve.

## Configuration
The application contains a valid configuration for API settings and caching duration in the `appsettings.json` file.

## Basic Testing Performed

1. The Hacker News API endpoints (`BestStoriesUrl` and `StoryDetailsUrl`) are accessible and return the expected data format.

## Potential Enhancements or Changes

1. **Error Handling**: Implement error handling for HttpClient requests to handle network failures and unexpected responses gracefully.
2. **Logging**: Improve logging to facilitate debugging and monitoring of the application.
3. **Additional Endpoints**: Enhance the API by adding additional endpoints to fetch specific story details or other Hacker News features.

## Contributors

- [Sanjay Kumar Poreddivar](https://github.com/sanjaykumarporeddi)

Feel free to contribute by submitting pull requests or reporting issues.
