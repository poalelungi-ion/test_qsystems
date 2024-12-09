import requests
import json

# Define the API base URL
base_url = "https://localhost:44361"  # Replace with your actual API URL

# Define the data to send in the requests
data = {
    "requestData": "string"
}

# Method 1: Send request to /IN_Method1
response_method1 = requests.post(
    f"{base_url}/IN_Method1",
    json=data,
    headers={"Content-Type": "application/json"}
)

if response_method1.status_code == 200:
    print("Method 1 Response:", response_method1.json())
else:
    print("Method 1 Failed with status code:", response_method1.status_code)

# Method 2: Send request to /IN_Method2
response_method2 = requests.post(
    f"{base_url}/IN_Method2",
    json=data,
    headers={"Content-Type": "application/json"}
)

if response_method2.status_code == 200:
    print("Method 2 Response:", response_method2.json())
else:
    print("Method 2 Failed with status code:", response_method2.status_code)
