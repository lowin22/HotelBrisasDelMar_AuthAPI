using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<string> AuthWithCredentials(AdminDTO adminDTO);
        Task<bool> VerifyToken(TokenDTO tokenDTO);
        Task<bool> RegisterNewAdmin(AdminDTO adminDTO);
    }
}
