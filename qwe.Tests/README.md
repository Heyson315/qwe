# HHR CPA Tests

This project contains unit tests for the HHR CPA website.

## Running Tests

### Visual Studio
1. Open Test Explorer (Test > Test Explorer)
2. Click "Run All" to execute all tests

### Command Line
```bash
dotnet test
```

## Test Structure

- **Services/** - Tests for business logic services
- **Configuration/** - Tests for configuration management
- **Controllers/** - Tests for API controllers (to be added)
- **Utilities/** - Tests for utility classes (to be added)

## Writing Tests

Follow the AAA pattern:
- **Arrange** - Set up test data and conditions
- **Act** - Execute the code being tested
- **Assert** - Verify the results

Example:
```csharp
[TestMethod]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var service = new MyService();
    
    // Act
    var result = service.DoSomething();
    
    // Assert
    Assert.IsNotNull(result);
}
```

## Coverage Goals

- Aim for 80%+ code coverage
- Focus on testing business logic
- Test edge cases and error conditions
