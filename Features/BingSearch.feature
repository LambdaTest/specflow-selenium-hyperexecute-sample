Feature: BingSearchLT
	Open Google
	Search for LambdaTest on the page

@BingSearch_1
Scenario Outline: Perform Bing Search for LambdaTest_1
	Given that I am on the Bing app <profile> and <environment>
	Then click on the text box
	Then search for LambdaTest
	Then click on the first result

	Examples:
		| profile	| environment |
		| single    | chrome      |
		| parallel	| firefox     |
		| parallel	| edge	      |

@BingSearch_2
Scenario Outline: Perform Bing Search for LambdaTest_2
	Given that I am on the Bing app <profile> and <environment>
	Then click on the text box
	Then search for LambdaTest
	Then click on the first result

	Examples:
		| profile	| environment |
		| parallel	| edge	      |

@BingSearch_3
Scenario Outline: Perform Bing Search for LambdaTest_3
	Given that I open the Bing app with <build>, <name>, <platform>, <browserName>, and <version>
	Then click on the text box
	Then search for LambdaTest
	Then click on the first result

	Examples:
		| build			  	  		  | name		            | platform	     | browserName	 | version   |
		| Bing Parallel Test - 1	  | Bing Parallel Test - 1	| Windows 10 	 | Firefox	     | latest    |
		| Bing Parallel Test - 2	  | Bing Parallel Test - 2	| Windows 10     | MicrosoftEdge | latest    |
		| Bing Parallel Test - 3	  | Bing Parallel Test - 3	| Windows 10     | Chrome 	     | latest    |
