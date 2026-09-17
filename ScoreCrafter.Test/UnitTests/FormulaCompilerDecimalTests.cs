using System;
using System.Collections.Generic;
using System.Text;

using ScoreCrafter.Domain.ValueObjects.FormulaEngin;

namespace ScoreCrafter.Test.UnitTests;

public class FormulaCompilerDecimalTests
{
    [Fact]
    public void Should_Evaluate_Arithmetic_Expression()
    {
        var formula = FormulaCompiler.Build("(PurchaseAmount / 60) * 2 + 100");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 6_000,
                PurchaseCount: 1,
                default));

        Assert.Equal(FormulaEvaluationResult.FormulaResultType.Decimal, result.ResultType);
        Assert.Equal(300m, result.AsDecimal());
    }

    [Fact]
    public void Should_Evaluate_Conditional_Bonus()
    {
        var formula = FormulaCompiler.Build(
            "(PurchaseAmount / 60) * " +
            "(1 + IF(PurchaseCount >= 5, 0.10, 0))");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 60_000,
                PurchaseCount: 5,
                default));

        Assert.Equal(FormulaEvaluationResult.FormulaResultType.Decimal, result.ResultType);
        Assert.Equal(1_100m, result.AsDecimal());
    }

    [Fact]
    public void Should_Evaluate_Tiered_Purchase_Bonus()
    {
        var formula = FormulaCompiler.Build(
            "(PurchaseAmount / 60) * " +
            "(1 + IF(PurchaseAmount > 10000000, " +
            "0.05 + FLOOR((PurchaseAmount - 10000000) / 10000000) * 0.025, 0))");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 35_000_000,
                PurchaseCount: 1,
                default));

        // 35M / 60 = 583,333.333...
        // Bonus = 5% + (2 * 2.5%) = 10%
        // Final = 641,666.666...
        Assert.Equal(FormulaEvaluationResult.FormulaResultType.Decimal, result.ResultType);
        Assert.Equal(
            641666.66666666666666666666666m,
            result.AsDecimal());
    }

    [Fact]
    public void Should_Reject_Division_By_Zero()
    {
        var formula = FormulaCompiler.Build(
            "PurchaseAmount / 0");

        var exception = Assert.Throws<FormulaEvaluationException>(
            () => formula.Evaluate(
                new FormulaContext(
                    PurchaseAmount: 1000,
                    PurchaseCount: 1,
                default)));

        Assert.Contains("Division by zero", exception.Message);
    }
    [Fact]
    public void Should_Respect_Mathematical_Operator_Precedence()
    {
        var formula = FormulaCompiler.Build(
            "PurchaseAmount + 10 * 2");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 100,
                PurchaseCount: 1,
                default));
        Assert.Equal(FormulaEvaluationResult.FormulaResultType.Decimal, result.ResultType);
        Assert.Equal(120m, result.AsDecimal());
    }
    [Fact]
    public void Complex_Formula()
    {
        var formula = FormulaCompiler.Build(
            @"(
    PurchaseAmount /
    IF(
        CustomerType == 1,
        100,
        IF(
            CustomerType == 2,
            80,
            60
        )
    )
)
*
(
    1
    + IF(
        PurchaseCount >= 5,
        0.10,
        0
      )
    + IF(
        PurchaseAmount > 10000000,
        0.05
        + FLOOR(
            (PurchaseAmount - 10000000) / 10000000
          ) * 0.025,
        0
      )
)");

        var result = formula.Evaluate(
            new FormulaContext(
                PurchaseAmount: 60_000_000,
                PurchaseCount: 5,
                CustomerType: 3));
        Assert.Equal(FormulaEvaluationResult.FormulaResultType.Decimal, result.ResultType);
        Assert.Equal(1_275_000m, result.AsDecimal());
    }
}