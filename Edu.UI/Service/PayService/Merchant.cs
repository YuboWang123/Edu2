using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using Autofac;
using  Autofac.Builder;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using PaySharp.Alipay;
using PaySharp.Core;
using PaySharp.Wechatpay;

namespace Edu.UI.Service.PayService
{
    public class Merchant
    {

        //public static void Register(Type type, ContainerBuilder containerBuilder, System.Func<IComponentContext, IGateways> func)
        //{
        //    containerBuilder.Register(func).InstancePerRequest();
        //    containerBuilder.RegisterControllers(type.Assembly);
        //    var container = containerBuilder.Build();
        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        //}


        public static void Register(Type type, ContainerBuilder containerBuilder, HttpConfiguration config, System.Func<IComponentContext, IGateways> func)
        {
            containerBuilder.Register(func).InstancePerRequest();
            containerBuilder.RegisterApiControllers(type.Assembly);

            var container = containerBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }




    }
}