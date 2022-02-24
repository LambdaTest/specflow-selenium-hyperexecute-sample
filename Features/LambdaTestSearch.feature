Feature: DuckDuckGoLTBlog
	Open DuckDuckGo
	Search for LambdaTest Blog on the page
	Check results

@LambdaTestBlogSearch_1
Scenario: Perform DuckDuckGo Search for LambdaTest_1
	Given that I am on the DuckDuckGo Search Page with <build>, <name>, <platform>, <browserName>, and <version>
	Then search for LambdaTest Blog
	Then click on the available result
	Then compare results

	Examples:
		| build			  	  | name		        | platform	     | browserName	 | version   |
		| Blog Search Parallel Test - 1	  | Blog Search Parallel Test - 1	| Windows 10 	 | Firefox	     | latest    |
		| Blog Search Parallel Test - 2	  | Blog Search Parallel Test - 2	| Windows 10     | MicrosoftEdge | latest    |
		| Blog Search Parallel Test - 3	  | Blog Search Parallel Test - 3	| Windows 10     | Chrome 	     | latest    |

@LambdaTestBlogSearch_2
Scenario: Perform DuckDuckGo Search for LambdaTest_2
	Given that I am on the DuckDuckGo Search Page with <build>, <name>, <platform>, <browserName>, and <version>
	Then search for LambdaTest Blog
	Then click on the available result
	Then compare results

	Examples:
		| build			  	  | name		        | platform	     | browserName	 | version   |
		| Blog Search Parallel Test - 4	  | Blog Search Parallel Test - 4	| Windows 10 	 | Firefox	     | latest    |
		| Blog Search Parallel Test - 5	  | Blog Search Parallel Test - 5	| Windows 10     | MicrosoftEdge | latest    |
		| Blog Search Parallel Test - 6	  | Blog Search Parallel Test - 6	| Windows 10     | Chrome 	     | latest    |

@LambdaTestBlogSearch_3
Scenario: Perform DuckDuckGo Search for LambdaTest_3
	Given that I am on the DuckDuckGo Search Page with <build>, <name>, <platform>, <browserName>, and <version>
	Then search for LambdaTest Blog
	Then click on the available result
	Then compare results

	Examples:
		| build			  	  | name		        | platform	     | browserName	 | version   |
		| Blog Search Parallel Test - 7	  | Blog Search Parallel Test - 7	| Windows 10 	 | Firefox	     | latest    |
		| Blog Search Parallel Test - 8	  | Blog Search Parallel Test - 8	| Windows 10     | MicrosoftEdge | latest    |
		| Blog Search Parallel Test - 9	  | Blog Search Parallel Test - 9	| Windows 10     | Chrome 	     | latest    |
