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
            var goods = UTAPI.GetGoods();
            //var goodsL = UTAPI.GetGoodsList();
            var good = UTAPI.GetGoodRestInfo(goods[9].Item);
            var osnovs = UTAPI.GetOsnovanList();

            //mast
            //JIRA Comment
            int skip = 7;
            int take = 2;
            //int amount = 2;
            //int quantity = 2;
            var itemsToSale = goods.Skip(skip).Take(take)
                .ToArray();
            //var itemsToSaleL = goodsL.Skip(skip).Take(take)
            //    .ToList();

            //
            //UT-2 commi t
            for (int i = 0; i < itemsToSale.Count(); ++i)
            {
                //br2
                itemsToSale[i].SetDiscount(0.3M * (i + 1));
                itemsToSale[i].Quantity = 1.0M * (itemsToSale.Count() - i);
                //itemsToSaleL[i].Discount = 0.3M * (i + 1);
                //if (itemsToSaleL[i] is GoodFuel)
                //    itemsToSaleL[i].Amount = itemsToSale[i].Price * 2.5M;
                //else
                //    itemsToSaleL[i].Quantity = 1.0M * (itemsToSaleL.Count - i);
            }
            //UT-4
            itemsToSale = itemsToSale.Where(t => /*!(t is GoodShop) || (t as GoodShop).*/t.RestQuantity >= t.Quantity).ToArray();

            var sale = UTAPI.SetOrder(itemsToSale, /*new Osnovan { OsnovanId = 23} );// */osnovs[osnovs.Count - 1].OsnovanId);
            //var saleL = UTAPI.SetOrder(itemsToSaleL, /* new Osnovan { OsnovanId = 23} );//*/osnovs[osnovs.Count - 1].OsnovanId);
            //var ret = UTAPI.ReturnOrder(itemsToSale, osnovs[osnovs.Count - 1]);
        }
    }
}
