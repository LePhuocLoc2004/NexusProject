using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface ConnectionPackageService
{
	public ConnectionPackage CreateConnectionPackage(int connectionId, int servicePackageId, DateTime endDate);
}
