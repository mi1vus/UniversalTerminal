using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTermnalAPI
{
    public abstract class Good
    {
        public int Kind { get; set; }
        public string Item { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int TaxID { get; set; }
        public int Price { get; set; }
    }

    public class Good0 : Good
    {
        public string BatchDate { get; set; }
        public int GroupId { get; set; }
        public string UnitName { get; set; }
        public int RestQuantity { get; set; }
    }

    public class Good1 : Good
    {
        public int GroupId { get; set; }
        public string UnitName { get; set; }
    }

    public class Good2 : Good
    {
        public int ReturnDepartmentId { get; set; }
        public bool ArbitraryPrice { get; set; }
        public bool Complex { get; set; }
    }

    public abstract class Osnovan
    {
        public int OsnovanId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool NoMoneyInReports { get; set; }
        public bool ZeroAmountsInCheque { get; set; }
        public bool PriceInCheque { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDisallowed { get; set; }
        public bool IsHidden { get; set; }
        public bool ForGoodsAndServices { get; set; }
        public bool ForFuels { get; set; }
        public bool DisallowPrepayMode { get; set; }
        public bool DisallowPostpayMode { get; set; }
        public bool PrintOsnovanName { get; set; }
        public bool FuelReturnsToTank { get; set; }
        public int MaxLitersPreset { get; set; }
        public int MaxMoneyPreset { get; set; }
        public bool DisallowMovePreset { get; set; }
    }

    public enum Hosts
    {
        ACTIVE_TERMINAL = 1
    }

    public class GoodsForSale
    {
        public int OpCode;
        public string Host;
        public int ItemCount;
        public GoodForSale[] Items;
        public int PaymentCount;
        public OsnovanForSale[] Payments;
    }

    public class GoodForSale : Good
    {

    }
    public class OsnovanForSale : Osnovan
    {

    }

    public static class UTAPI
    {
        public static int request_code = 1;
        public static int operation_code = 1;

        private static string url = "http://127.0.0.1:44310";

        static UTAPI() {
            ReadCodes();
        }

        public static List<Good> GetGoodsList()
        {
            var goods_Raw = GET(getGoodsList, request_code);
            ++request_code;
            SaveCodes();
            return JsonHelper.ParseGoods(goods_Raw);
        }

        public static List<Osnovan> GetOsnovanList()
        {
            var osnovans_Raw = GET(getOsnovanList, request_code);
            ++request_code;
            SaveCodes();
            return JsonHelper.ParsegetOsnovans(osnovans_Raw);
        }

        public static Good GetGoodRestInfo(string item)
        {
            var req = getGoodInfo.Replace("{0}",item);
            var good_Raw = GET(req, request_code);
            ++request_code;
            SaveCodes();
            int kind = -1;
            if (!good_Raw.Contains("Kind"))
            {
                var goods = GetGoodsList();
                kind = goods.First(t => t.Item == item).Kind;
            }

            return JsonHelper.ParseGoodPrepare(good_Raw, kind);
        }

        public static string getGoodsList = 
"{" + Environment.NewLine +
"  \"Method\": \"GetGoodsList\"" + Environment.NewLine +
"}" + Environment.NewLine;

        public static string getOsnovanList =
"{" + Environment.NewLine +
"  \"Method\": \"GetOsnovanList\"" + Environment.NewLine +
"}" + Environment.NewLine;

        public static string getGoodInfo =
"{" + Environment.NewLine +
"  \"Method\": \"GetGoodInfo\"" + Environment.NewLine +
"  ,\"Item\": \"{0}\"" + Environment.NewLine +
"}" + Environment.NewLine;

    //        string req2 =
    //" { " + 
    //"   \"Method\":\"GetFuellingPointConfig\" " + 
    //" } ";

        // Returns JSON string
        public static string GET(string req_S, int id)
        {
            var boundary = "------------------------" + DateTime.Now.Ticks;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?request_id=" + id);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            try
            {
                using (Stream dataStream = request.GetRequestStream())
                {
                    var reqWriter = new StreamWriter(dataStream);
                    reqWriter.Write(req_S);
                    reqWriter.Flush();
                }

                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }


        private static void SaveCodes()
        {
                try
                {
                    //lock (OrderEventRequests)
                    File.WriteAllText("codes.swp", $"request_code:{request_code};operation_code:{operation_code}");
                    //Serialization.Serialize(OrderEventRequests.ToArray(), "unHistoredOrders.swp");
                }
                catch (Exception ex)
                {
                    //Logger.Write("saveUnHistoredOrders Error: " + ex.Message);
                }
        }

        private static void ReadCodes()
        {
            try
            {
                var src = File.ReadAllText("codes.swp");
                var pairs = src.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries) ;
                foreach (var pair in pairs)
                {
                    var vals = pair.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (vals.Length != 2)
                        throw new Exception("Codes parse error!");

                    switch (vals[0])
                    {
                        case "request_code":
                            request_code = int.Parse(vals[1]);
                            break;
                        case "operation_code":
                            operation_code = int.Parse(vals[1]);
                            break;
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                //Logger.Write("readUnHistoredOrders Error: " + ex.Message);
            }
            return;
        }

    }
}
