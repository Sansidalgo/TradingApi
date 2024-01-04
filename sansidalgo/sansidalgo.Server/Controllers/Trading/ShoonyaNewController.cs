using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using sansidalgo.core.helpers;
using sansidalgo.core.Models;
using sansidalgo.core.Vendors;
using sansidalgo.Server.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ShoonyaNewController : ControllerBase
{
    private readonly CommonHelper helper;

    private readonly AlgoContext context;
    private readonly OrderSettingsRepository setingsRepo;
    private readonly ShoonyaCredentialsRepository shoonyaCredentialsRepo;
    private readonly OrderRepository orderRepo;

    public ShoonyaNewController(CommonHelper _helper, AlgoContext _context, OrderSettingsRepository settingsRepo, ShoonyaCredentialsRepository credentialsRepo, OrderRepository repo)
    {
        context = _context;
        helper = _helper;

        setingsRepo = settingsRepo;
        shoonyaCredentialsRepo = credentialsRepo;
        orderRepo = repo;
    }
    [HttpPost("ExecuteOrderById")]
    public async Task<DbStatus> ExecuteOrderById(int orderSettingId)
    {
        var order = new ShoonyaOrder();
        order.OSID ="temp_"+ Convert.ToString(orderSettingId);


        return await orderRepo.ExecuteOrderLogic(order, setingsRepo, shoonyaCredentialsRepo);
    }

    [HttpPost]
    public async Task<DbStatus> ExecuteOrder(ShoonyaOrder order)
    {
        return await orderRepo.ExecuteOrderLogic(order, setingsRepo, shoonyaCredentialsRepo);
    }

}