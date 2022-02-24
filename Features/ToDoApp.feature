Feature: TodoApp
	Select first two items in the ToDoApp
	Enter a new item in the ToDoApp
	Add the new item to the list

@ToDoApp_1
Scenario: Add items to the ToDoApp_1
	Given that I am on the LambdaTest Sample app <profile> and <environment>
	Then select the first item
	Then select the second item
	Then find the text box to enter the new value
	Then click the Submit button
	And  verify whether the item is added to the list

	Examples:
		| profile	| environment 		 |
		| single    | edge      		 |
		| parallel	| firefox            |
		| parallel	| chrome             |

@ToDoApp_2
Scenario: Add items to the ToDoApp_2
	Given that I am on the LambdaTest Sample app <profile> and <environment>
	Then select the first item
	Then select the second item
	Then find the text box to enter the new value
	Then click the Submit button
	And  verify whether the item is added to the list

	Examples:
		| profile	| environment 		 |
		| single    | edge      		 |
		| parallel	| firefox            |
		| parallel	| chrome             |

@ToDoApp_3
Scenario: Add items to the ToDoApp_3
	Given that I am on the LambdaTest Sample app <profile> and <environment>
	Then select the first item
	Then select the second item
	Then find the text box to enter the new value
	Then click the Submit button
	And  verify whether the item is added to the list

	Examples:
		| profile	| environment 		 |
		| single    | edge      		 |
		| parallel	| firefox            |
		| parallel	| chrome             |
