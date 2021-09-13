using AventStack.ExtentReports.Gherkin.Model;
using FluentAssertions;
using SpecFlowProject1.Bases;
using SpecFlowProject1.Services;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.Steps
{
    [Binding]
    public sealed class CalculatorStepDefinitions : ExtentBase
    { 
        #region Extent Reports

        [BeforeTestRun]
        public static void BeforeTestRun() => BeforeTestRunBase(nameof(CalculatorStepDefinitions));

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext) => BeforeFeatureBase(featureContext);

        [BeforeScenario]
        public void BeforeScenario() => BeforeScenarioBase();

        [AfterStep]
        public void InsertReportingSteps() => InsertReportingStepsBase();

        [AfterTestRun]
        public static void AfterTestRun() => AfterTestRunBase();

        #endregion

        #region SpecFlow

        private readonly CalculatorService _calculatorService;
        private int resultado;

        public CalculatorStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _calculatorService = new CalculatorService();
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            _calculatorService.Number1 = number;
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            _calculatorService.Number2 = number;
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            resultado = _calculatorService.Add();
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            result.Should().Be(resultado);
        }

        #endregion
    }
}
