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

    public static class UTAPI
    {
        public static int request_id = 63;
        private static string url = "http://127.0.0.1:44310";

        public static List<Good> GetGoodsList()
        {
            var goods_Raw = GET(getGoodsList, request_id);
            ++request_id;
            return JsonHelper.ParseGoods(goods_Raw);
        }

        public static Good GetGoodRestInfo(string item)
        {
            var req = getGoodInfo.Replace("{0}",item);
            var good_Raw = GET(req, request_id);
            ++request_id;
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
    }
}
