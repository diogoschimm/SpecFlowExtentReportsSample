using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.Bases
{
    public abstract class ExtentBase
    {
        protected readonly ScenarioContext _scenarioContext;

        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;

        public static string ReportPath;

        public ExtentBase(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        public static void BeforeTestRunBase(string testName)
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}\\TestsResults\\{testName}\\index.html";

            var htmlReporter = new ExtentHtmlReporter(path, AventStack.ExtentReports.Reporter.Configuration.ViewStyle.SPA);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            htmlReporter.Config.DocumentTitle = $"Tests {testName}";
            
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        public static void BeforeFeatureBase(FeatureContext featureContext)
        {
            featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        public void BeforeScenarioBase()
        {
            scenario = featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
        }

        public void InsertReportingStepsBase()
        {
            var currentScenarioStepContext = ScenarioStepContext.Current;
            var stepType = currentScenarioStepContext.StepInfo.StepDefinitionType.ToString();

            if (_scenarioContext.TestError == null)
                CreateScenaryNode(stepType, currentScenarioStepContext.StepInfo.Text);

            else if (_scenarioContext.TestError != null)
                CreateScenaryNode(stepType, currentScenarioStepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message);
        }

        public static void AfterTestRunBase() => extent.Flush();

        private ExtentTest CreateScenaryNode(string stepType, string name)
        {
            return scenario.CreateNode(new GherkinKeyword(stepType), name);
        }
    }
}
