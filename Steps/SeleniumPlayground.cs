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
            System.Threading.Thread.Sleep(2000);

            String current_url = _driver.Url;
            Console.WriteLine("Current URL is " + current_url);
        }

        [Then(@"I enter items in the form")]
        public void ThenEnterItems()
        {
            IWebElement name = _driver.FindElement(By.CssSelector("#name"));
            name.SendKeys("Testing");
            System.Threading.Thread.Sleep(2000);

            IWebElement email_address = _driver.FindElement(By.CssSelector("#inputEmail4"));
            email_address.SendKeys("testing@testing.com");
            System.Threading.Thread.Sleep(2000);

            IWebElement password = _driver.FindElement(By.CssSelector("#inputPassword4"));
            password.SendKeys("password");
            System.Threading.Thread.Sleep(2000);

            IWebElement company = _driver.FindElement(By.CssSelector("#company"));
            company.SendKeys("LambdaTest");
            System.Threading.Thread.Sleep(2000);

            IWebElement website = _driver.FindElement(By.CssSelector("#websitename"));
            website.SendKeys("https://wwww.lambdatest.com");
            System.Threading.Thread.Sleep(2000);

            IWebElement countryDropDown = _driver.FindElement(By.XPath("//select[@name='country']"));
            SelectElement selectElement = new SelectElement(countryDropDown);
            selectElement.SelectByText("United States");
            System.Threading.Thread.Sleep(2000);

            IWebElement city = _driver.FindElement(By.XPath("//input[@id='inputCity']"));
            city.SendKeys("San Jose");
            System.Threading.Thread.Sleep(2000);

            IWebElement address1 = _driver.FindElement(By.CssSelector("[placeholder='Address 1']"));
            address1.SendKeys("Googleplex, 1600 Amphitheatre Pkwy");
            System.Threading.Thread.Sleep(2000);

            IWebElement address2 = _driver.FindElement(By.CssSelector("[placeholder='Address 2']"));
            address2.SendKeys(" Mountain View, CA 94043");
            System.Threading.Thread.Sleep(2000);

            IWebElement state = _driver.FindElement(By.CssSelector("#inputState"));
            state.SendKeys("California");
            System.Threading.Thread.Sleep(2000);

            IWebElement zipcode = _driver.FindElement(By.CssSelector("#inputZip"));
            zipcode.SendKeys("94088");
            System.Threading.Thread.Sleep(2000);
        }

        [When(@"I click submit button")]
        public void WhenClickSubmitButton()
        {
            /* Click on the Submit button */
            IWebElement submit_button = _driver.FindElement(By.CssSelector(".btn"));
            submit_button.Click();
            System.Threading.Thread.Sleep(2000);
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
    }
}
