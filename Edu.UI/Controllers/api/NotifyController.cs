using PaySharp.Alipay.Response;
using PaySharp.Core;
using PaySharp.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Edu.UI.Service;
using log4net;

namespace Edu.UI.Controllers.api
{
    public class NotifyController : ApiController
    {
        private readonly IGateways _gateways;
        private bool isRedirect;

        public NotifyController(IGateways gateways)
        {
            _gateways = gateways;
        }

        public async Task Index()
        {
            // 订阅支付通知事件
            Notify notify = new Notify(_gateways);
            notify.PaySucceed += Notify_PaySucceed;
            notify.RefundSucceed += Notify_RefundSucceed;
            notify.CancelSucceed += Notify_CancelSucceed;
            notify.UnknownNotify += Notify_UnknownNotify;
            notify.UnknownGateway += Notify_UnknownGateway;

            // 接收并处理支付通知
            await notify.ReceivedAsync();

            if (isRedirect)
            {
               // IResponse.Redirect("https://github.com/Varorbc/PaySharp");
            }
        }

        private bool Notify_PaySucceed(object sender, PaySucceedEventArgs e)
        {

            LogService.AppLog.Info("alipay feedback starts....");

            // 支付成功时时的处理代码
            /* 建议添加以下校验。
             * 1、需要验证该通知数据中的OutTradeNo是否为商户系统中创建的订单号，
             * 2、判断Amount是否确实为该订单的实际金额（即商户订单创建时的金额），
             */
            if (e.GatewayType == typeof(PaySharp.Alipay.AlipayGateway))
            {
                var alipayNotifyResponse = (NotifyResponse)e.NotifyResponse;
                LogService.AppLog.Info("info"+alipayNotifyResponse.AppId+", time is:"+alipayNotifyResponse.NotifyTime);
                //同步通知，即浏览器跳转返回
                if (e.NotifyType == NotifyType.Sync)
                {
                    isRedirect = true;
                }
            }

            //处理成功返回true
            return true;
        }

        private bool Notify_RefundSucceed(object arg1, RefundSucceedEventArgs arg2)
        {
            // 订单退款时的处理代码
            return true;
        }

        private bool Notify_CancelSucceed(object arg1, CancelSucceedEventArgs arg2)
        {
            // 订单撤销时的处理代码
            return true;
        }

        private bool Notify_UnknownNotify(object sender, UnKnownNotifyEventArgs e)
        {
            // 未知时的处理代码
            return true;
        }

        private void Notify_UnknownGateway(object sender, UnknownGatewayEventArgs e)
        {
            // 无法识别支付网关时的处理代码
        }
    }
}
