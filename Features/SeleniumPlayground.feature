Feature: Selenium Playground
	Locate the Input Demo on the page
	Add relevant data in the input form

@SeleniumPlayground
Scenario: Add items on input form
	Given I go to Selenium playground home page <profile> and <environment>
  	Then I Click on Input Form Link
  	Then I enter items in the form
 	When I click submit button
  	Then I should verify if form submission was successful
	Then close the corresponding browser

	Examples:
		| profile	| environment |
		| single    	| chrome      |
		| parallel	| chrome      |
