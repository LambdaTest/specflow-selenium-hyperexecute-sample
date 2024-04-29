using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace SpecFlowLambdaSample
{
    [Binding]
    public sealed class GoogleSearch
    {
        private IWebDriver _driver;
        private IWebElement searchBox;
        private LambdaTestDriver LTDriver = null;

        String test_url = "https://www.google.com/";

        public GoogleSearch(ScenarioContext ScenarioContext)
        {
            LTDriver = (LambdaTestDriver)ScenarioContext["LTDriver"];
        }

        [Given(@"that I am on the Google app (.*) and (.*)")]
        public void GivenThatIAmOnTheBingAppAnd(string profile, string environment)
        {
            _driver = LTDriver.Init(profile, environment);
            _driver.Url = test_url;
            _driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);
        }

        [Given(@"that I open the Google app with (.*), (.*), (.*), (.*), and (.*)")]
        public void GivenThatIOpenTheBingAppWithAnd(
            string build,
            string name,
            string platform,
            string browserName,
            string version
        )
        {
            _driver = LTDriver.InitLocal(build, name, platform, browserName, version);
            _driver.Url = test_url;
            _driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);
        }

        [Then(@"click on the text box")]
        public void ThenClickOnTheTextBox()
        {
            searchBox = _driver.FindElement(By.XPath("//*[@id='APjFqb']"));
            searchBox.Click();
            System.Threading.Thread.Sleep(4000);
        }

        [Then(@"search for LambdaTest")]
        public void ThenSearchForLambdaTest()
        {
            searchBox.SendKeys("lambdatest.com" + Keys.Enter);
            System.Threading.Thread.Sleep(2000);
        }

        [Then(@"click on the first result")]
        public void ThenClickOnTheFirstResult()
        {
            System.Threading.Thread.Sleep(5000);
            int maxScrollAttempts = 5;
            bool elementFound = false;

            for (int i = 0; i < maxScrollAttempts && !elementFound; i++)
            {
                try
                {
                    IWebElement secondCheckBox = _driver.FindElement(
                        By.XPath(
                            "//*[@id='rso']/div[1]/div/div/div/div/div/div/div/div[1]/div/span/a/h3"
                        )
                    );
                    secondCheckBox.Click();
                    elementFound = true;
                }
                catch (NoSuchElementException)
                {
                    // Scroll the page
                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                    System.Threading.Thread.Sleep(2000); // Wait for the scroll to complete
                }
            }

            if (!elementFound)
            {
                Console.WriteLine("Element is present but not clickable at the moment.");
            }

            System.Threading.Thread.Sleep(2000);
        }
    }
}