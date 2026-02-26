namespace WebUtility.Tests;

public class CalculatorTests
{
    // =========================================================
    // DateTime.MinValue special case
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_DateTimeMinValue_ReturnsZero()
    {
        Assert.Equal(0, Calculator.CalculateFiscalYear(DateTime.MinValue));
    }

    [Fact]
    public void CalculateFiscalYear_ExplicitEquivalentOfMinValue_ReturnsZero()
    {
        var equivalent = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
        Assert.Equal(0, Calculator.CalculateFiscalYear(equivalent));
    }

    [Fact]
    public void CalculateFiscalYear_MinValueWithUtcKind_ReturnsZero()
    {
        // DateTime == ignores Kind and compares only Ticks, so this equals DateTime.MinValue
        var utcMin = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        Assert.Equal(0, Calculator.CalculateFiscalYear(utcMin));
    }

    [Fact]
    public void CalculateFiscalYear_MinValueWithLocalKind_ReturnsZero()
    {
        var localMin = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Local);
        Assert.Equal(0, Calculator.CalculateFiscalYear(localMin));
    }

    [Fact]
    public void CalculateFiscalYear_OnlyMinValueReturnsZero_OnTickLater_ReturnsOne()
    {
        // One tick after MinValue is no longer MinValue; month is still January (< 10) → year 1
        var oneTick = DateTime.MinValue.AddTicks(1);
        Assert.Equal(1, Calculator.CalculateFiscalYear(oneTick));
    }

    [Fact]
    public void CalculateFiscalYear_MinValueResult_IsExactlyZeroNotNegative()
    {
        int result = Calculator.CalculateFiscalYear(DateTime.MinValue);
        Assert.Equal(0, result);
        Assert.False(result < 0);
    }

    // =========================================================
    // Month boundary: September (last month < 10) → same year
    //                 October  (first month >= 10) → next year
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_September30_ReturnsSameYear()
    {
        Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 9, 30)));
    }

    [Fact]
    public void CalculateFiscalYear_October1_ReturnsNextYear()
    {
        Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 10, 1)));
    }

    [Fact]
    public void CalculateFiscalYear_Sep30AndOct1_DifferByExactlyOne()
    {
        int sep = Calculator.CalculateFiscalYear(new DateTime(2023, 9, 30));
        int oct = Calculator.CalculateFiscalYear(new DateTime(2023, 10, 1));
        Assert.Equal(1, oct - sep);
    }

    // =========================================================
    // All twelve months (Theory)
    // =========================================================

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void CalculateFiscalYear_MonthsJanuaryToSeptember_ReturnsSameCalendarYear(int month)
    {
        var date = new DateTime(2023, month, 1);
        Assert.Equal(2023, Calculator.CalculateFiscalYear(date));
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(12)]
    public void CalculateFiscalYear_MonthsOctoberToDecember_ReturnsNextCalendarYear(int month)
    {
        var date = new DateTime(2023, month, 1);
        Assert.Equal(2024, Calculator.CalculateFiscalYear(date));
    }

    // =========================================================
    // Each month individually (Fact) — exhaustive explicit coverage
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_January_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 1, 15)));

    [Fact]
    public void CalculateFiscalYear_February_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 2, 15)));

    [Fact]
    public void CalculateFiscalYear_March_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 3, 15)));

    [Fact]
    public void CalculateFiscalYear_April_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 4, 15)));

    [Fact]
    public void CalculateFiscalYear_May_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 5, 15)));

    [Fact]
    public void CalculateFiscalYear_June_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 6, 15)));

    [Fact]
    public void CalculateFiscalYear_July_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 7, 15)));

    [Fact]
    public void CalculateFiscalYear_August_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 8, 15)));

    [Fact]
    public void CalculateFiscalYear_September_ReturnsSameYear()
        => Assert.Equal(2022, Calculator.CalculateFiscalYear(new DateTime(2022, 9, 15)));

    [Fact]
    public void CalculateFiscalYear_October_ReturnsNextYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2022, 10, 15)));

    [Fact]
    public void CalculateFiscalYear_November_ReturnsNextYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2022, 11, 15)));

    [Fact]
    public void CalculateFiscalYear_December_ReturnsNextYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2022, 12, 15)));

    // =========================================================
    // First and last day of every month
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_January1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 1, 1)));

    [Fact]
    public void CalculateFiscalYear_January31_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 1, 31)));

    [Fact]
    public void CalculateFiscalYear_February1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 2, 1)));

    [Fact]
    public void CalculateFiscalYear_February28_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 2, 28)));

    [Fact]
    public void CalculateFiscalYear_March1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 3, 1)));

    [Fact]
    public void CalculateFiscalYear_March31_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 3, 31)));

    [Fact]
    public void CalculateFiscalYear_April1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 4, 1)));

    [Fact]
    public void CalculateFiscalYear_April30_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 4, 30)));

    [Fact]
    public void CalculateFiscalYear_May1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 5, 1)));

    [Fact]
    public void CalculateFiscalYear_May31_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 5, 31)));

    [Fact]
    public void CalculateFiscalYear_June1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 6, 1)));

    [Fact]
    public void CalculateFiscalYear_June30_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 6, 30)));

    [Fact]
    public void CalculateFiscalYear_July1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 7, 1)));

    [Fact]
    public void CalculateFiscalYear_July31_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 7, 31)));

    [Fact]
    public void CalculateFiscalYear_August1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 8, 1)));

    [Fact]
    public void CalculateFiscalYear_August31_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 8, 31)));

    [Fact]
    public void CalculateFiscalYear_September1_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 9, 1)));

    [Fact]
    public void CalculateFiscalYear_September30_LastDayBeforeFiscalBoundary_ReturnsSameYear()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 9, 30)));

    [Fact]
    public void CalculateFiscalYear_October1_FirstDayAfterFiscalBoundary_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 10, 1)));

    [Fact]
    public void CalculateFiscalYear_October31_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 10, 31)));

    [Fact]
    public void CalculateFiscalYear_November1_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 11, 1)));

    [Fact]
    public void CalculateFiscalYear_November30_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 11, 30)));

    [Fact]
    public void CalculateFiscalYear_December1_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 12, 1)));

    [Fact]
    public void CalculateFiscalYear_December31_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 12, 31)));

    // =========================================================
    // Time-of-day does not affect the result
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_Midnight_DoesNotAffectResult()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 5, 10, 0, 0, 0)));

    [Fact]
    public void CalculateFiscalYear_Noon_DoesNotAffectResult()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 5, 10, 12, 0, 0)));

    [Fact]
    public void CalculateFiscalYear_EndOfDay_DoesNotAffectResult()
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 5, 10, 23, 59, 59, 999)));

    [Fact]
    public void CalculateFiscalYear_SameDateDifferentTimes_ReturnSameResult()
    {
        var morning = new DateTime(2023, 11, 15, 6, 0, 0);
        var evening = new DateTime(2023, 11, 15, 22, 30, 0);
        Assert.Equal(Calculator.CalculateFiscalYear(morning), Calculator.CalculateFiscalYear(evening));
    }

    [Fact]
    public void CalculateFiscalYear_OctoberMidnight_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 10, 15, 0, 0, 0)));

    [Fact]
    public void CalculateFiscalYear_OctoberEndOfDay_ReturnsNextYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 10, 15, 23, 59, 59)));

    // =========================================================
    // Day-of-month does not affect result (only month matters)
    // =========================================================

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(28)]
    public void CalculateFiscalYear_DifferentDaysInJune_AllReturnSameYear(int day)
        => Assert.Equal(2023, Calculator.CalculateFiscalYear(new DateTime(2023, 6, day)));

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(31)]
    public void CalculateFiscalYear_DifferentDaysInOctober_AllReturnNextYear(int day)
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2023, 10, day)));

    // =========================================================
    // Leap-year dates
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_LeapYearFebruary29_ReturnsSameYear()
        => Assert.Equal(2024, Calculator.CalculateFiscalYear(new DateTime(2024, 2, 29)));

    [Fact]
    public void CalculateFiscalYear_LeapYearOctober_ReturnsNextYear()
        => Assert.Equal(2025, Calculator.CalculateFiscalYear(new DateTime(2024, 10, 1)));

    [Fact]
    public void CalculateFiscalYear_LeapYearDecember31_ReturnsNextYear()
        => Assert.Equal(2025, Calculator.CalculateFiscalYear(new DateTime(2024, 12, 31)));

    [Fact]
    public void CalculateFiscalYear_Century_NonLeapYear_February28_ReturnsSameYear()
        // 1900 is NOT a leap year (divisible by 100 but not 400)
        => Assert.Equal(1900, Calculator.CalculateFiscalYear(new DateTime(1900, 2, 28)));

    [Fact]
    public void CalculateFiscalYear_Century_LeapYear_February29_ReturnsSameYear()
        // 2000 IS a leap year (divisible by 400)
        => Assert.Equal(2000, Calculator.CalculateFiscalYear(new DateTime(2000, 2, 29)));

    // =========================================================
    // Extreme valid dates (year 1 and year 9999)
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_Year1_January2_ReturnsYear1()
    {
        // Jan 1 yr 1 is DateTime.MinValue; Jan 2 yr 1 is not
        Assert.Equal(1, Calculator.CalculateFiscalYear(new DateTime(1, 1, 2)));
    }

    [Fact]
    public void CalculateFiscalYear_Year1_September30_ReturnsYear1()
        => Assert.Equal(1, Calculator.CalculateFiscalYear(new DateTime(1, 9, 30)));

    [Fact]
    public void CalculateFiscalYear_Year1_October1_ReturnsYear2()
        => Assert.Equal(2, Calculator.CalculateFiscalYear(new DateTime(1, 10, 1)));

    [Fact]
    public void CalculateFiscalYear_Year1_December31_ReturnsYear2()
        => Assert.Equal(2, Calculator.CalculateFiscalYear(new DateTime(1, 12, 31)));

    [Fact]
    public void CalculateFiscalYear_Year9999_January1_Returns9999()
        => Assert.Equal(9999, Calculator.CalculateFiscalYear(new DateTime(9999, 1, 1)));

    [Fact]
    public void CalculateFiscalYear_Year9999_September30_Returns9999()
        => Assert.Equal(9999, Calculator.CalculateFiscalYear(new DateTime(9999, 9, 30)));

    [Fact]
    public void CalculateFiscalYear_Year9999_October1_Returns10000()
        => Assert.Equal(10000, Calculator.CalculateFiscalYear(new DateTime(9999, 10, 1)));

    [Fact]
    public void CalculateFiscalYear_DateTimeMaxValue_Returns10000()
    {
        // DateTime.MaxValue = Dec 31, 9999 → month 12 ≥ 10 → 9999 + 1 = 10000
        Assert.Equal(10000, Calculator.CalculateFiscalYear(DateTime.MaxValue));
    }

    // =========================================================
    // Return value is non-negative for all non-MinValue inputs
    // =========================================================

    [Theory]
    [InlineData(2000, 1, 1)]
    [InlineData(2000, 9, 30)]
    [InlineData(2000, 10, 1)]
    [InlineData(2000, 12, 31)]
    [InlineData(1, 1, 2)]     // just past MinValue
    [InlineData(9999, 12, 31)]
    public void CalculateFiscalYear_AnyNonMinValueDate_ReturnsPositiveNumber(int year, int month, int day)
    {
        int result = Calculator.CalculateFiscalYear(new DateTime(year, month, day));
        Assert.True(result > 0);
    }

    // =========================================================
    // Fiscal-year consistency: dates in the same fiscal year
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_JanuaryAndSeptemberSameCalendarYear_SameFiscalYear()
    {
        int jan = Calculator.CalculateFiscalYear(new DateTime(2022, 1, 1));
        int sep = Calculator.CalculateFiscalYear(new DateTime(2022, 9, 30));
        Assert.Equal(jan, sep);
    }

    [Fact]
    public void CalculateFiscalYear_OctoberAndDecemberSameCalendarYear_SameFiscalYear()
    {
        int oct = Calculator.CalculateFiscalYear(new DateTime(2022, 10, 1));
        int dec = Calculator.CalculateFiscalYear(new DateTime(2022, 12, 31));
        Assert.Equal(oct, dec);
    }

    [Fact]
    public void CalculateFiscalYear_OctoberY_AndJanuaryYPlus1_SameFiscalYear()
    {
        // Oct 2022 (FY2023) and Jan 2023 (FY2023) belong to the same fiscal year
        int oct2022 = Calculator.CalculateFiscalYear(new DateTime(2022, 10, 1));
        int jan2023 = Calculator.CalculateFiscalYear(new DateTime(2023, 1, 1));
        Assert.Equal(jan2023, oct2022);
    }

    [Fact]
    public void CalculateFiscalYear_OctoberY_AndSeptemberYPlus1_SameFiscalYear()
    {
        // Oct 2022 → FY2023; Sep 2023 → FY2023
        int oct = Calculator.CalculateFiscalYear(new DateTime(2022, 10, 1));
        int sep = Calculator.CalculateFiscalYear(new DateTime(2023, 9, 30));
        Assert.Equal(oct, sep);
    }

    [Fact]
    public void CalculateFiscalYear_DecemberY_AndSeptemberYPlus1_SameFiscalYear()
    {
        int dec = Calculator.CalculateFiscalYear(new DateTime(2021, 12, 31));
        int sep = Calculator.CalculateFiscalYear(new DateTime(2022, 9, 30));
        Assert.Equal(dec, sep);
    }

    // =========================================================
    // Return value equals Year or Year+1 — never anything else
    // =========================================================

    [Theory]
    [InlineData(2023, 1)]
    [InlineData(2023, 5)]
    [InlineData(2023, 9)]
    public void CalculateFiscalYear_MonthBefore10_ResultEqualsInputYear(int year, int month)
    {
        int result = Calculator.CalculateFiscalYear(new DateTime(year, month, 1));
        Assert.Equal(year, result);
    }

    [Theory]
    [InlineData(2023, 10)]
    [InlineData(2023, 11)]
    [InlineData(2023, 12)]
    public void CalculateFiscalYear_MonthFrom10_ResultEqualsInputYearPlusOne(int year, int month)
    {
        int result = Calculator.CalculateFiscalYear(new DateTime(year, month, 1));
        Assert.Equal(year + 1, result);
    }

    // =========================================================
    // Multiple years (stability across different calendar years)
    // =========================================================

    [Theory]
    [InlineData(2018)]
    [InlineData(2019)]
    [InlineData(2020)]
    [InlineData(2021)]
    [InlineData(2022)]
    [InlineData(2023)]
    [InlineData(2024)]
    public void CalculateFiscalYear_JanuaryAcrossMultipleYears_AlwaysReturnsSameYear(int year)
        => Assert.Equal(year, Calculator.CalculateFiscalYear(new DateTime(year, 1, 1)));

    [Theory]
    [InlineData(2018)]
    [InlineData(2019)]
    [InlineData(2020)]
    [InlineData(2021)]
    [InlineData(2022)]
    [InlineData(2023)]
    [InlineData(2024)]
    public void CalculateFiscalYear_DecemberAcrossMultipleYears_AlwaysReturnsYearPlusOne(int year)
        => Assert.Equal(year + 1, Calculator.CalculateFiscalYear(new DateTime(year, 12, 1)));

    // =========================================================
    // Return type
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_ReturnType_IsInt()
    {
        object result = Calculator.CalculateFiscalYear(new DateTime(2023, 6, 1));
        Assert.IsType<int>(result);
    }

    [Fact]
    public void CalculateFiscalYear_MinValueReturnType_IsInt()
    {
        object result = Calculator.CalculateFiscalYear(DateTime.MinValue);
        Assert.IsType<int>(result);
    }

    // =========================================================
    // Pure idempotency / determinism
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_CalledTwiceWithSameInput_ReturnsSameResult()
    {
        var date = new DateTime(2023, 11, 5);
        Assert.Equal(
            Calculator.CalculateFiscalYear(date),
            Calculator.CalculateFiscalYear(date));
    }

    [Fact]
    public void CalculateFiscalYear_MinValueCalledTwice_ReturnsSameResult()
    {
        Assert.Equal(
            Calculator.CalculateFiscalYear(DateTime.MinValue),
            Calculator.CalculateFiscalYear(DateTime.MinValue));
    }


    // =========================================================
    // FULL DOMAIN INVARIANT TESTS
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_ForAllYears_FirstDayOfEachMonth_IsCorrect()
    {
        for (int year = 1; year <= 9999; year++)
        {
            for (int month = 1; month <= 12; month++)
            {
                var date = new DateTime(year, month, 1);

                int result = Calculator.CalculateFiscalYear(date);

                if (date == DateTime.MinValue)
                {
                    Assert.Equal(0, result);
                }
                else if (month < 10)
                {
                    Assert.Equal(year, result);
                }
                else
                {
                    Assert.Equal(year + 1, result);
                }
            }
        }
    }

    // =========================================================
    // FULL FISCAL BOUNDARY SWEEP
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_AllYears_September30_And_October1_AreCorrect()
    {
        for (int year = 1; year <= 9999; year++)
        {
            var sep30 = new DateTime(year, 9, 30);
            var oct1 = new DateTime(year, 10, 1);

            int sepResult = Calculator.CalculateFiscalYear(sep30);
            int octResult = Calculator.CalculateFiscalYear(oct1);

            Assert.Equal(year, sepResult);
            Assert.Equal(year + 1, octResult);
        }
    }

    // =========================================================
    // RANDOMIZED STRESS TEST (DETERMINISTIC)
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_RandomizedStressTest()
    {
        var random = new Random(123456);

        for (int i = 0; i < 100000; i++)
        {
            int year = random.Next(1, 10000);
            int month = random.Next(1, 13);
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
            int hour = random.Next(0, 24);
            int minute = random.Next(0, 60);
            int second = random.Next(0, 60);

            var date = new DateTime(year, month, day, hour, minute, second);

            int result = Calculator.CalculateFiscalYear(date);

            if (date == DateTime.MinValue)
            {
                Assert.Equal(0, result);
            }
            else if (month < 10)
            {
                Assert.Equal(year, result);
            }
            else
            {
                Assert.Equal(year + 1, result);
            }
        }
    }

    // =========================================================
    // MULTIPLE TICK OFFSETS FROM MINVALUE
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_FirstThousandTicksAfterMinValue_AreYearOne()
    {
        for (int i = 1; i <= 1000; i++)
        {
            var date = DateTime.MinValue.AddTicks(i);
            int result = Calculator.CalculateFiscalYear(date);
            Assert.Equal(1, result);
        }
    }

    // =========================================================
    // GLOBAL RESULT RANGE VALIDATION
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_ResultIsAlwaysWithinValidRange()
    {
        for (int year = 1; year <= 9999; year += 137) // stride to reduce runtime
        {
            for (int month = 1; month <= 12; month++)
            {
                var date = new DateTime(year, month, 15);
                int result = Calculator.CalculateFiscalYear(date);

                Assert.InRange(result, 0, 10000);
            }
        }
    }

    // =========================================================
    // RESULT MUST BE YEAR OR YEAR+1
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_ResultIsEitherYearOrYearPlusOne_ForAllYears()
    {
        for (int year = 1; year <= 9999; year += 97)
        {
            for (int month = 1; month <= 12; month++)
            {
                var date = new DateTime(year, month, 10);
                int result = Calculator.CalculateFiscalYear(date);

                if (date == DateTime.MinValue)
                {
                    Assert.Equal(0, result);
                }
                else
                {
                    Assert.True(result == year || result == year + 1);
                }
            }
        }
    }

    // =========================================================
    // VERIFY MONTH IS ONLY INFLUENCER
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_SameYearMonthDifferentEverythingElse_ReturnsSame()
    {
        var a = new DateTime(2023, 11, 1, 0, 0, 0, DateTimeKind.Utc);
        var b = new DateTime(2023, 11, 28, 23, 59, 59, DateTimeKind.Local);

        Assert.Equal(
            Calculator.CalculateFiscalYear(a),
            Calculator.CalculateFiscalYear(b));
    }

    // =========================================================
    // HIGH FREQUENCY CALL STABILITY
    // =========================================================

    [Fact]
    public void CalculateFiscalYear_HighVolumeInvocation_IsStable()
    {
        var date = new DateTime(2023, 10, 15);

        for (int i = 0; i < 1_000_000; i++)
        {
            Assert.Equal(2024, Calculator.CalculateFiscalYear(date));
        }
    }
}
