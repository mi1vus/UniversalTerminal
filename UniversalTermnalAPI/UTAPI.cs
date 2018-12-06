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

    public class UTAPI
    {
        int request_id = 0;
        string url = "http://127.0.0.1:44310";

        public List<Good> GetGoodsList()
        {
            var goods_Raw = GET(req1, 14);
            var goods = JsonHelper.ParseGoods(goods_Raw);
            return null;
        }

        string req1 = 
"{" + Environment.NewLine +
"  \"Method\":\"GetGoodsList\"" + Environment.NewLine +
"}" + Environment.NewLine;

//        string req2 =
//" { " + 
//"   \"Method\":\"GetFuellingPointConfig\" " + 
//" } ";
        public UTAPI() {
            var goods = GetGoodsList();
        }

        // Returns JSON string
        string GET(string req_S, int id)
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
