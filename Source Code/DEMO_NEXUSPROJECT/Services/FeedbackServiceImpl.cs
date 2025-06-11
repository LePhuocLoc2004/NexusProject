using DEMO_NEXUSPROJECT.Models;

namespace DEMO_NEXUSPROJECT.Services;

public class FeedbackServiceImpl :FeedbackService
{
    private DatabaseContext db;

    public FeedbackServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public List<CustomerFeedback> GetAllFeedback()
    {
        return db.CustomerFeedbacks.ToList();
    }

    public void SubmitFeedback(int customerId, string feedbackContent)
    {

        try
        {
            var feedback = new CustomerFeedback
            {
                CustomerId = customerId,
                FeedbackDate = DateTime.Now,
                Feedback = feedbackContent
            };

            db.CustomerFeedbacks.Add(feedback);
            db.SaveChanges(); // Lưu vào cơ sở dữ liệu

            // Các xử lý khác (nếu cần)

        }
        catch (Exception ex)
        {
            // Xử lý ngoại lệ
            throw new ApplicationException("Đã xảy ra lỗi khi ghi nhận phản hồi", ex);
        }
    }

    public void DeleteFeedback(int id)
    {
        var feedback = db.CustomerFeedbacks.Find(id);
        if (feedback != null)
        {
            db.CustomerFeedbacks.Remove(feedback);
            db.SaveChanges();
        }
    }

    public CustomerFeedback GetFeedbackById(int id)
    {
        return db.CustomerFeedbacks.Find(id);
    }


}
