using ScoreCrafter.Domain.ValueObjects;
using ScoreCrafter.Domain.ValueObjects.FormulaEngin;

namespace ScoreCrafter.Test.UnitTests;

public class FormulaCompilerBooleanTests
{
    [Fact]
    public void Should_Evaluate_Greater_Than_Comparison()
    {
        var formula = FormulaCompiler.Build(
            "PurchaseAmount > 10000000");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 15_000_000,
                PurchaseCount: 1));

        Assert.True(result.AsBoolean());
    }

    [Fact]
    public void Should_Evaluate_Logical_And()
    {
        var formula = FormulaCompiler.Build(
            "PurchaseAmount >= 10000000 AND PurchaseCount >= 5");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 15_000_000,
                PurchaseCount: 7));

        Assert.True(result.AsBoolean());
    }

    [Fact]
    public void Should_Evaluate_Logical_Or()
    {
        var formula = FormulaCompiler.Build(
            "PurchaseAmount >= 10000000 OR PurchaseCount >= 5");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 5_000_000,
                PurchaseCount: 7));

        Assert.True(result.AsBoolean());
    }

    [Fact]
    public void Should_Evaluate_Combined_Logical_Expression()
    {
        var formula = FormulaCompiler.Build(
            "(PurchaseAmount >= 10000000 AND PurchaseCount >= 5) " +
            "OR PurchaseAmount >= 50000000");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 5_000_000,
                PurchaseCount: 2));

        Assert.False(result.AsBoolean());
    }
    [Fact]
    public void Should_Return_False_When_Grade_Criteria_Is_Not_Met()
    {
        var formula = FormulaCompiler.Build(
            "PurchaseAmount >= 50000000 AND PurchaseCount >= 5");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 50_000_000,
                PurchaseCount: 4));

        Assert.False(result.AsBoolean());
    }
}
