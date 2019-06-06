using PaySharp.Alipay;
using PaySharp.Alipay.Domain;
using PaySharp.Alipay.Request;
using PaySharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Edu.UI.Controllers.api
{
   
    public class AlipayController : ApiController
    {
        private readonly IGateway _gateway;
      
        public AlipayController(IGateways gateways)
        {
            _gateway = gateways.Get<AlipayGateway>();
        }

       
        [HttpPost]
        public HttpResponseMessage Pay([FromBody]WebPayModel payModel)
        {
            if (ModelState.IsValid)
            {
                var request = new WebPayRequest();
                request.AddGatewayData(payModel);
                var response = _gateway.Execute(request);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(response.Html, Encoding.UTF8, "text/html")
                };
                return result;
            }
            return new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// phone scan pay
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="subject"></param>
        /// <param name="total_amount"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ScanPay(string out_trade_no, string subject, double total_amount, string body)
        {
            var request = new ScanPayRequest();
            request.AddGatewayData(new ScanPayModel()
            {
                Body = body,
                TotalAmount = total_amount,
                Subject = subject,
                OutTradeNo = out_trade_no
            });

            if(_gateway != null)
            {
                var response = _gateway.Execute(request);

                return Json(response);
            }

            return Json(new string[]{ "parameter _gateway is null"});

        }

    }
}
