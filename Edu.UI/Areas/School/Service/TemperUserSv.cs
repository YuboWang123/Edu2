using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Edu.Entity;
using Edu.UI.Models;

namespace Edu.UI.Areas.School.Service
{
    /// <summary>
    /// remove all the changes to db if memo contains 'maker:tester'
    /// </summary>
    /// <typeparam name="TTester"></typeparam>
    public class TemperUserSv<TTester> where TTester:ITester
    {
        public const string StringTester = "maker:tester";




    }
}