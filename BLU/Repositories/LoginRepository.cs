using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public LoginRepository(AlgoContext _context) : base(_context)
        {
        }
        public async Task<DbStatus> SaveTraderDetails(TraderDetailsRequestDto traderDetails)
        {
            DbStatus res = new DbStatus();
            try
            {
                TblTraderDetail tblTraderDetail = new TblTraderDetail();
                tblTraderDetail.Name = traderDetails.Name;
                tblTraderDetail.EmailId = traderDetails.EmailId;
                tblTraderDetail.Password = traderDetails.Password;
                tblTraderDetail.PhoneNo = traderDetails.PhoneNo;
                await context.TblTraderDetails.AddAsync(tblTraderDetail);

                res.Status = await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                res.Message = res.GetStatus(ex);


            }

            return res;
        }

        public async Task<DbStatus> VerifyUser(TraderDetailsRequestDto requestDto)
        {
            DbStatus res=new DbStatus();
            try
            {

           
            res.Status=Convert.ToInt32(await context.TblTraderDetails.AnyAsync(e => e.EmailId == requestDto.EmailId && e.Password==requestDto.Password));
            }
            catch (Exception ex)
            {
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
    }
}
