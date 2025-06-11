namespace DEMO_NEXUSPROJECT.Services;

public class IPServiceImpl : IPService
{
    private List<string> ips;
    public IPServiceImpl()
    {
        ips = new List<string>
        {
        "127.0.0.1","192.168.1.1" ,"::2"
        };
    }
    public bool isBlock(string ip)
    {
        return ips.Any(i => i.Equals(ip));
    }
}
