using DEMO_NEXUSPROJECT.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace DEMO_NEXUSPROJECT.Services
{
    public class ConnectionPackageServiceImpl : ConnectionPackageService
    {
        private DatabaseContext db;

        public ConnectionPackageServiceImpl(DatabaseContext _db)
        {
            db = _db;
        }

        public ConnectionPackage CreateConnectionPackage(int connectionId, int servicePackageId, DateTime endDate)
        {
            try
            {
                // Kiểm tra xem đã có ConnectionPackage với các khóa chính đã cho chưa
                var existingConnectionPackage = db.ConnectionPackages
                    .SingleOrDefault(cp => cp.ConnectionId == connectionId && cp.ServicePackageId == servicePackageId);

                if (existingConnectionPackage != null)
                {
                    // Nếu đã tồn tại, cập nhật thông tin và lưu lại
                    existingConnectionPackage.StartDate = DateTime.Now;
                    existingConnectionPackage.EndDate = endDate;
                    db.SaveChanges();

                    return existingConnectionPackage;
                }
                else
                {
                    // Nếu chưa tồn tại, tạo mới và thêm vào context
                    var newConnectionPackage = new ConnectionPackage
                    {
                        ConnectionId = connectionId,
                        ServicePackageId = servicePackageId,
                        StartDate = DateTime.Now,
                        EndDate = endDate
                    };

                    db.ConnectionPackages.Add(newConnectionPackage);
                    db.SaveChanges();

                    return newConnectionPackage;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occurred while creating connection package: {ex.Message}");
                throw;
            }
        }
    }
}
