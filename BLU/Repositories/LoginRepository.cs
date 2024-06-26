﻿using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using sansidalgo.core.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public LoginRepository(AlgoContext _context) : base(_context)
        {
        }


        public async Task<DbStatus> SignUp(SignupRequestDto traderDetails)
        {
            DbStatus res = new DbStatus();
            try
            {
                TblTraderDetail tblTraderDetail = new TblTraderDetail();
                tblTraderDetail.Name = traderDetails.Name;
                tblTraderDetail.EmailId = traderDetails.EmailId;
                tblTraderDetail.Password = traderDetails.Password;
                tblTraderDetail.PhoneNo = traderDetails.PhoneNo;
                tblTraderDetail.RoleId = 3;
                await context.TblTraderDetails.AddAsync(tblTraderDetail);

                res.Status = await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Message = res.GetStatus(ex);


            }

            return res;
        }

        public async Task<DbStatus> SignIn(SignInRequestDto requestDto)
        {
            DbStatus res = new DbStatus();
            try
            {
                var Result = await context.TblTraderDetails.Include(td => td.Role)
                    .Where(e => e.PhoneNo == requestDto.PhoneNo && e.Password == requestDto.Password)
                    .Select(s => new SignInResponseDto() { Id = s.Id, EmailId = s.EmailId, Name = s.Name, PhoneNo = s.PhoneNo, Role = s.Role != null ? s.Role.Name : "user" })
                    .FirstOrDefaultAsync();

                if (Result == null || string.IsNullOrWhiteSpace(Result.EmailId))
                {
                    res.Message = "Mobile or EmailId or Password Does not Match";
                    res.Status = 0;
                }
                else
                {
                    res.Message = "Login Successfully";
                    res.Result = Result;
                    res.Status = 1;
                }
            }
            catch (Exception ex)
            {
                await CommonHelper.LogExceptionAsync(ex, logger);
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
    }
}
