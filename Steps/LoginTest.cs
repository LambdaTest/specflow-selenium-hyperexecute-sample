using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace SpecFlowLambdaSample
{
    [Binding]
    public sealed class LoginTest
    {
        private IWebDriver _driver;
        private LambdaTestDriver LTDriver = null;
        private readonly string test_url = "https://the-internet.herokuapp.com/login";

        public LoginTest(ScenarioContext ScenarioContext)
        {
            LTDriver = (LambdaTestDriver)ScenarioContext["LTDriver"];
        }

        [Given(@"I am on the Login page (.*) and (.*)")]
        public void GivenIAmOnTheLoginPage(string profile, string environment)
        {
            _driver = LTDriver.Init(profile, environment);
            _driver.Navigate().GoToUrl(test_url);
            
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("username")).Displayed);
            Console.WriteLine("Navigated to Login page");
        }

        [When(@"I enter username ""(.*)""")]
        public void WhenIEnterUsername(string username)
        {
            IWebElement usernameField = _driver.FindElement(By.Id("username"));
            usernameField.Clear();
            usernameField.SendKeys(username);
            Console.WriteLine($"Entered username: {username}");
        }

        [When(@"I enter password ""(.*)""")]
        public void WhenIEnterPassword(string password)
        {
            IWebElement passwordField = _driver.FindElement(By.Id("password"));
            passwordField.Clear();
            passwordField.SendKeys(password);
            Console.WriteLine("Entered password");
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            IWebElement loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            loginButton.Click();
            Console.WriteLine("Clicked Login button");
        }

        [Then(@"I should see a success message containing ""(.*)""")]
        public void ThenIShouldSeeASuccessMessageContaining(string expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("flash")).Displayed);

            IWebElement flashMessage = _driver.FindElement(By.Id("flash"));
            string messageText = flashMessage.Text;
            
            Assert.That(messageText, Does.Contain(expectedMessage), 
                $"Expected message to contain '{expectedMessage}' but got '{messageText}'");
            Console.WriteLine($"Verified flash message contains: {expectedMessage}");
        }

        [Then(@"I should see an error message containing ""(.*)""")]
        public void ThenIShouldSeeAnErrorMessageContaining(string expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("flash")).Displayed);

            IWebElement flashMessage = _driver.FindElement(By.Id("flash"));
            string messageText = flashMessage.Text;
            
            Assert.That(messageText, Does.Contain(expectedMessage), 
                $"Expected error message to contain '{expectedMessage}' but got '{messageText}'");
            Console.WriteLine($"Verified error message contains: {expectedMessage}");
        }

        [Then(@"the URL should contain ""(.*)""")]
        public void ThenTheURLShouldContain(string urlPart)
        {
            Assert.That(_driver.Url, Does.Contain(urlPart), 
                $"Expected URL to contain '{urlPart}' but got '{_driver.Url}'");
            Console.WriteLine($"Verified URL contains: {urlPart}");
        }
    }
}
