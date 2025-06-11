using Castle.Core.Resource;
using DEMO_NEXUSPROJECT.Models;

namespace NEXUSPROJECT.Services;

public class ContactServiceImpl  :ContactService
{
    private DatabaseContext db;

    public ContactServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public bool SaveContactMessage(ContactMessage contactMessage)
    {
        try
        {
            // Thêm khách hàng vào danh sách
            db.ContactMessages.Add(contactMessage);
            db.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
