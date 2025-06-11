using DEMO_NEXUSPROJECT.Models;
using System.Diagnostics;

namespace DEMO_NEXUSPROJECT.Services;

public class ConnectionRequestServiceimpl : ConnectionRequestService
{
	private DatabaseContext db;

	public ConnectionRequestServiceimpl(DatabaseContext _db)
	{
		db = _db;
	}
	public ConnectionRequest CreateConnectionRequest(int customerId, string connectionType )
	{
		try
		{
			var connectionRequest = new ConnectionRequest
			{
				CustomerId = customerId,
				ConnectionType = connectionType,
				RequestDate = DateTime.Now,
				Status = "Pending"
			};

			db.ConnectionRequests.Add(connectionRequest);
			db.SaveChanges();

			return connectionRequest;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error occurred while creating connection request: {ex.Message}");
			throw;
		}
	}
}
