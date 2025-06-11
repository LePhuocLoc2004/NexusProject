namespace DEMO_NEXUSPROJECT.Services;

public interface MailServices
{
    public bool Send(string from, string to, string subject, string body);
}
