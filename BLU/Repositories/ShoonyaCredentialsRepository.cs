using AutoMapper;
using Azure;
using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using NorenRestApiWrapper;
using sansidalgo.core.helpers;
using sansidalgo.core.Helpers;
using sansidalgo.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class ShoonyaCredentialsRepository : BaseRepository, IShoonyaCredentialsRepository
    {

        
        public ShoonyaCredentialsRepository(AlgoContext _context) : base(_context)
        {

           
        }

        public async Task<DbStatus> Add(TblShoonyaCredential credential)
        {
            DbStatus res = new DbStatus();
            try
            {
                if (credential.TraderId != 0)
                {
                    await context.TblShoonyaCredentials.AddAsync(credential);

                    res.Status = await context.SaveChangesAsync();
                    if (res.Status != 1)
                    {
                        res.Message = "Entry unsuccessful";
                    }
                    else { res.Message = "Entry Successfull"; };
                }



            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }

        public async Task<DbStatus> GetCredentials(int? traderID)
        {
            DbStatus res = new DbStatus();
            if (traderID == null) { return res; }
            try
            {

                var Result = await context.TblShoonyaCredentials.Where(w => w.TraderId == Convert.ToInt32(traderID)).ToListAsync();



                if (Result.Count > 0)
                {

                    res.Result = Result;

                }
                res.Status = 1;
            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
        public async Task<DbStatus> ShoonyaSignIn(OrderSettingsResponseDto order)
        {
            DbStatus res = new DbStatus();
            ShoonyaReponseDto response = new ShoonyaReponseDto();
            //if (traderID == null) { return res; }
            try
            {
                var nApi = new NorenRestApi();
                var responseHandler = new BaseResponseHandler();
                var endPoint = "https://api.shoonya.com/NorenWClientTP/";



                if (!string.IsNullOrWhiteSpace(Convert.ToString(order.Credential.Token).Trim()))
                {
                    await nApi.ValidateLoginAync(responseHandler.OnResponse, endPoint, CommonHelper.DecodeValue(order.Credential.Uid.Trim()), CommonHelper.DecodeValue(order.Credential.Password.Trim()), order.Credential.Token);

                    var result = responseHandler.baseResponse.toJson();

                    if (result.Contains("Session Expired"))
                    {
                        res.Status = 0;
                        res.Message = "Session Expired";

                    }
                    else if (result.Contains("Invalid Session"))
                    {
                        res.Status = 0;
                        res.Message = "Invalid Session";

                    }
                    else if (result.Contains("Unauthorized"))
                    {
                        res.Status = 0;
                        res.Message = "Unauthorized";


                    }
                    else
                    {
                        res.Message="Login success: "+ result.ToString();
                        res.Status = 1;
                    }




                }
                if (res.Status == 0)
                {

                    LoginMessage loginMessage = new LoginMessage();
                    loginMessage.apkversion = "1.0.0";
                    loginMessage.uid = CommonHelper.DecodeValue(order.Credential.Uid.Trim());
                    loginMessage.pwd = CommonHelper.DecodeValue(order.Credential.Password.Trim());
                    loginMessage.vc = order.Credential.Vc.Trim();
                    loginMessage.appkey = order.Credential.ApiKey.Trim();
                    loginMessage.imei = order.Credential.Imei.Trim();
                    loginMessage.source = "API";

                    OtpEntity oe = await CommonHelper.GetTOTP(order.Credential.AuthSecreteKey.Trim());

                    loginMessage.factor2 = oe.OTP;

                    await nApi.SendLoginAsync(responseHandler.OnResponse, endPoint, loginMessage);



                    LoginResponse? loginResponse = responseHandler?.baseResponse as LoginResponse;
                    Console.WriteLine("app handler :" + responseHandler.baseResponse.toJson());

                    if (loginResponse?.emsg != null && loginResponse.emsg.Contains("Session Expired"))
                    {
                        res.Status = 0;
                        res.Message = "Session Expired";
                    }
                    else if (!string.IsNullOrWhiteSpace(loginResponse.susertoken))
                    {
                        var credentailTemp = context.TblShoonyaCredentials.Find(order.Credential.Id);
                        credentailTemp.Token = loginResponse.susertoken;
                        context.TblShoonyaCredentials.Update(credentailTemp);
                        await context.SaveChangesAsync();

                        res.Status = 1;
                        res.Message ="User logged in :"+ loginResponse.uname;

                    }




                }
                else
                {
                    res.Status = 1;
                }
                response.NorenRestApi = nApi;
                response.BaseResponseHandler = responseHandler;
                res.Result = response;
            }
            catch (Exception ex)
            {
                res.Status = 0;
                res.Message = res.GetStatus(ex);
            }
            return res;
        }
       

    }
}
