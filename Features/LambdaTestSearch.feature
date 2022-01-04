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
	Then close the current browser window

	Examples:
		| build			  | name		| platform	 | browserName	 | version   |
		| Parallel Test - 1	  | Parallel Test - 1	| MacOS Catalina | Chrome	 | latest    |
		| Parallel Test - 2	  | Parallel Test - 2	| Windows 11 	 | Chrome	 | latest-1  |
		| Parallel Test - 3	  | Parallel Test - 3	| MacOS Catalina | Firefox	 | latest    |
		| Parallel Test - 4	  | Parallel Test - 4	| MacOS Montere  | MicrosoftEdge | latest-1  |
