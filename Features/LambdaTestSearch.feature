Feature: DuckDuckGoLTBlog
	Open DuckDuckGo
	Search for LambdaTest Blog on the page
	Check results

@LambdaTestBlogSearch
Scenario: Perform DuckDuckGo Search for LambdaTest
	Given that I am on the DuckDuckGo Search Page with <build>, <name>, <platform>, <browserName>, and <version>
	Then search for LambdaTest Blog
	Then click on the available result
	Then compare results

	Examples:
		| build			  	  | name		        | platform	     | browserName	 | version   |
		| Parallel Test - 1	  | Parallel Test - 1	| Windows 10 	 | Firefox	     | latest    |
		| Parallel Test - 2	  | Parallel Test - 2	| Windows 10     | MicrosoftEdge | latest    |
		| Parallel Test - 3	  | Parallel Test - 3	| Windows 10     | Chrome 	     | latest    |
