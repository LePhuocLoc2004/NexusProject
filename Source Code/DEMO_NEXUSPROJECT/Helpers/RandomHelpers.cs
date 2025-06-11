namespace DEMO_NEXUSPROJECT.Helpers;

public class RandomHelpers
{
    public static string generteSecurityCode()
    {
        return Guid.NewGuid().ToString().Replace("_", "");
    }
}
