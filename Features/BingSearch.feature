Feature: BingSearchLT
	Open Google
	Search for LambdaTest on the page

@BingSearch
Scenario: Perform Bing Search for LambdaTest
	Given that I am on the Bing app <profile> and <environment>
	Then click on the text box
	Then search for LambdaTest
	Then click on the first result

	Examples:
		| profile	| environment |
		| single    | chrome      |
		| parallel	| firefox     |
		| parallel	| edge	      |