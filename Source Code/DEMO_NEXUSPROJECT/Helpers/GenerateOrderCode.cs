namespace DEMO_NEXUSPROJECT.Helpers;

public static class GenerateOrderCode
{
    public static string GenerateOrderCode1(string servicePackageType)
    {
        var random = new Random();
        var orderCode = servicePackageType + new string(Enumerable.Range(0, 10).Select(_ => random.Next(1, 10).ToString()[0]).ToArray());
        return orderCode;
    }

    public static string GenerateOrderCode2( )
    {
        var random = new Random();
        var orderCode =  new string(Enumerable.Range(0, 10).Select(_ => random.Next(1, 10).ToString()[0]).ToArray());
        return orderCode;
    }

    public static string GenerateOrderCode3(string servicePackageType, string cityCode)
    {
        var random = new Random();
        var randomNumberPart = new string(Enumerable.Range(0, 12).Select(_ => random.Next(1, 10).ToString()[0]).ToArray());
        var customerCode = $"{servicePackageType}{cityCode}{randomNumberPart}";

        // Ensure customerCode is exactly 16 characters
        if (customerCode.Length > 16)
        {
            customerCode = customerCode.Substring(0, 16); // Truncate if too long
        }
        else if (customerCode.Length < 16)
        {
            customerCode = customerCode.PadRight(16, '0'); // Pad with zeros if too short
        }

        return customerCode;
    }

}
