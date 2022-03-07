Feature: SeleniumPlayground
	Locate the Input Demo on the page
	Add relevant data in the input form

@SeleniumPlayground_1
Scenario Outline: Testing on Selenium Playground_1
	Given I go to Selenium playground home page <profile> and <environment>
  	Then I Click on Input Form Link
  	Then I enter items in the form
 	When I click submit button
  	Then I should verify if form submission was successful

	Examples:
		| profile	| environment |
		| single    | firefox     |
		| parallel	| chrome      |
		| parallel	| edge        |

@SeleniumPlayground_2
Scenario Outline: Testing on Selenium Playground_2
	Given I go to Selenium playground home page <profile> and <environment>
  	Then I Click on Input Form Link
  	Then I enter items in the form
 	When I click submit button
  	Then I should verify if form submission was successful

	Examples:
		| profile	| environment |
		| single    | firefox     |
		| parallel	| chrome      |
		| parallel	| edge        |

@SeleniumPlayground_3
Scenario Outline: Testing on Selenium Playground_3
	Given I go to Selenium playground home page <profile> and <environment>
  	Then I Click on Input Form Link
  	Then I enter items in the form
 	When I click submit button
  	Then I should verify if form submission was successful

	Examples:
		| profile	| environment |
		| single    | firefox     |
		| parallel	| chrome      |
		| parallel	| edge        |
