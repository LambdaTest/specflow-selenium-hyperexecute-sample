using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Specialized;
using TechTalk.SpecFlow.Tracing;
using System.IO;
using System.Reflection;
using BoDi;

using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;

namespace SpecFlowLambdaSample
{
    [Binding]
    public sealed class Hooks
    {
        private LambdaTestDriver LTDriver;
        private string[] tags;
        private ScenarioContext _scenarioContext;
        private readonly IObjectContainer _objectContainer;

        static string configTheme = "standard";
        static string configReportPath = "Report//index.html";

        [ThreadStatic]
        private static ExtentTest feature;
        [ThreadStatic]
        private static ExtentTest scenario;
        private static ExtentReports extentReport;
        private static readonly string base64ImageType = "base64";

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(configReportPath);

            switch (configTheme.ToLower())
            {
                case "dark":
                    htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
                    break;
                case "standard":
                default:
                    htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
                    break;
            }

            extentReport = new ExtentReports();
            extentReport.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            extentReport.Flush();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            feature = extentReport.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext ScenarioContext)
        {
            _scenarioContext = ScenarioContext;
            LTDriver = new LambdaTestDriver(ScenarioContext);
            ScenarioContext["LTDriver"] = LTDriver;
            _objectContainer.RegisterInstanceAs<LambdaTestDriver>(LTDriver);

            scenario = feature.CreateNode<Scenario>(ScenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext ScenarioContext)
        {
            String screenShotPath, fileName;
            /* Create a folder with the Scenario Title */
            String scenario_path = ScenarioContext.ScenarioInfo.Title;

            DateTime time = DateTime.Now;
            fileName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";

            /* Take the scenario screenshot */
            screenShotPath = LTDriver.Capture(scenario_path, fileName);
            /* Capturing Screenshots using built-in methods in ExtentReports 4 */

            var mediaEntity = LTDriver.CaptureScreenShot(fileName);

            /* Usage of MediaEntityBuilder for capturing screenshots */
            scenario.Pass("Scenario Execution Status", mediaEntity);

            /* Usage of traditional approach for capturing screenshots */
            scenario.Log(Status.Info, "Snapshot below: " + feature.AddScreenCaptureFromPath("Screenshots//" + screenShotPath + fileName));

            LTDriver.Cleanup();
        }

        [AfterStep]
        public void InsertReportingSteps(ScenarioContext ScenarioContext)
        {
            string stepType = ScenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepInfo = ScenarioContext.StepContext.StepInfo.Text;

            string resultOfImplementation = ScenarioContext.ScenarioExecutionStatus.ToString();


            if (ScenarioContext.TestError == null && resultOfImplementation == "OK")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepInfo);
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepInfo);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepInfo);
                else if (stepType == "And")
                    scenario.CreateNode<And>(stepInfo);
                else if (stepType == "But")
                    scenario.CreateNode<And>(stepInfo);
            }
            else if (ScenarioContext.TestError != null)
            {
                Exception innerException = ScenarioContext.TestError.InnerException;
                string testError = ScenarioContext.TestError.Message;

                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepInfo).Fail(innerException, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepInfo).Fail(innerException, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepInfo).Fail(testError, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "And")
                    scenario.CreateNode<Then>(stepInfo).Fail(testError, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "But")
                    scenario.CreateNode<Then>(stepInfo).Fail(testError, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());

            }
            else if (resultOfImplementation == "StepDefinitionPending")
            {
                string errorMessage = "Step Definition is not implemented!";

                if (stepType == "Given")
                    scenario.CreateNode<Given>(stepInfo).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "When")
                    scenario.CreateNode<When>(stepInfo).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(stepInfo).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());
                else if (stepType == "But")
                    scenario.CreateNode<Then>(stepInfo).Fail(errorMessage, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64ImageType).Build());

            }
        }
    }
    
    public class LambdaTestDriver
    {
        private IWebDriver driver;
        private string profile;
        private string environment;
        private ScenarioContext ScenarioContext;

        public LambdaTestDriver(ScenarioContext ScenarioContext)
        {
            this.ScenarioContext = ScenarioContext;
        }

        public IWebDriver Init(string profile, string environment)
        {
            NameValueCollection caps = ConfigurationManager.GetSection("capabilities/" + profile) as NameValueCollection;
            NameValueCollection settings = ConfigurationManager.GetSection("environments/" + environment) as NameValueCollection;
            DesiredCapabilities capability = new DesiredCapabilities();

            foreach (string key in caps.AllKeys)
            {
                capability.SetCapability(key, caps[key]);
            }

            foreach (string key in settings.AllKeys)
            {
                capability.SetCapability(key, settings[key]);
            }

            String username = Environment.GetEnvironmentVariable("LT_USERNAME");
            if (username == null)
            {
                username = ConfigurationManager.AppSettings.Get("username");
            }

            String accesskey = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
            if (accesskey == null)
            {
                accesskey = ConfigurationManager.AppSettings.Get("accesskey");
            }

            driver = new RemoteWebDriver(new Uri("https://" + username + ":" + accesskey + "@hub.lambdatest.com/wd/hub/"), capability, TimeSpan.FromSeconds(600));
            return driver;
        }

        public IWebDriver InitLocal(String build, String name, String platform, String browserName, String version)
        {
            String username, accesskey;
            String grid_url = "@hub.lambdatest.com";

            DesiredCapabilities capability = new DesiredCapabilities();

            username = Environment.GetEnvironmentVariable("LT_USERNAME");
            if (username == null)
            {
                username = ConfigurationManager.AppSettings.Get("username");
            }

            accesskey = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
            if (accesskey == null)
            {
                accesskey = ConfigurationManager.AppSettings.Get("accesskey");
            }

            driver = new RemoteWebDriver(new Uri("http://" + username + ":" + accesskey + grid_url + "/wd/hub/"), capability, TimeSpan.FromSeconds(600));
            return driver;
        }

        public void Cleanup()
        {
            /* Since the scenario screenshot has to be captured, the session is ended here */
            /* This is after the screenshot is taken */
            bool passed = TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed;

            var status = passed ? "passed" : "failed";

            ((IJavaScriptExecutor)driver).ExecuteScript($"lambda-status={status}");
            driver.Close();
            driver.Quit();
        }

        public string Capture(String scenario_path, String screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();

            var pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            var actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            var reportPath = new Uri(actualPath).LocalPath;

            Directory.CreateDirectory(reportPath + "//Screenshots//" + scenario_path);
            var finalpth = pth.Substring(0, pth.LastIndexOf("bin")) + "//Screenshots//" +
                                         scenario_path + "//" + screenShotName;
            var localpath = new Uri(finalpth).LocalPath;
            screenshot.SaveAsFile(localpath, ScreenshotImageFormat.Png);
            return reportPath;
        }

        public MediaEntityModelProvider CaptureScreenShot(String screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot().AsBase64EncodedString;

            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
        }
    }
}