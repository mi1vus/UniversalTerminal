﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace UniversalTermnalAPI
{
    public class ShopItemInfo
    {
        public string GroupName { get; set; }
        public string Name { get; set; }
        public decimal Price { get { var res = BasePrice - Discount; return res > 0 ? res : 0; } }

        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public string DiscountInformation { get; set; }
        public string Code { get; set; }
        public string Item { get; set; }

        public string Measure { get; set; }
        public int TaxNumber { get; set; }
        public int Section { get; set; }
        public decimal RestQuantity { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get { return Quantity * Price; } }
        public void SetDiscount(decimal Discont, string Comment = null, bool InPercent = false)
        {
            Discount = InPercent ? Math.Round((BasePrice / 100) * Discont, 2) : Discont;
            DiscountInformation = Comment;
        }
        public override string ToString()
        {
            return $"{Name}: {Price}р * {Quantity}{Measure} = {Amount}р";
        }
    }

    public abstract class Good
    {
        public int Kind { get; set; }
        public string Item { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int TaxID { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public string InternalGroupId { get; set; }
        public string InternalGroupName { get; set; }
        public int InternalSectionId { get; set; }
    }

    public class GoodShop : Good
    {
        public string BatchDate { get; set; }
        public int GroupId { get; set; }
        public string UnitName { get; set; }
        public decimal RestQuantity { get; set; }
    }

    public class GoodService : Good
    {
        public int GroupId { get; set; }
        public string UnitName { get; set; }
    }

    public class GoodFuel : Good
    {
        public int ReturnDepartmentId { get; set; }
        public bool ArbitraryPrice { get; set; }
        public bool Complex { get; set; }
    }

    public class Osnovan
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

    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }

    public enum Hosts
    {
        ACTIVE_TERMINAL = 1
    }
    public enum Operations
    {
        Платеж = 0,
        Подтверждение_платежа = 1,
        Возврат = 2,
        Подтверждение_возврата = 3,
        Коррекция = 4,
        Подтверждение_коррекции = 5,
        Пополнение = 6, 
        Подтверждение_пополнения = 7,
        Открытие_смены = 8,
        Установка = 11,
        Сброс = 12, 
        Аварийный_возврат = 13,
        Транзакция_завершена = 14, 
        Транзакция_отменена = 15            
    }

    public class Discount
    {
        public int DiscountId { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
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

    public class GoodForSale// : Good
    {
        public GoodForSale(Good good)
        {
            Kind = good.Kind;
            Item = good.Item;
            //Name = good.Name;
            //DepartmentId = good.DepartmentId;
            //TaxID = good.TaxID;
            //Price = good.Price;
        }

        public int Kind { get; set; }
        public string Item { get; set; }
        //public int FuellingPointId { get; set; }
        public int PresetMode { get; set; }
        public decimal PresetPrice { get; set; }
        public decimal PresetAmount { get; set; }
        public decimal PresetQuantity { get; set; }
        public int DiscountCount { get; set; }
        public Discount[] Discounts { get; set; }
    }

    public class OsnovanForSale// : Osnovan
    {
        public OsnovanForSale(Osnovan osnavan)
        {
            OsnovanId = osnavan.OsnovanId;
            //Name = osnavan.Name;
            //ShortName = osnavan.ShortName;
            //NoMoneyInReports = osnavan.NoMoneyInReports;
            //ZeroAmountsInCheque = osnavan.ZeroAmountsInCheque;
            //PriceInCheque = osnavan.PriceInCheque;
            //IsDefault = osnavan.IsDefault;
            //IsDisallowed = osnavan.IsDisallowed;
            //IsHidden = osnavan.IsHidden;
            //ForGoodsAndServices = osnavan.ForGoodsAndServices;
            //ForFuels = osnavan.ForFuels;
            //DisallowPrepayMode = osnavan.DisallowPrepayMode;
            //DisallowPostpayMode = osnavan.DisallowPostpayMode;
            //PrintOsnovanName = osnavan.PrintOsnovanName;
            //FuelReturnsToTank = osnavan.FuelReturnsToTank;
            //MaxLitersPreset = osnavan.MaxLitersPreset;
            //MaxMoneyPreset = osnavan.MaxMoneyPreset;
            //DisallowMovePreset = osnavan.DisallowMovePreset;
        }
        public OsnovanForSale(int osnovanId)
        {
            OsnovanId = osnovanId;
            //Name = osnavan.Name;
            //ShortName = osnavan.ShortName;
            //NoMoneyInReports = osnavan.NoMoneyInReports;
            //ZeroAmountsInCheque = osnavan.ZeroAmountsInCheque;
            //PriceInCheque = osnavan.PriceInCheque;
            //IsDefault = osnavan.IsDefault;
            //IsDisallowed = osnavan.IsDisallowed;
            //IsHidden = osnavan.IsHidden;
            //ForGoodsAndServices = osnavan.ForGoodsAndServices;
            //ForFuels = osnavan.ForFuels;
            //DisallowPrepayMode = osnavan.DisallowPrepayMode;
            //DisallowPostpayMode = osnavan.DisallowPostpayMode;
            //PrintOsnovanName = osnavan.PrintOsnovanName;
            //FuelReturnsToTank = osnavan.FuelReturnsToTank;
            //MaxLitersPreset = osnavan.MaxLitersPreset;
            //MaxMoneyPreset = osnavan.MaxMoneyPreset;
            //DisallowMovePreset = osnavan.DisallowMovePreset;
        }
        public int OsnovanId { get; set; }
        //public string CardNumber { get; set; }
    }

    /// <summary>
    /// Initialize variables url and port!
    /// </summary>
    public static class UTAPI
    {
        private static IniParser iniFile;

        private static int request_code = 1;
        //public static int operation_code = 1;

        public static string url = "http://127.0.0.1";
        public static string port = "44310";
        private static string getGoodsList =
"{" + Environment.NewLine +
"  \"Method\": \"GetGoodsList\"" + Environment.NewLine +
"}" + Environment.NewLine;

        private static string getOsnovanList =
"{" + Environment.NewLine +
"  \"Method\": \"GetOsnovanList\"" + Environment.NewLine +
"}" + Environment.NewLine;

        private static string getGoodInfo =
"{" + Environment.NewLine +
"  \"Method\": \"GetGoodInfo\"" + Environment.NewLine +
"  ,\"Item\": \"{0}\"" + Environment.NewLine +
"}" + Environment.NewLine;

        private static string setOrder =
"{" + Environment.NewLine +
"  \"Method\": \"Preset\"" + Environment.NewLine +
"  ,\"Transaction\": {0}" + Environment.NewLine +
"}" + Environment.NewLine;

        //        string req2 =
        //" { " + 
        //"   \"Method\":\"GetFuellingPointConfig\" " + 
        //" } ";
        //        "Method":"Preset"
        //,"Transaction": 

        static UTAPI() {
            ReadCodes();
            iniFile = new IniParser("groups.ini");
        }

        public static ShopItemInfo[] GetGoods(string GroupName = null)
        {
            IEnumerable<Good> selectedGoods = GetGoodsList();
            if (!string.IsNullOrWhiteSpace(GroupName))
                selectedGoods = selectedGoods.Where(t => t.InternalGroupName == GroupName);

            return selectedGoods.Select(t => new ShopItemInfo
            {
                GroupName = t.InternalGroupName,
                Name = t.Name,

                BasePrice = t.Price,
                Code = t.Kind + "_" + t.Item,
                Item = t.Item,

                Measure = t is GoodFuel ? "" : (t is GoodShop) ? (t as GoodShop).UnitName : (t as GoodService).UnitName,
                TaxNumber = t.TaxID,
                Section = t.InternalSectionId,
                RestQuantity = (t as GoodShop)?.RestQuantity ?? 0,
            }).ToArray();
        }

        public static bool SetOrder(ShopItemInfo[] Items, int osnovanId)
        {
            return SetOrder(Items.Select(t=> GoodFabric(t)).ToList(), osnovanId);
        }

        private static Good GoodFabric(ShopItemInfo item)
        {
            Good good = null;

            var ids = item.Code.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (ids.Count() != 2)
                return null;

            int kind = int.Parse(ids[0]);
            string code = ids[1];

            switch (kind)
            {
                case 0:
                    good = new GoodShop() { UnitName = item.Measure, RestQuantity = item.RestQuantity, Quantity = item.Quantity};
                    break;
                case 1:
                    good = new GoodService() { UnitName = item.Measure, Quantity = item.Quantity };
                    break;
                case 2:
                    good = new GoodFuel() { Amount = item.Quantity};
                    break;
                default:
                    return null;
                    break;
            }
            good.Kind = kind;
            good.Item = code;
            good.InternalGroupName = item.GroupName;
            good.Name = item.Name;

            good.Price = item.BasePrice;

            good.TaxID = item.TaxNumber;
            good.InternalSectionId = item.Section;

            good.Discount = item.Discount;

            return good;
        }

        private static List<Good> GetGoodsList()
        {
            try
            {
                var goods_Raw = GET(getGoodsList, request_code);
                ++request_code;
                SaveCodes();
                var listGoods = JsonHelper.ParseGoods(goods_Raw);
                listGoods.ForEach(t => t.InternalGroupName = iniFile.GetSection(t.InternalGroupId));
                listGoods.ForEach(t => t.InternalSectionId = int.Parse(iniFile.GetSetting(t.InternalGroupName, t.Kind + "_" + t.Item)));
                return listGoods;
            }
            catch (Exception ex)
            {
                LogError("GetGoodsList ERROR: " + ex.ToString(), ex.StackTrace);
                return null;
            }
        }

        public static List<Osnovan> GetOsnovanList()
        {
            try
            {
                var osnovans_Raw = GET(getOsnovanList, request_code);
                ++request_code;
                SaveCodes();
                return JsonHelper.ParsegetOsnovans(osnovans_Raw);
            }
            catch (Exception ex)
            {
                LogError("GetOsnovanList ERROR: " + ex.ToString(), ex.StackTrace);
                return null;
            }
        }

        public static Good GetGoodRestInfo(string item)
        {
            try
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
            catch (Exception ex)
            {
                LogError("GetGoodRestInfo ERROR: " + ex.ToString(), ex.StackTrace);
                return null;
            }
        }

        private static bool SetOrder(List<Good> itemsToSale, int osnovanId)
        {
            try
            {
                var gForSale = new GoodsForSale
                {
                    Host = Hosts.ACTIVE_TERMINAL.ToString(),
                    OpCode = (int)Operations.Установка,
                    ItemCount = itemsToSale.Count(),
                    Items = itemsToSale.Select(t => new GoodForSale(t)
                    {
                        //FuellingPointId = 1,
                        PresetMode = t is GoodFuel ? 1 : 0,
                        PresetPrice = ((decimal)t.Price),
                        PresetAmount = t is GoodFuel ? t.Amount : 0,
                        PresetQuantity = t is GoodFuel ? 0 : t.Quantity,
                        DiscountCount = t.Discount == 0 ? 0 : 1,
                        Discounts = t.Discount == 0 ? new Discount[] { } : new Discount[]{ new Discount{
                            DiscountId = 1,
                            DiscountType = 2,
                            DiscountValue = t.Discount
                        } },

                    }).ToArray(),
                    PaymentCount = 1,
                    Payments = new[]
                    {
                        new OsnovanForSale(osnovanId)
                        {
                            //CardNumber = "10101021215414"
                        }
                    }
                };

                var json = new JavaScriptSerializer().Serialize(gForSale);
                var req = setOrder.Replace("{0}", json);
                var order_Raw = GET(req, request_code);
            
                ++request_code;
                //++operation_code;
                SaveCodes();

                var res = JsonHelper.ParseResponseErrors(order_Raw);
                if (res != null)
                {
                    LogError("SetOrder ERROR: " + res.ErrorDescription, "error code: " + res.ErrorCode);
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                LogError("SetOrder ERROR: " + ex.ToString(), ex.StackTrace);
                return false;
            }
        }

        private static bool ReturnOrder(List<Good> itemsToSale, Osnovan osnovan)
        {
            var gForSale = new GoodsForSale
            {
                Host = Hosts.ACTIVE_TERMINAL.ToString(),
                OpCode = (int)Operations.Возврат,
                ItemCount = itemsToSale.Count(),
                Items = itemsToSale.Select(t => new GoodForSale(t)
                {
                    //FuellingPointId = 1,
                    PresetMode = t is GoodFuel ? 1 : 0,
                    PresetPrice = t.Price,
                    PresetAmount = t is GoodFuel ? t.Amount : 0,
                    PresetQuantity = t is GoodFuel ? 0 : t.Quantity,
                    DiscountCount = 1,//t?.Discounts.Count()??0,
                    Discounts = new Discount[]{ new Discount{
                        DiscountId = 1,
                        DiscountType = 2,
                        DiscountValue = t.Discount
                    } }
                }).ToArray(),
                PaymentCount = 1,
                Payments = new[]
                {
                    new OsnovanForSale(osnovan)
                    {
                        //CardNumber = "10101021215414"
                    }
                }
            };

            var json = new JavaScriptSerializer().Serialize(gForSale);
            var req = setOrder.Replace("{0}", json);
            var order_Raw = GET(req, request_code);

            ++request_code;
            //++operation_code;
            SaveCodes();

            return true;
        }

        private static string GET(string req_S, int id)
        {
            var boundary = "------------------------" + DateTime.Now.Ticks;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + ":" + port + "?request_id=" + id);
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
                    LogError("WebException ERROR: " + errorText, ex.StackTrace);
                }
                throw;
            }
        }

        private static void SaveCodes()
        {
                try
                {
                    //lock (OrderEventRequests)
                    File.WriteAllText("codes.swp", $"request_code:{request_code}");
                //;operation_code:{operation_code}");
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
                        //case "operation_code":
                        //    operation_code = int.Parse(vals[1]);
                        //    break;
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

        private static string LogError(string msg, string path)
        {
            bool res = false;
            try
            {
                if (!System.IO.Directory.Exists("Logs/"))
                    System.IO.Directory.CreateDirectory("Logs/");

                var fpath = "Logs/" + DateTime.Today.ToString("dd_MM_yyyy") + ".txt";
                var text = string.Format(
    @"[{0}] - {1}" + Environment.NewLine + "+++ [{2}]" + Environment.NewLine, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), msg, path);
                System.IO.File.AppendAllText(fpath, text);
                res = true;
            }
            catch { }

            return res.ToString();
        }
    }
}
