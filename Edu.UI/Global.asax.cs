using Autofac;
using Edu.UI.App_Start;
using PaySharp.Alipay;
using PaySharp.Core;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Edu.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private string ipPort = "http://wx.ztuy.com/";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(ApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = new ContainerBuilder();

            HttpConfiguration configs = GlobalConfiguration.Configuration;
            
            Service.PayService.Merchant.Register(typeof(MvcApplication), container,configs, a =>
            {
                var gateways = new Gateways();
                //gateways.RegisterAlipay();

                var alipayMerchant = new PaySharp.Alipay.Merchant
                    {
                        AppId = "2019052865386578",
                        NotifyUrl = ipPort + "api/Notify",
                        ReturnUrl = ipPort + "api/Notify",
                        AlipayPublicKey =
                            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgCPTK/Ydo9coWpQbTJVvhP79ospFdEAanbc0lbrIasdWrY8eLdAQ7RD+lRAkC1u6JvZCjOtPPfJyGoOqzw2UNjPzl12ePYY1Olv5Legrf9P6MqOnM2liCL/pScu06wRJ2QfPstmTbv+kuojCYX2MJYopbzP3Va/iCgA00aWt1Ko9IsvkFMn8YGi6743L+nphBTTO+7VUE6aDwmWgsAT+j6Mz3nhztImcmRtmEwOpvvrftPkEj4PmUw342KjBbISeQWxFgJi6u6VUeZYbdxiHMLY/CD/hxDKxRk8L17XXU88gNPcAq6UqzWjd90LHpXt9D1rP44ksQ9xdKZ4yqypDIQIDAQAB",
                            //"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx96GhQR7T67L18D45tI49sIrqO8BfLA4Ks609VgcMVKQDFyxXzJvQrJNaNQN3XHVrGNoW3NeTKfFq0cGKeRVgNUGM0pQBlTCbcjvdhGm+Lox+W4RtOJkDNRFxl48aZvHcVAtlOZFK5AKSF2tn2wbwjWozb40IKvyS9jdidQy8h0W7tB/ujA8JiokIqW38Kfr7wM3G4AAG5z8XDhBc7mrVOY8HytewLKcbBHdUPkSMBEp61ualX3Xd7RsuxIeCICtLsISR5l4CwrdK+ye12WHDfBWo0T7MhYIoMqXyotyAcj5S3GHhUBONxoE0tnTcj9GlCaejVhd192JtIEA+5l0ZQIDAQAB",
                                Privatekey =
                          "MIIEowIBAAKCAQEAx96GhQR7T67L18D45tI49sIrqO8BfLA4Ks609VgcMVKQDFyxXzJvQrJNaNQN3XHVrGNoW3NeTKfFq0cGKeRVgNUGM0pQBlTCbcjvdhGm+Lox+W4RtOJkDNRFxl48aZvHcVAtlOZFK5AKSF2tn2wbwjWozb40IKvyS9jdidQy8h0W7tB/ujA8JiokIqW38Kfr7wM3G4AAG5z8XDhBc7mrVOY8HytewLKcbBHdUPkSMBEp61ualX3Xd7RsuxIeCICtLsISR5l4CwrdK+ye12WHDfBWo0T7MhYIoMqXyotyAcj5S3GHhUBONxoE0tnTcj9GlCaejVhd192JtIEA+5l0ZQIDAQABAoIBAQCcWxu3EaN52Y+D8GWHBOwlNg87sAXkymKWMnDkAhLEDwe7dAQaKfVaIuxl1oYmN3hlzLqF2EHsC8+aRwyuVv8AsyWPmFH0MfiMNAYIwpRxvg98Rrw5WmXUl3ciUPRH1eL3ZTbTZjS0eJFivU4nCkbI7ntowKQ87ua0qSneOg/E0NThaOgrkfG13FHTrm2Zl/NDtAWYxhY2sPUk7PAwzktKZSoCdcvHgV9NMjw5Okdu7WML09mErVgNStqtSmEKkiwgA4wD3PVQpZMRjZ3uXK99pggpcehOuhHmMeYwr6PVjp3XSCN6NXq9g6K45zu/eUlN4xYYY3v87JrfqtNd10a9AoGBAOjNgiOkaHk5vmXwmL8BCmHU2verJDsU3U0dT550vUdDwbxFxa1Hu2mnCoxWiOL1XJ+drFIR/TfrhSmG7DiNXeHFxgo1T4qH1shIgWtU49AxFaJ5nXDmmbDtZndiSe1Kavs8mWTEIOUkKzFMnExRNfLdXJKRqf3nyg23PrkRBU1vAoGBANvI7R9zDI6rf4r27JCOeoHj1YC40UGhKjEcfqTa1QseTrxHlvc1AF82P5GDlLT7p/ZbVPLBsb5O1nejJzU+ZtWa/R6y6t1w7wGQUoMWcHLEjzdokHexF4Z7O5lqjwV4xjXA4SPL1qYU7u9YHNvOcAUMli/RRTl+GMSqPL+KsdlrAoGAYSOvU9P8S+kOS1bzRW/Xty6sF3/v5D82gEDkwgeQGzZolni+nSk7SECNJDXPdHRRK8EM8EnVUQoTTFqiIo7KR7TQh93qzLEVrAilB/YqAOsaTetVCJEQPEUy5km68yqELUUB9Ivp9fL7mtyOs0GHs0kuoXHqbqnTI5n8sBgsED0CgYAHWQVMat09m9AsxiqMfqbr1sEZh5Q/XHL7p1c4jbRXpdC+DKqoYxY7GKYxaDO9hThNTSpe30jg8uPKpiK2bfqkI4VY1GRzuMXUyI1pooNp4tZi5NUHm10M+uu4Kk7TwQDnZSZhuvvXG5YNXGXCl3k5Qf13ZciprmSHlqVnRezCNwKBgBBGTlbznNxZEsMVDdRmtry+oOAvXPAAZeHTRF5gasyHWSjjh7PsMcoH70TbOWSI6xxJ5FiPnIa7qdX3SrYisY4Na06NIZihtua9KyS4UpbINvI/8pHEfnWusrDCqVQcWeNe4QzhVNh/Vsd5kDHyLqz3ltHNPcGBIfneFuIS256r"

                    }
                    ;

                gateways.Add(new AlipayGateway(alipayMerchant)
                {
                    GatewayUrl = "https://openapi.alipay.com/gateway.do"
                });
                
                //gateways.Add(new WechatpayGateway(wechatpayMerchant));


                return gateways;
            });

            BundleConfig.RegisterBundles(BundleTable.Bundles);

       
            //scheduleHelper=new ScheduleHelper();
            //scheduleHelper.StartMailService();
            //Thread.Sleep(1000);
            //scheduleHelper.StartLogService();

        }


        protected void Application_End(object sender, EventArgs e)
        {
            //scheduleHelper.End();
        }

    }

}
