# WAES_Test

This repository contains the WAES assignment test

## Opening the project:

In order to open this solution you may follow the below steps:

1. Download the zip file of the repository
2. Extract the content of the zip
3. Open the WAES_Test.sln
4. Build the solution

##  About the solution:

This solution contains 3 projects:

- WAES_Test
	- This project contains the Web API responsible for provide the 3 endpoints requested, those endpoints can be found in DataController.cs
- WAES_UnitTest
	- This project contains the unit tests responsible to validate the logic behind the endpoints described above, those tests can be found in DataControllerTests.cs
- WAES_IntegrationTest
	- This project contains the integration tests responsible for validating the integration between the endpoints and also de database, those tests can be found in IntegrationTests.cs

## Consuming the endpoints

To consume the endpoints is recommended to use Postman.

There are 2 different ways to consume those endpoints with postman, we can use the URL as localhost after running the project or we can using the AZURE URL.

### Running locally:

1. Build the solution
2. Run the solution

#### Left Endpoint:

1. Use the URL as localhost, please note that the port might change depends on your IIS express
2. also add the route v1/diff/{id}/left
	2.1.  --------------> Image of postman insert left
3. Add the content as an encoded Json, for example: ew0KCSJhZ2UiOjI2LA0KCSJuYW1lIjoiSmVhbiIsDQoJIm1lc3NhZ2UiOiAiV0FFUyINCn0=

#### Right Endpoint:
1. Use the URL as localhost, please note that the port might change depends on your IIS express
2. also add the route v1/diff/{id}/right
	2.1.  --------------> Image of postman insert right
3. Add the content as an encoded Json, for example: ew0KCSJhZ2UiOjI2LA0KCSJuYW1lIjoiSmVhbiIsDQoJIm1lc3NhZ2UiOiAiV0FFUyINCn0=

#### Diff Endpoint:
1. Use the URL as localhost, please note that the port might change depends on your IIS express
2. also add the route v1/diff/{id}
	3.  --------------> Image of postman diff

### Running on Azure:
To run the application pointing to Azure is basicaly the same of running locally, the only difference is the base URL that needs to be changed by waestestjean.azurewebsites.net
So to call the endpoints on Azure use:
- waestestjean.azurewebsites.net/v1/diff/{id}/left
- waestestjean.azurewebsites.net/v1/diff/{id}/right
- waestestjean.azurewebsites.net/v1/diff/{id}

## Running Unit tests
The unit tests can be triggerd on the TestExplorer window inside visual studio.
In order to test the methods without change any data in the database was created an abstraction of the data base context to moq the information needed.
  --------------> Image of the test explorer
The unit tests was also configured into VSTS and Azure as a Continuous integrations to run the unit tests on every commit -> push in git.
  --------------> Image of the VSTS build test result
## Running Integration tests
The integration tests also can be triggered in the TestExplorer windows inside the visual studio
on the oposit of the unit test, the integration test call the API`s methods usin an HTTP Client to test the integration with the other methods and also the database.

## Assumptions
- If the Diff endpoint is called to compare a pair of jsons but there is a json missing (left or right) the Get method return a BadRequest.
- If a Post endpoint is consuming without sending a content the method return a BadRequest
- If the Diff endpoint is consumed passing a inexistent id the method will return a BadRequest

## Improvements made on the original assignment
- CICD (Continuous Integration & Continuous Deployment) configured using Azure
- SQLServer Database used and also hosted into an Azure VM
