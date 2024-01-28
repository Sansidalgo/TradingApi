using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.AppConfig;
using NLog;
using sansidalgo.core.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Repositories
{
    public class PaymentsRepository : BaseRepository
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public PaymentsRepository(AlgoContext _context) : base(_context)
        {
        }

      

        public async Task<DbStatus> GetPaymentsByTraderId(int traderId)
        {
            DbStatus res = new DbStatus();

            try
            {

                var Result = await context.TblPayments.Where(w => w.TraderId == traderId).ToListAsync();



                if (Result.Count > 0)
                {

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

        public async Task<DbStatus> AddPayments(PaymentsRequestDto payment)
        {
            DbStatus res = new DbStatus();

            try
            {
                
                if (payment.SubscriptionId == null || payment.SubscriptionId == 0)
                {

                    TblUserSubscription subscription = new TblUserSubscription();
                    subscription.TraderId = payment.TraderId;
                    subscription.PlanId = (await context.TblPlans.Where(w => w.Name == payment.PlanName+" "+payment.PlanTerm).FirstOrDefaultAsync()).Id;
                    subscription.SubscriptionStatusId = (await context.TblSubscriptionStatuses.Where(w => w.Name == "Payment Submitted").FirstOrDefaultAsync()).Id;
                    subscription.StartDt = DateTime.Now;
                    if (payment.PlanTerm.ToLower() == "monthly")
                    {
                        subscription.EndDt = DateTime.Now.AddDays(30);

                    }
                    else if (payment.PlanTerm.ToLower() == "yearly")
                    {
                        subscription.EndDt = DateTime.Now.AddDays(365);
                    }

                     context.TblUserSubscriptions.Add(subscription);
                    res.Status =await context.SaveChangesAsync();
                    payment.SubscriptionId= subscription.Id;
                }
                if (res.Status == 1)
                {


                    TblPayment paymentDb = new TblPayment();
                    paymentDb.Amount = payment.Amount;
                    paymentDb.StatusId = (await context.TblPaymentStatuses.Where(w => w.Name == "Initiated").FirstOrDefaultAsync()).Id;
                    paymentDb.SubscriptionId = payment.SubscriptionId;
                    paymentDb.TraderId = payment.TraderId;
                    paymentDb.TransactionId = payment.TransactionId;
                    paymentDb.PaymentDt = DateTime.Now;
                    context.TblPayments.Add(paymentDb);
                    res.Status=await context.SaveChangesAsync();
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
