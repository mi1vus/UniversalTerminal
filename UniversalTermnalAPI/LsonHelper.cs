using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTermnalAPI
{
    class LsonHelper
    {
        public static List<Good> ParseGoods(string src)
        {
            var begInd = src.IndexOf('[', 0);
            var endInd = src.IndexOf(']', 0);

//            if (!src.StartsWith(
//"{\r\n  \"GoodsList\":[") || !src.EndsWith("]\r\n  }"))
//                return null;

            src = src.Substring(begInd, src.Length - endInd);

            var result = new List<Good>();

            var objs = src.Split(new[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
            var count = 0;
            foreach (var g in objs)
            {
                try
                {
                    if (count > 0 && count < objs.Count() - 1)
                        result.Add(ParseGood("{" + g + "}"));
                    else if (count == objs.Count() - 1)
                        result.Add(ParseGood("{" + g));
                    else
                        result.Add(ParseGood(g + "}"));
                    ++count;
                }
                catch
                {
                    continue;
                }
            }
            return result;
        }
        public static Good ParseGood(string src)
        {
            Good result = null;
            int kind = -1;

            if (!src.StartsWith("{") || !src.EndsWith("}"))
                return null;
            src = src.Substring(1, src.Count() - 2);

            var parameters = src.Split(new[] { ",\"" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in parameters)
            {
                var values = pair.Split(new[] { "\":" }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Count() != 2)
                    continue;

                var nm = values[0];
                if (nm.StartsWith("\""))
                    nm = nm.Substring(1, nm.Count() - 1);
                if (nm.EndsWith("\""))
                    nm = nm.Substring(0, nm.Count() - 1);

                var val = values[1];
                if (val.StartsWith("\""))
                    val = val.Substring(1, val.Count() - 1);
                if (val.EndsWith("\""))
                    val = val.Substring(0, val.Count() - 1);


                switch (nm)
                {
                    case "Kind":
                        kind = int.Parse(val);
                        switch (kind)
                        {
                            case 0:
                                result = new Good0();
                                break;
                            case 1:
                                result = new Good1();
                                break;
                            case 2:
                                result = new Good2();
                                break;
                        }
                        break;
                    case "Item":
                        if (result != null)
                            result.Item = val == "null" ? null : val;
                        break;
                    case "Name":
                        if (result != null)
                            result.Name = val == "null" ? null : val;
                        break;
                    case "DepartmentId":
                        if (result != null)
                            result.DepartmentId = int.Parse(val);
                        break;
                    case "TaxID":
                        if (result != null)
                            result.TaxID = int.Parse(val);
                        break;
                    case "Price":
                        if (result != null)
                            result.Price = int.Parse(val);
                        break;
                    case "BatchDate":
                    {
                        var 
                        if (result != null)
                            result.BatchDate = val == "null" ? null : val;
                        break;
                    }
                    case "Mail":
                        if (result != null)
                            result.Mail = val == "null" ? null : val;
                        break;
                    case "Work":
                        if (result != null)
                            result.Work = val == "null" ? null : val;
                        break;
                    case "Status":
                        if (result != null)
                            result.Status = val == "null" ? null : val;
                        break;
                    case "Deviz":
                        result.Deviz = val == "null" ? null : val;
                        break;
                    case "Parties":
                        result.Parties = val == "null" ? null : val;
                        break;
                    case "PolitExp":
                        result.PolitExp = val == "null" ? null : val;
                        break;
                    case "Female":
                        result.Female = bool.Parse(val);
                        break;
                    case "Alcohol":
                        result.Alcohol = bool.Parse(val);
                        break;
                    case "Smoking":
                        result.Smoking = bool.Parse(val);
                        break;
                    case "Religions":
                        result.Religions = val == "null" ? null : val;
                        break;
                    case "ExamPass":
                        result.ExamPass = val == "null" ? null : val;
                        break;
                    case "Stage":
                        result.Stage = int.Parse(val);
                        break;
                    case "State":
                        result.State = (Models.TaskStates)int.Parse(val);
                        break;
                    case "Score":
                        result.Score = int.Parse(val);
                        break;
                }
            }
            return result;
        }


    }
}
