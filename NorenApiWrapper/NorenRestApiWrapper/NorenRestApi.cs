using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NorenRestApiWrapper;

public class NorenRestApi
{
    private RESTClient rClient;

    private NorenWebSocket wsclient;

    private LoginResponse loginResp;

    private LoginMessage loginReq;

    public OnResponse SessionCloseCallback
    {
        set
        {
            rClient.onSessionClose = value;
        }
    }

    public string UserToken => loginResp?.susertoken;

    private string getJKey => "jKey=" + loginResp?.susertoken;

    public OnStreamConnect onStreamConnectCallback { get; set; }

    public OnCloseHandler onStreamCloseCallback { get; set; }

    public OnErrorHandler onStreamErrorCallback { get; set; }

    public NorenRestApi()
    {
        rClient = new RESTClient();
    }

    internal void OnLoginResponseNotify(NorenResponseMsg responseMsg)
    {
        loginResp = responseMsg as LoginResponse;
    }

    private string ComputeSha256Hash(string rawData)
    {
        using SHA256 sHA = SHA256.Create();
        byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }

    public bool SendLogin(OnResponse response, string endPoint, LoginMessage login)
    {
        loginReq = login;
        login.pwd = ComputeSha256Hash(login.pwd);
        login.appkey = ComputeSha256Hash(login.uid + "|" + login.appkey);
        rClient.endPoint = endPoint;
        string uri = "QuickAuth";
        NorenApiResponse<LoginResponse> norenApiResponse = new NorenApiResponse<LoginResponse>(response);
        norenApiResponse.ResponseNotifyInstance = (NorenApiResponse<LoginResponse>.ResponseNotify)Delegate.Combine(norenApiResponse.ResponseNotifyInstance, new NorenApiResponse<LoginResponse>.ResponseNotify(OnLoginResponseNotify));
        rClient.makeRequest(norenApiResponse, uri, login.toJson());
        return true;
    }
    public async Task<bool> SendLoginAsync(OnResponse response, string endPoint, LoginMessage login)
    {
        loginReq = login;
        login.pwd = ComputeSha256Hash(login.pwd);
        login.appkey = ComputeSha256Hash(login.uid + "|" + login.appkey);
        rClient.endPoint = endPoint;
        string uri = "QuickAuth";
        NorenApiResponse<LoginResponse> norenApiResponse = new NorenApiResponse<LoginResponse>(response);
        norenApiResponse.ResponseNotifyInstance = (NorenApiResponse<LoginResponse>.ResponseNotify)Delegate.Combine(norenApiResponse.ResponseNotifyInstance, new NorenApiResponse<LoginResponse>.ResponseNotify(OnLoginResponseNotify));
        await rClient.makeRequestAsync(norenApiResponse, uri, login.toJson());
        return true;
    }

    public async Task<bool> ValidateLoginAync(OnResponse response, string endpoint, string uid, string pwd, string usertoken)
    {
        rClient.endPoint = endpoint;
        loginReq = new LoginMessage();
        loginReq.uid = uid;
        loginReq.pwd = pwd;
        loginReq.source = "API";
        loginResp = new LoginResponse();
        loginResp.actid = uid;
        loginResp.susertoken = usertoken;
        var result = await ValidateToken(response);

        return true;
    }
    private async Task<bool> ValidateToken(OnResponse response)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetHsToken";
        UserDetails positionBook = new UserDetails();
        positionBook.uid = loginReq.uid;
        //positionBook.actid = account;
        await rClient.makeRequestAsync(new NorenApiResponseList<PositionBookResponse, PositionBookItem>(response), uri, positionBook.toJson(), getJKey);
        return true;
    }
    public bool SendLogout(OnResponse response)
    {
        if (loginResp == null)
        {
            return false;
        }
        LogoutMessage logoutMessage = new LogoutMessage();
        logoutMessage.uid = loginReq.uid;
        string uri = "Logout";
        NorenApiResponse<LogoutResponse> response2 = new NorenApiResponse<LogoutResponse>(response);
        rClient.makeRequest(response2, uri, logoutMessage.toJson(), getJKey);
        return true;
    }

    public bool Changepwd(OnResponse response, Changepwd changepwd)
    {
        if (loginResp == null)
        {
            return false;
        }
        changepwd.uid = loginReq.uid;
        changepwd.oldpwd = ComputeSha256Hash(changepwd.oldpwd);
        string uri = "Changepwd";
        NorenApiResponse<ChangepwdResponse> response2 = new NorenApiResponse<ChangepwdResponse>(response);
        rClient.makeRequest(response2, uri, changepwd.toJson());
        return true;
    }

    public bool SendProductConversion(OnResponse response, ProductConversion productConversion)
    {
        if (loginResp == null)
        {
            return false;
        }
        productConversion.uid = loginReq.uid;
        string uri = "ProductConversion";
        NorenApiResponse<ProductConversionResponse> response2 = new NorenApiResponse<ProductConversionResponse>(response);
        rClient.makeRequest(response2, uri, productConversion.toJson(), getJKey);
        return true;
    }

    public bool SendForgotPassword(OnResponse response, string endpoint, string user, string pan, string dob)
    {
        ForgotPassword forgotPassword = new ForgotPassword();
        forgotPassword.uid = user;
        forgotPassword.pan = pan;
        forgotPassword.dob = dob;
        string uri = "ForgotPassword";
        rClient.endPoint = endpoint;
        NorenApiResponse<ForgotPasswordResponse> response2 = new NorenApiResponse<ForgotPasswordResponse>(response);
        rClient.makeRequest(response2, uri, forgotPassword.toJson());
        return true;
    }

    public bool SendGetUserDetails(OnResponse response)
    {
        if (loginResp == null)
        {
            return false;
        }
        UserDetails userDetails = new UserDetails();
        userDetails.uid = loginReq.uid;
        string uri = "UserDetails";
        rClient.makeRequest(new NorenApiResponse<UserDetailsResponse>(response), uri, userDetails.toJson(), getJKey);
        return true;
    }

    public bool SendGetMWList(OnResponse response)
    {
        if (loginResp == null)
        {
            return false;
        }
        UserDetails userDetails = new UserDetails();
        userDetails.uid = loginReq.uid;
        string uri = "MWList";
        rClient.makeRequest(new NorenApiResponse<MWListResponse>(response), uri, userDetails.toJson(), getJKey);
        return true;
    }

    public bool SendSearchScrip(OnResponse response, string exch, string searchtxt)
    {
        if (loginResp == null)
        {
            return false;
        }
        SearchScrip searchScrip = new SearchScrip();
        searchScrip.uid = loginReq.uid;
        searchScrip.exch = exch;
        searchScrip.stext = searchtxt;
        string uri = "SearchScrip";
        rClient.makeRequest(new NorenApiResponse<SearchScripResponse>(response), uri, searchScrip.toJson(), getJKey);
        return true;
    }

    public async Task<bool> SendSearchScripAsync(OnResponse response, string exch, string searchtxt)
    {
        if (loginResp == null)
        {
            return false;
        }
        SearchScrip searchScrip = new SearchScrip();
        searchScrip.uid = loginReq.uid;
        searchScrip.exch = exch;
        searchScrip.stext = searchtxt;
        string uri = "SearchScrip";
        await rClient.makeRequestAsync(new NorenApiResponse<SearchScripResponse>(response), uri, searchScrip.toJson(), getJKey);
        return true;
    }

    public bool SendGetSecurityInfo(OnResponse response, string exch, string token)
    {
        if (loginResp == null)
        {
            return false;
        }
        GetSecurityInfo getSecurityInfo = new GetSecurityInfo();
        getSecurityInfo.uid = loginReq.uid;
        getSecurityInfo.exch = exch;
        getSecurityInfo.token = token;
        string uri = "GetSecurityInfo";
        rClient.makeRequest(new NorenApiResponse<GetSecurityInfoResponse>(response), uri, getSecurityInfo.toJson(), getJKey);
        return true;
    }

    public bool SendAddMultiScripsToMW(OnResponse response, string watchlist, string scrips)
    {
        if (loginResp == null)
        {
            return false;
        }
        AddMultiScripsToMW addMultiScripsToMW = new AddMultiScripsToMW();
        addMultiScripsToMW.uid = loginReq.uid;
        addMultiScripsToMW.wlname = watchlist;
        addMultiScripsToMW.scrips = scrips;
        string uri = "AddMultiScripsToMW";
        rClient.makeRequest(new NorenApiResponse<StandardResponse>(response), uri, addMultiScripsToMW.toJson(), getJKey);
        return true;
    }

    public bool SendDeleteMultiMWScrips(OnResponse response, string watchlist, string scrips)
    {
        if (loginResp == null)
        {
            return false;
        }
        AddMultiScripsToMW addMultiScripsToMW = new AddMultiScripsToMW();
        addMultiScripsToMW.uid = loginReq.uid;
        addMultiScripsToMW.wlname = watchlist;
        addMultiScripsToMW.scrips = scrips;
        string uri = "DeleteMultiMWScrips";
        rClient.makeRequest(new NorenApiResponse<StandardResponse>(response), uri, addMultiScripsToMW.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendPlaceOrderAsync(OnResponse response, PlaceOrder order)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "PlaceOrder";
        await rClient.makeRequestAsync(new NorenApiResponse<PlaceOrderResponse>(response), uri, order.toJson(), getJKey);
        return true;
    }

    public bool SendPlaceOrder(OnResponse response, PlaceOrder order)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "PlaceOrder";
        rClient.makeRequest(new NorenApiResponse<PlaceOrderResponse>(response), uri, order.toJson(), getJKey);
        return true;
    }

    public bool SendModifyOrder(OnResponse response, ModifyOrder order)
    {
        if (loginResp == null)
        {
            return false;
        }
        order.uid = loginReq.uid;
        string uri = "ModifyOrder";
        rClient.makeRequest(new NorenApiResponse<ModifyOrderResponse>(response), uri, order.toJson(), getJKey);
        return true;
    }

    public bool SendCancelOrder(OnResponse response, string norenordno)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "CancelOrder";
        CancelOrder cancelOrder = new CancelOrder();
        cancelOrder.norenordno = norenordno;
        cancelOrder.uid = loginReq.uid;
        rClient.makeRequest(new NorenApiResponse<CancelOrderResponse>(response), uri, cancelOrder.toJson(), getJKey);
        return true;
    }

    public bool SendExitSNOOrder(OnResponse response, string norenordno, string product)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "ExitSNOOrder";
        ExitSNOOrder exitSNOOrder = new ExitSNOOrder();
        exitSNOOrder.norenordno = norenordno;
        exitSNOOrder.prd = product;
        exitSNOOrder.uid = loginReq.uid;
        rClient.makeRequest(new NorenApiResponse<ExitSNOOrderResponse>(response), uri, exitSNOOrder.toJson(), getJKey);
        return true;
    }

    public bool SendGetOrderBook(OnResponse response, string product)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "OrderBook";
        OrderBook orderBook = new OrderBook();
        orderBook.uid = loginReq.uid;
        orderBook.prd = product;
        rClient.makeRequest(new NorenApiResponseList<OrderBookResponse, OrderBookItem>(response), uri, orderBook.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendGetOrderBookAsync(OnResponse response, string product)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "OrderBook";
        OrderBook orderBook = new OrderBook();
        orderBook.uid = loginReq.uid;
        orderBook.prd = product;
        await rClient.makeRequestAsync(new NorenApiResponseList<OrderBookResponse, OrderBookItem>(response), uri, orderBook.toJson(), getJKey);
        return true;
    }

    public bool SendGetMultiLegOrderBook(OnResponse response, string product)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "MultiLegOrderBook";
        MultiLegOrderBook multiLegOrderBook = new MultiLegOrderBook();
        multiLegOrderBook.uid = loginReq.uid;
        multiLegOrderBook.prd = product;
        rClient.makeRequest(new NorenApiResponseList<MultiLegOrderBookResponse, MultiLegOrderBookItem>(response), uri, multiLegOrderBook.toJson(), getJKey);
        return true;
    }

    public bool SendGetTradeBook(OnResponse response, string account)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "TradeBook";
        TradeBook tradeBook = new TradeBook();
        tradeBook.uid = loginReq.uid;
        tradeBook.actid = account;
        rClient.makeRequest(new NorenApiResponseList<TradeBookResponse, TradeBookItem>(response), uri, tradeBook.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendGetTradeBookAsync(OnResponse response, string account)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "TradeBook";
        TradeBook tradeBook = new TradeBook();
        tradeBook.uid = loginReq.uid;
        tradeBook.actid = account;
        await rClient.makeRequestAsync(new NorenApiResponseList<TradeBookResponse, TradeBookItem>(response), uri, tradeBook.toJson(), getJKey);
        return true;
    }

    public bool SendGetOrderHistory(OnResponse response, string norenordno)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "SingleOrdHist";
        SingleOrdHist singleOrdHist = new SingleOrdHist();
        singleOrdHist.uid = loginReq.uid;
        singleOrdHist.norenordno = norenordno;
        rClient.makeRequest(new NorenApiResponseList<OrderHistoryResponse, SingleOrdHistItem>(response), uri, singleOrdHist.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendGetOrderHistoryAsync(OnResponse response, string norenordno)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "SingleOrdHist";
        SingleOrdHist singleOrdHist = new SingleOrdHist();
        singleOrdHist.uid = loginReq.uid;
        singleOrdHist.norenordno = norenordno;
       await rClient.makeRequestAsync(new NorenApiResponseList<OrderHistoryResponse, SingleOrdHistItem>(response), uri, singleOrdHist.toJson(), getJKey);
        return true;
    }

    public bool SendGetPositionBook(OnResponse response, string account)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "PositionBook";
        PositionBook positionBook = new PositionBook();
        positionBook.uid = loginReq.uid;
        positionBook.actid = account;
        rClient.makeRequest(new NorenApiResponseList<PositionBookResponse, PositionBookItem>(response), uri, positionBook.toJson(), getJKey);


        return true;
    }
    public async Task<bool> SendGetPositionBookAsync(OnResponse response, string account)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "PositionBook";
        PositionBook positionBook = new PositionBook();
        positionBook.uid = loginReq.uid;
        positionBook.actid = account;
        await rClient.makeRequestAsync(new NorenApiResponseList<PositionBookResponse, PositionBookItem>(response), uri, positionBook.toJson(), getJKey);


        return true;
    }


    public bool SendGetHoldings(OnResponse response, string account, string product)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "Holdings";
        Holdings holdings = new Holdings();
        holdings.uid = loginReq.uid;
        holdings.actid = account;
        holdings.prd = product;
        rClient.makeRequest(new NorenApiResponseList<HoldingsResponse, HoldingsItem>(response), uri, holdings.toJson(), getJKey);
        return true;
    }

    public bool SendGetLimits(OnResponse response, string account, string product = "", string segment = "", string exchange = "")
    {
        if (loginResp == null)
        {
            return false;
        }
        Limits limits = new Limits();
        limits.actid = account;
        limits.uid = loginReq.uid;
        if (product != "")
        {
            limits.prd = product;
        }
        if (segment != "")
        {
            limits.seg = segment;
        }
        if (exchange != "")
        {
            limits.exch = exchange;
        }
        string uri = "Limits";
        rClient.makeRequest(new NorenApiResponse<LimitsResponse>(response), uri, limits.toJson(), getJKey);
        return true;
    }

    public bool SendGetOrderMargin(OnResponse response, OrderMargin order)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetOrderMargin";
        rClient.makeRequest(new NorenApiResponse<GetOrderMarginResponse>(response), uri, order.toJson(), getJKey);
        return true;
    }

    public bool SendGetBasketMargin(OnResponse response, BasketMargin basket)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetBasketMargin";
        rClient.makeRequest(new NorenApiResponse<GetBasketMarginResponse>(response), uri, basket.toJson(), getJKey);
        return true;
    }

    public bool SendGetExchMsg(OnResponse response, ExchMsg exchmsg)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "ExchMsg";
        rClient.makeRequest(new NorenApiResponseList<ExchMsgResponse, ExchMsgItem>(response), uri, exchmsg.toJson(), getJKey);
        return true;
    }

    public bool SendGetQuote(OnResponse response, string exch, string token)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetQuotes";
        Quote quote = new Quote();
        quote.uid = loginReq.uid;
        quote.exch = exch;
        quote.token = token;
        rClient.makeRequest(new NorenApiResponse<GetQuoteResponse>(response), uri, quote.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendGetQuoteAsync(OnResponse response, string exch, string token)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetQuotes";
        Quote quote = new Quote();
        quote.uid = loginReq.uid;
        quote.exch = exch;
        quote.token = token;
        await rClient.makeRequestAsync(new NorenApiResponse<GetQuoteResponse>(response), uri, quote.toJson(), getJKey);
        return true;
    }

    public bool SendGetTPSeries(OnResponse response, string exch, string token, string starttime = null, string endtime = null, string interval = null)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "TPSeries";
        TPSeries tPSeries = new TPSeries();
        tPSeries.uid = loginReq.uid;
        tPSeries.exch = exch;
        tPSeries.token = token;
        if (!string.IsNullOrEmpty(starttime))
        {
            tPSeries.st = starttime;
        }
        if (!string.IsNullOrEmpty(endtime))
        {
            tPSeries.et = endtime;
        }
        if (!string.IsNullOrEmpty(interval))
        {
            tPSeries.intrv = interval;
        }
        rClient.makeRequest(new NorenApiResponseList<GetTPSeriesResponse, TPSeriesItem>(response), uri, tPSeries.toJson(), getJKey);
        return true;
    }

    public bool SendGetIndexList(OnResponse response, string exch)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetIndexList";
        IndexList indexList = new IndexList();
        indexList.uid = loginReq.uid;
        indexList.exch = exch;
        rClient.makeRequest(new NorenApiResponse<GetIndexListResponse>(response), uri, indexList.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendGetIndexListAsync(OnResponse response, string exch)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetIndexList";
        IndexList indexList = new IndexList();
        indexList.uid = loginReq.uid;
        indexList.exch = exch;
        await rClient.makeRequestAsync(new NorenApiResponse<GetIndexListResponse>(response), uri, indexList.toJson(), getJKey);
        return true;
    }

    public bool GetDailyTPSeries(OnResponse response, string endpoint, string exch, string token, string starttime, string endtime)
    {
        return true;
    }

    public bool SendGetOptionChain(OnResponse response, string exch, string tsym, string strprc, int count)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetOptionChain";
        OptionChain optionChain = new OptionChain();
        optionChain.uid = loginReq.uid;
        optionChain.exch = exch;
        optionChain.tsym = tsym;
        optionChain.strprc = strprc;
        optionChain.cnt = count.ToString();
        rClient.makeRequest(new NorenApiResponse<OptionChainResponse>(response), uri, optionChain.toJson(), getJKey);
        return true;
    }
    public async Task<bool> SendGetOptionChainAsync(OnResponse response, string exch, string tsym, string strprc, int count)
    {
        if (loginResp == null)
        {
            return false;
        }
        string uri = "GetOptionChain";
        OptionChain optionChain = new OptionChain();

        optionChain.uid = loginReq.uid;
        optionChain.exch = exch;
        optionChain.tsym = tsym;
        optionChain.strprc = strprc;
        optionChain.cnt = count.ToString();
        await rClient.makeRequestAsync(new NorenApiResponse<OptionChainResponse>(response), uri, optionChain.toJson(), getJKey);
        return true;
    }

    public bool ConnectWatcher(string url, OnFeed marketdataHandler, OnOrderFeed orderHandler)
    {
        wsclient = new NorenWebSocket();
        wsclient.onStreamConnectCallback = onStreamConnectCallback;
        wsclient.onStreamCloseCallback = onStreamCloseCallback;
        wsclient.onStreamErrorCallback = onStreamErrorCallback;
        wsclient.Start(url, loginReq.uid, loginResp?.susertoken, marketdataHandler, orderHandler);
        return true;
    }

    public void CloseWatcher()
    {
        wsclient?.Stop();
    }

    public bool SubscribeToken(string exch, string token)
    {
        SubscribeTouchline subscribeTouchline = new SubscribeTouchline();
        subscribeTouchline.k = exch + "|" + token;
        wsclient.Send(subscribeTouchline.toJson());
        Console.WriteLine("Sub Token: " + subscribeTouchline.toJson());
        return true;
    }

    public bool SubscribeToken(List<Quote> tokenlist)
    {
        SubscribeTouchline subscribeTouchline = new SubscribeTouchline();
        subscribeTouchline.k = string.Empty;
        foreach (Quote item in tokenlist)
        {
            if (string.IsNullOrEmpty(subscribeTouchline.k))
            {
                subscribeTouchline.k = item.exch + "|" + item.token;
                continue;
            }
            SubscribeTouchline subscribeTouchline2 = subscribeTouchline;
            subscribeTouchline2.k = subscribeTouchline2.k + "#" + item.exch + "|" + item.token;
        }
        wsclient.Send(subscribeTouchline.toJson());
        Console.WriteLine("Sub Token: " + subscribeTouchline.toJson());
        return true;
    }

    public bool SubscribeTokenDepth(List<Quote> tokenlist)
    {
        SubscribeDepth subscribeDepth = new SubscribeDepth();
        subscribeDepth.k = string.Empty;
        foreach (Quote item in tokenlist)
        {
            if (string.IsNullOrEmpty(subscribeDepth.k))
            {
                subscribeDepth.k = item.exch + "|" + item.token;
                continue;
            }
            SubscribeDepth subscribeDepth2 = subscribeDepth;
            subscribeDepth2.k = subscribeDepth2.k + "#" + item.exch + "|" + item.token;
        }
        wsclient.Send(subscribeDepth.toJson());
        Console.WriteLine("Sub Depth: " + subscribeDepth.toJson());
        return true;
    }

    public bool SubscribeTokenDepth(string exch, string token)
    {
        SubscribeDepth subscribeDepth = new SubscribeDepth();
        subscribeDepth.k = exch + "|" + token;
        wsclient.Send(subscribeDepth.toJson());
        Console.WriteLine("Sub Token Depth: " + subscribeDepth.toJson());
        return true;
    }

    public bool UnSubscribeToken(string exch, string token)
    {
        UnSubscribeTouchline unSubscribeTouchline = new UnSubscribeTouchline();
        unSubscribeTouchline.k = exch + "|" + token;
        wsclient.Send(unSubscribeTouchline.toJson());
        Console.WriteLine("UnSub Token: " + unSubscribeTouchline.toJson());
        return true;
    }

    public bool UnSubscribeTokenDepth(string exch, string token)
    {
        UnSubscribeDepth unSubscribeDepth = new UnSubscribeDepth();
        unSubscribeDepth.k = exch + "|" + token;
        wsclient.Send(unSubscribeDepth.toJson());
        Console.WriteLine("UnSub Token Depth: " + unSubscribeDepth.toJson());
        return true;
    }

    public bool UnSubscribe(List<Quote> tokenlist)
    {
        UnSubscribeTouchline unSubscribeTouchline = new UnSubscribeTouchline();
        unSubscribeTouchline.k = string.Empty;
        foreach (Quote item in tokenlist)
        {
            if (string.IsNullOrEmpty(unSubscribeTouchline.k))
            {
                unSubscribeTouchline.k = item.exch + "|" + item.token;
                continue;
            }
            UnSubscribeTouchline unSubscribeTouchline2 = unSubscribeTouchline;
            unSubscribeTouchline2.k = unSubscribeTouchline2.k + "#" + item.exch + "|" + item.token;
        }
        wsclient.Send(unSubscribeTouchline.toJson());
        Console.WriteLine("UnSub Token: " + unSubscribeTouchline.toJson());
        return true;
    }

    public bool SubscribeOrders(OnOrderFeed orderFeed, string account)
    {
        OrderSubscribeMessage orderSubscribeMessage = new OrderSubscribeMessage();
        orderSubscribeMessage.actid = account;
        wsclient.Send(orderSubscribeMessage.toJson());
        Console.WriteLine("Sub Order: " + orderSubscribeMessage.toJson());
        return true;
    }
}
