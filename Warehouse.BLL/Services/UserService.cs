using Warehouse.DAL.Repositories;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        // Sửa lỗi ở đây: Nhận Repo từ DI container thay vì tự 'new'
        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User CheckLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            // Trong đồ án, bạn nên viết một hàm MD5 hoặc SHA256 để băm password
            // Ở đây mình ví dụ băm đơn giản (thực tế nên dùng thư viện BCrypt hoặc Identity)
            string hashedPass = HashPassword(password);

            return _userRepo.Login(username, hashedPass);
        }

        private string HashPassword(string password)
        {
            // Logic mã hóa mật khẩu của bạn ở đây
            // Để demo nhanh, bạn có thể trả về chính nó nếu DB lưu plaintext (không khuyến khích)
            return password;
        }
    }
}