using PaySharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PaySharp.Wechatpay;
using PaySharp.Wechatpay.Request;
using PaySharp.Wechatpay.Domain;
using PaySharp.Core.Response;

namespace Edu.UI.Controllers.api
{
    /// <summary>
    /// wechat pay
    /// </summary>
    public class WPController : ApiController
    {
        private readonly IGateway _gateway;
        public WPController(IGateways gw)
        {
            _gateway = gw.Get<WechatpayGateway>();
        }

        [ActionName("pay")]
        [HttpPost]
        public IHttpActionResult Pay([FromBody]ScanPayModel payModel)
        {
            var request = new ScanPayRequest();
            if (payModel.TotalAmount == 0)
            {
                payModel.TotalAmount = 500;
            }

            request.AddGatewayData(payModel);
            var response = _gateway.Execute(request);
            return Json(response);
        }



        #region Rest Funcs

        /// <summary>
        /// 支付成功事件
        /// </summary>
        /// <param name="response">返回结果</param>
        /// <param name="message">提示信息</param>
        private void BarcodePay_PaySucceed(IResponse response, string message)
        {

        }

        /// <summary>
        /// 支付失败事件
        /// </summary>
        /// <param name="response">返回结果,可能是BarcodePayResponse/QueryResponse</param>
        /// <param name="message">提示信息</param>
        private void BarcodePay_PayFaild(IResponse response, string message)
        {
        }

        [HttpPost]
        public IHttpActionResult Query(string out_trade_no, string trade_no)
        {
            var request = new QueryRequest();
            request.AddGatewayData(new QueryModel()
            {
                TradeNo = trade_no,
                OutTradeNo = out_trade_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult Refund(string out_trade_no, string trade_no, int total_amount, int refund_amount, string refund_desc, string out_refund_no)
        {
            var request = new RefundRequest();
            request.AddGatewayData(new RefundModel()
            {
                TradeNo = trade_no,
                RefundAmount = refund_amount,
                RefundDesc = refund_desc,
                OutRefundNo = out_refund_no,
                TotalAmount = total_amount,
                OutTradeNo = out_trade_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult RefundQuery(string out_trade_no, string trade_no, string out_refund_no, string refund_no)
        {
            var request = new RefundQueryRequest();
            request.AddGatewayData(new RefundQueryModel()
            {
                TradeNo = trade_no,
                OutTradeNo = out_trade_no,
                OutRefundNo = out_refund_no,
                RefundNo = refund_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult Close(string out_trade_no)
        {
            var request = new CloseRequest();
            request.AddGatewayData(new CloseModel()
            {
                OutTradeNo = out_trade_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult Cancel(string out_trade_no)
        {
            var request = new CancelRequest();
            request.AddGatewayData(new CancelModel()
            {
                OutTradeNo = out_trade_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult Transfer(string out_trade_no, string openid, string check_name, string true_name, int amount, string desc)
        {
            var request = new TransferRequest();
            request.AddGatewayData(new TransferModel()
            {
                OutTradeNo = out_trade_no,
                OpenId = openid,
                Amount = amount,
                Desc = desc,
                CheckName = check_name,
                TrueName = true_name
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult TransferQuery(string out_trade_no)
        {
            var request = new TransferQueryRequest();
            request.AddGatewayData(new TransferQueryModel()
            {
                OutTradeNo = out_trade_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [ActionName("pk")]
        [HttpGet]
        public IHttpActionResult PublicKey()
        {
            var request = new PublicKeyRequest();

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult TransferToBank(string out_trade_no, string bank_no, string true_name, string bank_code, int amount, string desc)
        {
            var request = new TransferToBankRequest();
            request.AddGatewayData(new TransferToBankModel()
            {
                OutTradeNo = out_trade_no,
                BankNo = bank_no,
                Amount = amount,
                Desc = desc,
                BankCode = bank_code,
                TrueName = true_name
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult TransferToBankQuery(string out_trade_no)
        {
            var request = new TransferToBankQueryRequest();
            request.AddGatewayData(new TransferToBankQueryModel()
            {
                OutTradeNo = out_trade_no
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        [HttpPost]
        public IHttpActionResult BillDownload(string bill_date, string bill_type)
        {
            var request = new BillDownloadRequest();
            request.AddGatewayData(new BillDownloadModel()
            {
                BillDate = bill_date,
                BillType = bill_type
            });

            var response = _gateway.Execute(request);



            // return File(response.GetBillFile(), "text/csv", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            throw new NotImplementedException();
        }

        [HttpPost]
        public IHttpActionResult FundFlowDownload(string bill_date, string account_type)
        {
            var request = new FundFlowDownloadRequest();
            request.AddGatewayData(new FundFlowDownloadModel()
            {
                BillDate = bill_date,
                AccountType = account_type
            });

            var response = _gateway.Execute(request);
            
             throw new NotImplementedException();
            //return File(response.GetBillFile(), "text/csv", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
        }

        [HttpPost]
        public IHttpActionResult OAuth(string code)
        {
            var request = new OAuthRequest();
            request.AddGatewayData(new OAuthModel()
            {
                Code = code
            });

            var response = _gateway.Execute(request);
            return Json(response);
        }

        #endregion
    }
}
