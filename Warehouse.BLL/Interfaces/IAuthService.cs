using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        User Login(
            string username,
            string password
        );
    }
}