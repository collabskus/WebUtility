namespace WebUtility;

public static class Calculator
{
    public static int CalculateFiscalYear(DateTime inputDate)
    {
        if (inputDate == DateTime.MinValue)
            return 0;
        return inputDate.Month < 10 ? inputDate.Year : inputDate.Year + 1;
    }
}
