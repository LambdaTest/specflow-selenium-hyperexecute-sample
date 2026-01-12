Feature: ToDoTest
    Test the LambdaTest ToDo sample application
    Verify checkboxes and adding new items work correctly

@ToDoTest
Scenario Outline: Add and verify ToDo items
    Given I am on the ToDo app <profile> and <environment>
    When I click on the first checkbox
    And I click on the second checkbox
    Then I should see two items checked
    When I enter "Yey, Let's add it to list" in the text box
    And I click the add button
    Then I should see "Yey, Let's add it to list" in the list

    Examples:
        | profile  | environment |
        | parallel | chrome      |
        | parallel | firefox     |
