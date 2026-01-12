using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Safari;
using TechTalk.SpecFlow.Tracing;
using System.IO;
using System.Reflection;
using BoDi;
using Microsoft.Extensions.Configuration;

using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;
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
            // ExtentReports 5.x uses ExtentSparkReporter instead of ExtentHtmlReporter
            var sparkReporter = new ExtentSparkReporter(configReportPath);

            switch (configTheme.ToLower())
            {
                case "dark":
                    sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Dark;
                    break;
                case "standard":
                default:
                    sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;
                    break;
            }

            extentReport = new ExtentReports();
            extentReport.AttachReporter(sparkReporter);
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
        private static IConfiguration _configuration;

        static LambdaTestDriver()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public LambdaTestDriver(ScenarioContext ScenarioContext)
        {
            this.ScenarioContext = ScenarioContext;
        }

        public IWebDriver Init(string profile, string environment)
        {
            // Get capabilities and settings from configuration
            var capsSection = _configuration.GetSection($"capabilities:{profile}");
            var envSection = _configuration.GetSection($"environments:{environment}");

            // Selenium 4: Use browser-specific options instead of DesiredCapabilities
            var browserName = envSection["browserName"]?.ToLower() ?? "chrome";
            DriverOptions options = GetBrowserOptions(browserName);

            // Set LambdaTest options using the new W3C format
            var ltOptions = new Dictionary<string, object>();

            // Add capabilities from profile
            foreach (var cap in capsSection.GetChildren())
            {
                ltOptions[cap.Key] = cap.Value;
            }

            // Add environment settings
            ltOptions["browserName"] = envSection["browserName"];
            ltOptions["browserVersion"] = envSection["browserVersion"];
            ltOptions["platformName"] = envSection["platformName"];

            // Set LambdaTest options on the browser options
            options.AddAdditionalOption("LT:Options", ltOptions);

            String username = Environment.GetEnvironmentVariable("LT_USERNAME");
            if (string.IsNullOrEmpty(username))
            {
                username = _configuration["appSettings:username"];
            }

            String accesskey = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
            if (string.IsNullOrEmpty(accesskey))
            {
                accesskey = _configuration["appSettings:accesskey"];
            }

            // Selenium 4: RemoteWebDriver now takes DriverOptions directly
            driver = new RemoteWebDriver(
                new Uri($"https://{username}:{accesskey}@hub.lambdatest.com/wd/hub/"), 
                options.ToCapabilities(), 
                TimeSpan.FromSeconds(600));
            
            return driver;
        }

        private DriverOptions GetBrowserOptions(string browserName)
        {
            return browserName.ToLower() switch
            {
                "chrome" => new ChromeOptions(),
                "firefox" => new FirefoxOptions(),
                "edge" or "microsoftedge" => new EdgeOptions(),
                "safari" => new SafariOptions(),
                _ => new ChromeOptions()
            };
        }

        public IWebDriver InitLocal(String build, String name, String platform, String browserName, String version)
        {
            String username, accesskey;
            String grid_url = "@hub.lambdatest.com";

            // Selenium 4: Use browser-specific options
            DriverOptions options = GetBrowserOptions(browserName);
            
            var ltOptions = new Dictionary<string, object>
            {
                ["build"] = build,
                ["name"] = name,
                ["platformName"] = platform,
                ["browserName"] = browserName,
                ["browserVersion"] = version
            };

            options.AddAdditionalOption("LT:Options", ltOptions);

            username = Environment.GetEnvironmentVariable("LT_USERNAME");
            if (string.IsNullOrEmpty(username))
            {
                username = _configuration["appSettings:username"];
            }

            accesskey = Environment.GetEnvironmentVariable("LT_ACCESS_KEY");
            if (string.IsNullOrEmpty(accesskey))
            {
                accesskey = _configuration["appSettings:accesskey"];
            }

            driver = new RemoteWebDriver(
                new Uri($"http://{username}:{accesskey}{grid_url}/wd/hub/"), 
                options.ToCapabilities(), 
                TimeSpan.FromSeconds(600));
            
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

            var pth = Assembly.GetCallingAssembly().Location;
            var actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            var reportPath = actualPath;

            Directory.CreateDirectory(reportPath + "Screenshots" + Path.DirectorySeparatorChar + scenario_path);
            var finalpth = reportPath + "Screenshots" + Path.DirectorySeparatorChar +
                                         scenario_path + Path.DirectorySeparatorChar + screenShotName;
            screenshot.SaveAsFile(finalpth);
            return reportPath;
        }

        public Media CaptureScreenShot(String screenShotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot().AsBase64EncodedString;

            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenShotName).Build();
        }
    }
}
