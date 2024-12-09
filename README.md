Sure! Below is a README template that summarizes the project, installation steps, usage, and details of the API endpoints and functionalities based on the code you provided.

---

# In_MethodAPI

This is an ASP.NET Core Web API project that exposes two methods (`IN_Method1` and `IN_Method2`) to process incoming requests, forward them to mock servers using HTTP, log the requests and responses in an in-memory database, and provide Swagger UI for API documentation.

## Features

- **API Endpoints**:
  - `POST /IN_Method1`: Processes a request and logs the interaction with mock servers.
  - `POST /IN_Method2`: Similar functionality to `IN_Method1` for handling different types of requests.
  - `GET /api/requests`: Retrieves a list of all request logs from the in-memory database.
  
- **In-Memory Database**: Stores the request and response logs using an in-memory database.
  
- **Mock Server Interaction**: The API interacts with multiple mock servers, rotating through them with round-robin logic to handle requests.

- **Swagger UI**: Provides interactive API documentation and testing interface during development.

## Prerequisites

Before you begin, make sure you have the following installed:

- [.NET 6.0 or later](https://dotnet.microsoft.com/download)
- [Python 3.12](https://www.python.org/) (optional for testing)
- [pip](https://pip.pypa.io/en/stable/) (for Python dependencies)

## Installation

### 1. Clone the repository
Clone the project repository to your local machine using Git:

```bash
git clone https://github.com/poalelungi-ion/test_qsystems
cd In_MethodAPI
```

### 2. Set up the .NET Web API

- Open the project in your favorite IDE (e.g., [Visual Studio](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/)).
- Restore the project dependencies by running:

```bash
dotnet restore
```

- Run the application locally using the following command:

```bash
dotnet run
```

The API will be hosted at `https://localhost:44361` (or another port depending on your setup).

### 3. Install Python dependencies (optional)

If you plan to use the provided Python dummy app to send requests to the API, follow these steps:

1. Install the `requests` library in Python:

```bash
pip install requests
```

2. Use the Python script provided in the `/python` directory (or similar, depending on your project structure).

## API Endpoints

### 1. `POST /IN_Method1`

#### Request

- **URL**: `https://localhost:44361/IN_Method1`
- **Method**: `POST`
- **Request Body** (JSON):
  ```json
  {
    "requestData": "string"
  }
  ```

#### Response

- **Status Code**: `200 OK` or `500 Internal Server Error`
- **Response Body**: Returns a response from the mock server (or an error message if all servers fail).

### 2. `POST /IN_Method2`

#### Request

- **URL**: `https://localhost:44361/IN_Method2`
- **Method**: `POST`
- **Request Body** (JSON):
  ```json
  {
    "requestData": "string"
  }
  ```

#### Response

- **Status Code**: `200 OK` or `500 Internal Server Error`
- **Response Body**: Returns a response from the mock server (or an error message if all servers fail).

### 3. `GET /api/requests`

#### Request

- **URL**: `https://localhost:44361/api/requests`
- **Method**: `GET`

#### Response

- **Status Code**: `200 OK`
- **Response Body**: A list of all request logs stored in the in-memory database.
  ```json
  [
    {
      "id": 1,
      "request": "{\"requestData\":\"sample data\"}",
      "response": "Mock server response",
      "method": "IN_Method1",
      "component": "Component1",
      "timestamp": "2024-12-09T12:00:00Z"
    }
  ]
  ```

## Example Python Script to Send Requests

The following Python script uses the `requests` library to send data to the `IN_Method1` and `IN_Method2` endpoints.

```python
import requests
import json

url_method1 = "https://localhost:44361/IN_Method1"
url_method2 = "https://localhost:44361/IN_Method2"

# Define the payload to be sent in the POST request
data = {
    "requestData": "sample data"
}

headers = {
    "Content-Type": "application/json"
}

# Send the request to IN_Method1
response_method1 = requests.post(url_method1, json=data, headers=headers, verify=False)
print(f"IN_Method1 Response: {response_method1.text}")

# Send the request to IN_Method2
response_method2 = requests.post(url_method2, json=data, headers=headers, verify=False)
print(f"IN_Method2 Response: {response_method2.text}")
```

### Notes:

- **verify=False**: Disables SSL verification. This is useful when you're using `https` locally with self-signed certificates.
- The script will print the response from the API (either from the mock server or an error message).

## How It Works

1. When a request is made to `IN_Method1` or `IN_Method2`, the server processes the request, forwards it to a mock server using round-robin logic, and logs the request and response in an in-memory database.
2. If a mock server fails to respond, the request is retried on the next mock server until one responds or all servers fail.
3. All interactions (requests and responses) are stored and can be retrieved via the `/api/requests` endpoint.

## Troubleshooting

- If you encounter a `500 Internal Server Error` response, ensure that all dependencies are installed correctly and that the mock servers are up and running.
- For SSL certificate issues when using `https`, you may need to disable SSL verification temporarily or add the certificate to your trusted store.
