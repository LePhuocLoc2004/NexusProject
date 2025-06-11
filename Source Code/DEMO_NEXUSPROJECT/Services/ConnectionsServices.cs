using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface ConnectionsServices
{
    public Connection CreateConnection(int customerId, string connectionType, int servicePacake);
}
