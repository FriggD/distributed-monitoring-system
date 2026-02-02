using FluentAssertions;
using MonitorService.Application.DTOs;
using MonitorService.Application.Validators;

namespace MonitorService.UnitTests.Validators;

public class CreateMetricValidatorTests
{
    private readonly CreateMetricValidator _validator;

    public CreateMetricValidatorTests()
    {
        _validator = new CreateMetricValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldPass()
    {
        var dto = new CreateMetricDto
        {
            Name = "CPU Usage",
            Value = 75.5,
            Unit = "%",
            Source = "server-01"
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithInvalidName_ShouldFail(string? name)
    {
        var dto = new CreateMetricDto
        {
            Name = name!,
            Value = 75.5,
            Unit = "%",
            Source = "server-01"
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateMetricDto.Name));
    }

    [Fact]
    public void Validate_WithNameTooLong_ShouldFail()
    {
        var dto = new CreateMetricDto
        {
            Name = new string('A', 101),
            Value = 75.5,
            Unit = "%",
            Source = "server-01"
        };

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
    }
}
