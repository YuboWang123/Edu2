using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Edu.BLL.TrainBase;
using Edu.Entity;
using Edu.Entity.SchoolFinance;
using Wyb.DbUtility;

namespace Edu.BLL.SchoolFinance
{
    /// <summary>
    ///   gerate new year card list.
    ///   bulk delete fincard.
    /// </summary>
    public class StudyCard
    {
        #region class vars
        protected FINCardConfig _config;
        protected FINCardBLL _BLL;
        private string _configId; 
        #endregion

        public StudyCard()
        {
            _BLL=new FINCardBLL();
        }
    

        public StudyCard(string configId):this()
        {
            _configId = configId;
            _config=new FINCardConfigBLL().SingleCardConfig(configId);
        }

        

        public string GenPassword(int l)
        {
            string a = DateTime.Now.ToFileTime().ToString() + Guid.NewGuid().GetHashCode().ToString().Trim('-');
            char[] s = a.ToCharArray();
            char[] n = new char[l];
            Random rnd = new Random();
            for (int i = 0; i < l; i++)
            {
                n[i] = s[rnd.Next(s.Length)];
            }
            return string.Join("", n);

        }

        /// <summary>
        /// generate card list by config.
        /// </summary>
        /// <returns></returns>


        protected static string GenCardNo(int len, int i)
        {
            string CN = DateTime.Now.ToFileTime().ToString() + i.ToString().PadLeft(4, '0'); //max number is 1000

            return CN.Substring(CN.Length - len, len);
        }

        private int Gen(int pwdlen, int cardNoLen, int count)
        {
            if (_config == null)
            {
                throw new InvalidOperationException("no config found");
            }

            FINCard newCard;
            List<FINCard> list=new  List<FINCard>();

            //need to know card with the same prefix count number.
            int i =  _BLL.CountCardsExist(_configId);
            int r = i + count;
            for (; i <r; i++)
            {
                newCard=new  FINCard();
                newCard.Id = _config.CardPrefix + GenCardNo(cardNoLen, i+1);
                newCard.CardConfigId = _configId;
                newCard.Password = GenPassword(pwdlen);
                newCard.Status = AppConfigs.SingleCardStatus.NeverUsed;
                newCard.StatusDay=DateTime.Now;
                newCard.FailTimes = 0;
                list.Add(newCard);
                Thread.Sleep(3);
            }
            
            return  this._BLL.BulkAdd(GenCardListTable(list))?1:0;
     
        }
        /// <summary>
        /// generate list for new card list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable GenCardListTable(IEnumerable<FINCard> list)
        {
            DataTable dt = GenTableHeader();

            if (list.Count() > 0)
            {
                foreach (var rc in list)
                {
                    DataRow dr = dt.NewRow();
                    dr["Id"] = rc.Id;
                    dr["UserId"] = rc.UserId;
                    dr["Memo"] = rc.Memo;
                    dr["FailTimes"] = rc.FailTimes;
                    dr["CardConfigId"] = rc.CardConfigId;
                    dr["Password"] = rc.Password;
                    dr["Status"] = rc.Status;
                    dr["StatusDay"] = rc.StatusDay;
                  
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public DataTable GenTableHeader()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("CardConfigId", typeof(string));
            dt.Columns.Add("UserId", typeof(string));
            dt.Columns.Add("Memo", typeof(string));
            dt.Columns.Add("FailTimes", typeof(int));
            dt.Columns.Add("Password", typeof(string));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("StatusDay", typeof(DateTime));
            return dt;

        }
   

        /// <summary>
        /// 异步生成学习卡
        /// </summary>
        /// <param name="pwdLen"></param>
        /// <param name="pref"></param>
        /// <param name="cardNoLen"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        public Task<int> genCardListAsync(CardGenParams gParams)
        {
            if (string.IsNullOrEmpty(_configId))
            {
                throw new ArgumentNullException("config id not found.");
            }
           return Task.Factory.StartNew(() => { return Gen(gParams.pswLen, gParams.cardNoLen, gParams.count); });
        }

   

        /// <summary>
        ///make excel table readable.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        protected DataTable GetTableCN(DataTable tb)
        {
            tb.Columns.Remove("CardConfigId");
            tb.TableName = "test";
            foreach (DataColumn cl in tb.Columns)
            {
                if (cl.ColumnName == "Id") { cl.ColumnName = "卡号"; };
                if (cl.ColumnName == "Password") { cl.ColumnName = "密码"; }
                if (cl.ColumnName == "Status") { cl.ColumnName = "状态"; }
                if (cl.ColumnName == "StatusDay") { cl.ColumnName = "状态日期"; }
                if (cl.ColumnName == "ActivedDay") { cl.ColumnName = "激活日"; }
                if (cl.ColumnName == "Memo") { cl.ColumnName = "用户邮箱"; }
               // if (cl.ColumnName == "DepositPayType") { cl.ColumnName = "支付方式"; }
               // if (cl.ColumnName == "DepositDay") { cl.ColumnName = "付款日"; }
            }
            return tb;
        }

        protected DataTable GetExcelCardList()
        {

            if (_config != null)
            {
               return _BLL.QueryByConfig(_configId);
            }


            throw new NotImplementedException();
        }

        /// <summary>
        /// get excel data of a configed card list.
        /// </summary>
        /// <returns></returns>
        public byte[] getExcelStream()
        {
            if (_config.Id != null)
            {
                DataTable dt = GetExcelCardList();
                if (dt != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        //var shit= wb.AddWorksheet("test");
                        
                        wb.Worksheets.Add(GetTableCN(dt));

                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;
                        using (var MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.Position = 0;
                            return MyMemoryStream.ToArray();
                        }
                    }
                }
                else
                {
                    return new byte[0];
                }
            
            }
            else
            {
                return new byte[0];
            }

        }
        
        private string DelCardList(IList<string> IdList)
        {
            string w = "1=1";
            if (IdList.Count > 0)
            {
                w = string.Join("\',\'", IdList);
                w = "'" + w + "'";
            }

            return w;
        }

        /// <summary>
        /// delete cardlist records
        /// </summary>
        /// <param name="IdList"></param>
        /// <returns></returns>
        public int BulkDelCard(IList<string> idList)
        {
            var ids = DelCardList(idList);
            return this._BLL.BulkDel(ids);
        }

       
     

    }

}
