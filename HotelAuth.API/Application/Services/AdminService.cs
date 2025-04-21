using Application.DTOs;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminService : IAdminService

    {
        public readonly IAdminRepository admninRepository;
        public AdminService(IAdminRepository adminRepository) { 
            this.admninRepository = adminRepository;
        }
        public async Task<string> AuthWithCredentials(AdminDTO adminDTO)
        {
            Admin admin = new Admin
            {
                UserName = adminDTO.UserNameDTO,
                
            };
            return await admninRepository.AuthWithCredentials(admin,adminDTO.PasswordDTO);
        }

        public async Task<bool> RegisterNewAdmin(AdminDTO adminDTO)
        {
            Console.WriteLine(adminDTO.UserNameDTO);
            Admin admin = new Admin
            {
                UserName = adminDTO.UserNameDTO,
                NormalizedUserName = adminDTO.UserNameDTO,
                
            };
            return await this.admninRepository.RegisterNewAdmin(admin, adminDTO.PasswordDTO);
        }

        public async Task<bool> VerifyToken(TokenDTO tokenDTO)
        {
            Token token = new Token { TokenString= tokenDTO.TokenDTOString };
            return await this.admninRepository.VerifyToken(token);
        }
    }
}
