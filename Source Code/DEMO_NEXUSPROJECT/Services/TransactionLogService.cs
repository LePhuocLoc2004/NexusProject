using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface TransactionLogService
{

    public  List<TransactionLog> findbyId(int customerId);
	public TransactionLog CreateTransactionLog(int customerId, string transactionType, DateTime transactionDate, string details);
}
