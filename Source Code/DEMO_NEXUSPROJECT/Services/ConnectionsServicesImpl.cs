using DEMO_NEXUSPROJECT.Models;
using System.Diagnostics;

namespace DEMO_NEXUSPROJECT.Services
{
	public class ConnectionsServicesImpl : ConnectionsServices
	{
		private readonly DatabaseContext db;
		private readonly ConnectionPackageService connectionPackageService;

		public ConnectionsServicesImpl(DatabaseContext _db, ConnectionPackageService connectionPackageService)
		{
			db = _db;
			this.connectionPackageService = connectionPackageService;
		}

		public Connection CreateConnection(int customerId, string connectionType, int servicePackageId)
		{
			try
			{
				var customer = db.Customers.SingleOrDefault(c => c.CustomerId == customerId);
				if (customer == null)
				{
					throw new Exception($"Customer with ID {customerId} not found.");
				}

				var accountNumber = customer.AccountNumber;
				if (string.IsNullOrEmpty(accountNumber))
				{
					throw new Exception($"Customer with ID {customerId} does not have a valid AccountNumber.");
				}

				var servicePackage = db.ServicePackages.SingleOrDefault(sp => sp.ServicePackageId == servicePackageId);
				if (servicePackage == null)
				{
					throw new Exception($"Service package with ID {servicePackageId} not found.");
				}

				var activationDate = DateTime.Now;
				var duration = servicePackage.Duration ?? 0; // Sử dụng giá trị 0 nếu duration là null
				var terminationDate = activationDate.AddDays((double)duration);

				var connection = new Connection
				{
					AccountNumber = accountNumber,
					CustomerId = customerId,
					ConnectionType = connectionType,
					Status = "Active",
					ActivationDate = activationDate,
					TerminationDate = terminationDate
				};

				db.Connections.Add(connection);
				db.SaveChanges();

				// Create a connection package entry
				var connectionPackage = connectionPackageService.CreateConnectionPackage(connection.ConnectionId, servicePackageId, terminationDate);

				return connection;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Error occurred while creating connection: {ex.Message}");
				throw;
			}
		}
	}
}
