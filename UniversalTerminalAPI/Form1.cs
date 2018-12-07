using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalTermnalAPI;

namespace UniversalTerminalAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var goods = UTAPI.GetGoodsList();
            var good = UTAPI.GetGoodRestInfo(goods[9].Item);
            var osnovs = UTAPI.GetOsnovanList();

            var gForSale = new GoodsForSale
            {
                Host = Hosts.ACTIVE_TERMINAL.ToString(),
                OpCode = 0,
                ItemCount = goods.Count(),
                Items = goods.Select(t=> new GoodForSale(t) {
                    FuellingPointId = 1,
                    PresetMode = 1,
                    PresetPrice = ((decimal)t.Price) / 100,
                    PresetAmount = 1,
                    DiscountCount = 1,
                    Discounts = new[] 
                    {
                        new Discount() {
                            DiscountId = 1,
                            DiscountType = 2,
                            DiscountValue = 0.01M
                        }
                    }
                }).ToArray(),
                PaymentCount = 1,
                Payments = new[] 
                {
                    new OsnovanForSale(osnovs[3])
                    {
                        CardNumber = "10101021215414"
                    }
                }
            };
            var sale = UTAPI.SetOrder(gForSale);
        }
    }
}
