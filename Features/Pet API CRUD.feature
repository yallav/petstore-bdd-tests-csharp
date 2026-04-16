Feature: Pet API CRUD

Automated CRUD API tests for the Swagger Petstore using C#

Scenario: Create, Read, Update and Delete a pet
	When I create a new pet with the following details:
		| Id  | Name   | Photourls                     | Status    |
		| 123 | Fluffy | test.jpg, end-to-end test.png | available |
	Then the pet should be created successfully
	When I retrieve the pet by its ID 123
	Then the pet should exist

	When I update the below pet with the following details:
		| Id  | Name   | Photourls                     | Status    |
		| 123 | UpdatedFluffy | test.jpg, end-to-end test.png | available |
	Then the pet should be updated

	When I delete the pet with ID 123
	Then the pet should not exist
