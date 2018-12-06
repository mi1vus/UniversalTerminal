﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalTermnalAPI
{
    class JsonHelper
    {
        public static List<Good> ParseGoods(string src)
        {
            var begInd = src.IndexOf('[', 0);
            var endInd = src.IndexOf(']', 0);

//            if (!src.StartsWith(
//"{\r\n  \"GoodsList\":[") || !src.EndsWith("]\r\n  }"))
//                return null;

            src = src.Substring(begInd + 7, endInd - begInd - 11);
            src = src.Replace("\r\n      ", "");
            src = src.Replace("\r\n    ", "");

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
                        if ((result as Good0) != null)
                            (result as Good0).BatchDate = val == "null" ? null : val;
                        break;
                    }
                    case "GroupId":
                        if ((result as Good0) != null)
                            (result as Good0).GroupId = int.Parse(val);
                        if ((result as Good1) != null)
                            (result as Good1).GroupId = int.Parse(val);
                        break;
                    case "UnitName":
                        if ((result as Good0) != null)
                            (result as Good0).UnitName = val == "null" ? null : val;
                        if ((result as Good1) != null)
                            (result as Good1).UnitName = val == "null" ? null : val;
                        break;
                    case "RestQuantity":
                        if ((result as Good0) != null)
                            (result as Good0).RestQuantity = int.Parse(val);
                        break;
                    case "ReturnDepartmentId":
                        if ((result as Good2) != null)
                            (result as Good2).ReturnDepartmentId = int.Parse(val);
                        break;
                    case "ArbitraryPrice":
                        if ((result as Good2) != null)
                            (result as Good2).ArbitraryPrice = bool.Parse(val);
                        break;
                    case "Complex":
                        if ((result as Good2) != null)
                            (result as Good2).Complex = bool.Parse(val);
                        break;
                }
            }
            return result;
        }


    }
}
