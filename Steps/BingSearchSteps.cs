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
    public sealed class BingSearch
    {
        private IWebDriver _driver;
        private IWebElement searchBox;
        private LambdaTestDriver LTDriver = null;

        String test_url = "https://www.bing.com/";

        public BingSearch(ScenarioContext ScenarioContext)
        {
            LTDriver = (LambdaTestDriver)ScenarioContext["LTDriver"];
        }

        [Given(@"that I am on the Bing app (.*) and (.*)")]
        public void GivenThatIAmOnTheBingAppAnd(string profile, string environment)
        {
            _driver = LTDriver.Init(profile, environment);
            _driver.Url = test_url;
            _driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);
        }
        
        [Given(@"that I open the Bing app with (.*), (.*), (.*), (.*), and (.*)")]
        public void GivenThatIOpenTheBingAppWithAnd(string build, string name, string platform, 
                    string browserName, string version)
        {
            _driver = LTDriver.InitLocal(build, name, platform, browserName, version);
            _driver.Url = test_url;
            _driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(2000);
        }

        [Then(@"click on the text box")]
        public void ThenClickOnTheTextBox()
        {
            searchBox = _driver.FindElement(By.XPath("//input[@id='sb_form_q']"));
            searchBox.Click();
            System.Threading.Thread.Sleep(4000);
        }

        [Then(@"search for LambdaTest")]
        public void ThenSearchForLambdaTest()
        {
            searchBox.SendKeys("LambdaTest" + Keys.Enter);
            System.Threading.Thread.Sleep(2000);
        }

        [Then(@"click on the first result")]
        public void ThenClickOnTheFirstResult()
        {
            IWebElement secondCheckBox = _driver.FindElement(By.LinkText("Most Powerful Cross Browser Testing Tool Online"));
            secondCheckBox.Click();                                     
            System.Threading.Thread.Sleep(2000);
        }
    }
}
