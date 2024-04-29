Feature: GoogleSearchLT
	Open Google
	Search for LambdaTest on the page

@GoogleSearch_1
Scenario Outline: Perform Google Search for LambdaTest_2
	Given that I am on the Google app <profile> and <environment>
	Then click on the text box
	Then search for LambdaTest
	Then click on the first result

	Examples:
		| profile	| environment |
		| parallel	| edge	      |