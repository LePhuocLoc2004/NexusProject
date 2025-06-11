using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface ConnectionRequestService
{
	public ConnectionRequest CreateConnectionRequest(int customerId, string connectionType);
}
