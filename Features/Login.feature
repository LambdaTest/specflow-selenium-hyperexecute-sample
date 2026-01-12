Feature: LoginTest
    Test the login functionality on the-internet.herokuapp.com
    Verify successful and failed login scenarios

@LoginTest_Success
Scenario Outline: Successful Login Test
    Given I am on the Login page <profile> and <environment>
    When I enter username "tomsmith"
    And I enter password "SuperSecretPassword!"
    And I click the login button
    Then I should see a success message containing "You logged into a secure area!"
    And the URL should contain "/secure"

    Examples:
        | profile  | environment |
        | parallel | chrome      |
        | parallel | firefox     |

@LoginTest_Failure
Scenario Outline: Failed Login Test
    Given I am on the Login page <profile> and <environment>
    When I enter username "invaliduser"
    And I enter password "wrongpassword"
    And I click the login button
    Then I should see an error message containing "Your username is invalid!"
    And the URL should contain "/login"

    Examples:
        | profile  | environment |
        | parallel | chrome      |
        | parallel | firefox     |
