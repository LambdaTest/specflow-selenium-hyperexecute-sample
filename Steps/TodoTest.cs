using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace SpecFlowLambdaSample
{
    [Binding]
    public sealed class TodoTest
    {
        private IWebDriver _driver;
        private LambdaTestDriver LTDriver = null;
        private readonly string test_url = "https://lambdatest.github.io/sample-todo-app/";

        public TodoTest(ScenarioContext ScenarioContext)
        {
            LTDriver = (LambdaTestDriver)ScenarioContext["LTDriver"];
        }

        [Given(@"I am on the ToDo app (.*) and (.*)")]
        public void GivenIAmOnTheToDoApp(string profile, string environment)
        {
            _driver = LTDriver.Init(profile, environment);
            _driver.Navigate().GoToUrl(test_url);
            
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Name("li1")).Displayed);
            Console.WriteLine("Navigated to ToDo app");
        }

        [When(@"I click on the first checkbox")]
        public void WhenIClickOnTheFirstCheckbox()
        {
            _driver.FindElement(By.Name("li4")).Click();
            Console.WriteLine("Clicked first checkbox (li4)");
        }

        [When(@"I click on the second checkbox")]
        public void WhenIClickOnTheSecondCheckbox()
        {
            _driver.FindElement(By.Name("li5")).Click();
            Console.WriteLine("Clicked second checkbox (li5)");
        }

        [Then(@"I should see two items checked")]
        public void ThenIShouldSeeTwoItemsChecked()
        {
            IList<IWebElement> elems = _driver.FindElements(By.ClassName("ng-not-empty"));
            Assert.That(elems.Count, Is.EqualTo(2), "Expected 2 checked items");
            Console.WriteLine("Verified 2 items are checked");
        }

        [When(@"I enter ""(.*)"" in the text box")]
        public void WhenIEnterInTheTextBox(string text)
        {
            _driver.FindElement(By.Id("sampletodotext")).SendKeys(text);
            Console.WriteLine($"Entered text: {text}");
        }

        [When(@"I click the add button")]
        public void WhenIClickTheAddButton()
        {
            _driver.FindElement(By.Id("addbutton")).Click();
            Console.WriteLine("Clicked add button");
        }

        [Then(@"I should see ""(.*)"" in the list")]
        public void ThenIShouldSeeInTheList(string expectedText)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var lastItem = wait.Until(d =>
            {
                var items = d.FindElements(By.CssSelector("ul.list-unstyled li span.ng-binding"));
                if (items.Count == 0) return null;
                var last = items[items.Count - 1];
                return last.Text.Trim() == expectedText ? last : null;
            });
            
            Assert.That(lastItem, Is.Not.Null, $"Expected to find '{expectedText}' in the list");
            Assert.That(lastItem!.Text.Trim(), Is.EqualTo(expectedText));
            Console.WriteLine($"Verified '{expectedText}' appears in the list");
        }
    }
}
