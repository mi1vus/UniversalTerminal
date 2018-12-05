using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTermnalAPI
{
    public class Good
    {
    
    
    }

    public class UTAPI
    {
        int request_id = 0;
        string url = "http://127.0.0.1:44310";

        public List<Good> GetGoodsList()
        {
            return null;
        }

        string req1 = 
"{" + Environment.NewLine +
"  \"Method\": \"GetGoodsList\"" + Environment.NewLine +
"}" + Environment.NewLine;
        string req2 =
" { " + 
"   \"Method\":\"GetFuellingPointConfig\" " + 
" } ";
        public UTAPI() {
            var res = GET("http://127.0.0.1:44310?request_id=13");
        }

        // Returns JSON string
        string GET(string url)
        {
            var boundary = "------------------------" + DateTime.Now.Ticks;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            try
            {
                //byte[] reqData = Encoding.UTF8.GetBytes(req);
                using (Stream dataStream = request.GetRequestStream())
                {
                    var reqWriter = new StreamWriter(dataStream);
                    reqWriter.Write(req1);
                    reqWriter.Flush();

                    //dataStream.Write(reqData, 0, reqData.Length);
                }

                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }

                //byte[] reqData = Encoding.UTF8.GetBytes(postData);
                //using (Stream dataStream = req.GetRequestStream())
                //{
                //    dataStream.Write(reqData, 0, reqData.Length);
                //}
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
