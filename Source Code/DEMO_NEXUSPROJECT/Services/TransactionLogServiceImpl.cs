using DEMO_NEXUSPROJECT.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace DEMO_NEXUSPROJECT.Services;

public class TransactionLogServiceImpl : TransactionLogService
{
	private DatabaseContext db;

	public TransactionLogServiceImpl(DatabaseContext _db)
	{
		db = _db;
	}
	public TransactionLog CreateTransactionLog(int customerId, string transactionType, DateTime transactionDate, string details)
	{
		try
		{
			var transactionLog = new TransactionLog
			{
				CustomerId = customerId,
				TransactionType = transactionType,
				TransactionDate = transactionDate,
                Details = details
			};

			db.TransactionLogs.Add(transactionLog);
			db.SaveChanges();

			return transactionLog;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error occurred while creating transaction log: {ex.Message}");
			throw;
		}
	}

    public List<TransactionLog> findbyId(int customerId)
    {
        return db.TransactionLogs.Where(t => t.CustomerId == customerId).ToList();
    }
}
