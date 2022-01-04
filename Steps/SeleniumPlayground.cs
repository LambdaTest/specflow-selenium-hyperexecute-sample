using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

namespace SpecFlowLambdaSample
{
    [Binding]
    public sealed class SeleniumPlayground
    {
        private IWebDriver _driver;
        private LambdaTestDriver LTDriver = null;

        String test_url = "https://www.lambdatest.com/selenium-playground/";

        public SeleniumPlayground(ScenarioContext ScenarioContext)
        {
            LTDriver = (LambdaTestDriver)ScenarioContext["LTDriver"];
        }

        [Given(@"I go to Selenium playground home page (.*) and (.*)")]
        public void GivenThatIAmOnSeleniumPlayground(string profile, string environment)
        {

            _driver = LTDriver.Init(profile, environment);
            _driver.Url = test_url;
            _driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);
        }

        [Then(@"I Click on Input Form Link")]
        public void ThenClickInputFormLink()
        {
            IWebElement element = _driver.FindElement(By.XPath("//a[.='Input Form Submit']"));
            element.Click();

            String current_url = _driver.Url;
            Console.WriteLine("Current URL is " + current_url);
        }

        [Then(@"I enter items in the form")]
        public void ThenEnterItems()
        {
            IWebElement name = _driver.FindElement(By.XPath("//input[@id='name']"));
            name.SendKeys("Testing");

            IWebElement email_address = _driver.FindElement(By.XPath("//input[@name='email']"));
            email_address.SendKeys("testing@testing.com");

            IWebElement password = _driver.FindElement(By.XPath("//input[@name='password']"));
            password.SendKeys("password");

            IWebElement company = _driver.FindElement(By.CssSelector("#company"));
            company.SendKeys("LambdaTest");

            IWebElement website = _driver.FindElement(By.CssSelector("#websitename"));
            website.SendKeys("https://wwww.lambdatest.com");

            IWebElement countryDropDown = _driver.FindElement(By.XPath("//select[@name='country']"));
            SelectElement selectElement = new SelectElement(countryDropDown);
            selectElement.SelectByText("United States");

            IWebElement city = _driver.FindElement(By.XPath("//input[@id='inputCity']"));
            city.SendKeys("San Jose");

            IWebElement address1 = _driver.FindElement(By.CssSelector("[placeholder='Address 1']"));
            address1.SendKeys("Googleplex, 1600 Amphitheatre Pkwy");

            IWebElement address2 = _driver.FindElement(By.CssSelector("[placeholder='Address 2']"));
            address2.SendKeys(" Mountain View, CA 94043");

            IWebElement state = _driver.FindElement(By.CssSelector("#inputState"));
            state.SendKeys("California");

            IWebElement zipcode = _driver.FindElement(By.CssSelector("#inputZip"));
            zipcode.SendKeys("94088");
        }

        [When(@"I click submit button")]
        public void WhenClickSubmitButton()
        {
            /* Click on the Submit button */
            IWebElement submit_button = _driver.FindElement(By.CssSelector(".btn"));
            submit_button.Click();
        }

        [Then(@"I should verify if form submission was successful")]
        public void ThenVerifySubmitSuccessful()
        {
            /* Assert if the page contains a certain text */
            bool bValue = _driver.PageSource.Contains("Thanks for contacting us, we will get back to you shortly");

            if (bValue)
            {
                Console.WriteLine("Input Form Demo successful");
            }
            else
            {
                Console.WriteLine("Input Form Demo failed");
            }
        }

        [Then(@"close the corresponding browser")]
        public void ThenCloseTheBrowserUnderTest()
        {
            _driver.Close();
        }
    }
}
