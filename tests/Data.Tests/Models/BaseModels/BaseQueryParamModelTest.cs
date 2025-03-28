﻿using Data.DTOs.BaseModels;
using Data.DTOs.HistoricalPrice;
using JetBrains.Annotations;

namespace Data.Tests.Models.BaseModels;

[TestSubject(typeof(BaseQueryParamModel))]
public class BaseQueryParamModelTest
{
    [Fact]
    public void ToDictionary_WithBaseQueryParam_ShouldReturnDictionary()
    {
        var historicalPriceParams = new HistoricalPriceParams { Symbol = "USD", Interval = "s" };
        var result = historicalPriceParams.ToDictionary();
        Assert.Equal(2, result.Count());
        Assert.Equal("USD", result["symbol"]);
    }
}