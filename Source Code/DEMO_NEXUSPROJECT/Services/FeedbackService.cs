using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public interface FeedbackService
{

    public List<CustomerFeedback> GetAllFeedback();

    public void SubmitFeedback(int customerId, string feedbackContent);

    public void DeleteFeedback(int id);

    public CustomerFeedback GetFeedbackById(int id);


}
