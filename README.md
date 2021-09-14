# SpecFlowExtentReportsSample
Sample Spec Flow Extent Reports

## Referências

```xml
    <PackageReference Include="ExtentReports" Version="4.1.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.5.0" />
    <PackageReference Include="SpecFlow.Plus.LivingDocPlugin" Version="3.9.5" /> 
    <PackageReference Include="SpecFlow.MsTest" Version="3.9.8" />
    <PackageReference Include="MSTest.TestAdapter" Version="2.1.2" />
    <PackageReference Include="MSTest.TestFramework" Version="2.1.2" /> 
    <PackageReference Include="FluentAssertions" Version="5.10.3" /> 
```

## Base class

```csharp
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
```

## Classe do SpecFlow

```csharp
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
```
