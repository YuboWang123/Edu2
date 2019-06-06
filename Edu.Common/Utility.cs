using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Encoder = System.Text.Encoder;

namespace Edu.Common
{
    public sealed class Utility
    {

        #region Data

        public static List<string> GetLetters(int Length = 4)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < Length; i++)
            {
                byte[] array = new byte[1];
                array[0] = (byte)(65 + i); //ASCII码强制转换二进制
                result.Add(Encoding.ASCII.GetString(array));
            }
            return result;
        }
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#xa0;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring.Replace("&emsp;", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        public  static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts.Minutes;
        }

        public static string Encrypt(string text)
        {
            var md5 = MD5.Create();
            byte[] enTxt =ASCIIEncoding.UTF8.GetBytes(text);
            byte[] s = md5.ComputeHash(enTxt);
            StringBuilder sb=new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                sb.Append(s[i].ToString("x2"));
            }

            return sb.ToString();
        }




            /// <summary>
            /// Base64加密
            /// </summary>
            /// <param name="encode">加密采用的编码方式</param>
            /// <param name="source">待加密的明文</param>
            /// <returns></returns>
            public static string EncodeBase64(Encoding encode, string source)
            {
                string enString = "";
                byte[] bytes = encode.GetBytes(source);
                try
                {
                    enString = Convert.ToBase64String(bytes);
                }
                catch
                {
                    enString = source;
                }
                return enString;
            }

            /// <summary>
            /// Base64加密，采用utf8编码方式加密
            /// </summary>
            /// <param name="source">待加密的明文</param>
            /// <returns>加密后的字符串</returns>
            public static string EncodeBase64(string source)
            {
                return EncodeBase64(Encoding.UTF8, source);
            }

            /// <summary>
            /// Base64解密
            /// </summary>
            /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
            /// <param name="result">待解密的密文</param>
            /// <returns>解密后的字符串</returns>
            public static string DecodeBase64(Encoding encode, string result)
            {
                string decode = "";
                byte[] bytes = Convert.FromBase64String(result);
                try
                {
                    decode = encode.GetString(bytes);
                }
                catch
                {
                    decode = result;
                }
                return decode;
            }

            /// <summary>
            /// Base64解密，采用utf8编码方式解密
            /// </summary>
            /// <param name="result">待解密的密文</param>
            /// <returns>解密后的字符串</returns>
            public static string DecodeBase64(string result)
            {
                return DecodeBase64(Encoding.UTF8, result);
            }
        

        #endregion

        #region File
        // <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="filepath">待删除的文件相对路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string filepath)
        {
            return DeleteFile(filepath, false);
        }

        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="filepath">待删除的文件路径</param>
        /// <param name="absolute">是否绝对路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string filepath, bool absolute)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return false;
            }
            string fullpath = filepath;
            if (!absolute)
            {
                fullpath = GetMapPath(fullpath);
            }
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
                return true;
            }
            return false;
        }
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion


        #region Image

        #endregion

        /// <summary>
        /// 返回分页页码-js使用
        /// </summary>
        public static string HtmlPager(int pageSize, int pageIndex, int totalCount, int centSize)
        {
            //计算页数
            if (totalCount < 1 || pageSize < 1)
            {
                return "";
            }
            int pageCount = totalCount / pageSize;
            if (pageCount < 1)
            {
                return "";
            }
            if (totalCount % pageSize > 0)
            {
                pageCount += 1;
            }
            if (pageCount <= 1)
            {
                return "";
            }
            StringBuilder pageStr = new StringBuilder();

            string firstBtn = "<a href='javascript:GetPageList(" + (pageIndex - 1).ToString() + ", " + pageSize + ");'>«上一页</a>";
            string lastBtn = "<a href='javascript:GetPageList(" + (pageIndex + 1).ToString() + ", " + pageSize + ");'>下一页»</a>";
            string firstStr = "<a href='javascript:GetPageList(1, " + pageSize + ");'>1</a>";
            string lastStr = "<a href='javascript:GetPageList(" + pageCount.ToString() + ", " + pageSize + ");'>" + pageCount.ToString() + "</a>";

            if (pageIndex <= 1)
            {
                firstBtn = "<span class=\"disabled\">«上一页</span>";
            }
            if (pageIndex >= pageCount)
            {
                lastBtn = "<span class=\"disabled\">下一页»</span>";
            }
            if (pageIndex == 1)
            {
                firstStr = "<span class=\"current\">1</span>";
            }
            if (pageIndex == pageCount)
            {
                lastStr = "<span class=\"current\">" + pageCount.ToString() + "</span>";
            }
            int firstNum = pageIndex - (centSize / 2); //中间开始的页码
            if (pageIndex < centSize)
                firstNum = 2;
            int lastNum = pageIndex + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= pageCount)
                lastNum = pageCount - 1;
            pageStr.Append("<span>共" + totalCount + "条记录</span>");
            pageStr.Append(firstBtn + firstStr);
            if (pageIndex >= centSize)
            {
                pageStr.Append("<span>...</span>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == pageIndex)
                {
                    pageStr.Append("<span class=\"current\">" + i + "</span>");
                }
                else
                {
                    pageStr.Append("<a href='javascript:GetPageList(" + i.ToString() + ", " + pageSize + ");'>" + i + "</a>");
                }
            }
            if (pageCount - pageIndex > centSize - ((centSize / 2)))
            {
                pageStr.Append("<span>...</span>");
            }
            pageStr.Append(lastStr + lastBtn);
            return pageStr.ToString();
        }


        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Transparent), -1, -1, image.Width + 1, image.Height + 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #region Regex

        #endregion
    }
}
