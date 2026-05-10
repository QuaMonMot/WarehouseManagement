using System.Collections.Generic;
using Warehouse.DAL.Repositories;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class RoleService 
    {
        private readonly RoleRepository _roleRepo;

        // Sửa lỗi ở đây: Nhận Repo từ DI container thay vì tự 'new'
        public RoleService(RoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public List<Role> GetRoleList()
        {
            // Có thể thêm logic kiểm tra quyền Admin mới được lấy danh sách Role
            return _roleRepo.GetAll();
        }
    }
}