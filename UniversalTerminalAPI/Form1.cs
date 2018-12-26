﻿using System;
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

            int skip = 6;
            int take = 2;
            //int amount = 2;
            //int quantity = 2;
            var itemsToSale = goods.Skip(skip).Take(take)
                .ToList();

            for (int i = 0; i < itemsToSale.Count; ++i)
            {
                itemsToSale[i].Discount = 0.3M * (i + 1);
                if (itemsToSale[i] is GoodFuel)
                    itemsToSale[i].Amount = itemsToSale[i].Price * 2.5M;
                else
                    itemsToSale[i].Quantity = 1.0M * (itemsToSale.Count - i);
            }

            itemsToSale = itemsToSale.Where(t => !(t is GoodShop) || (t as GoodShop).RestQuantity >= t.Quantity).ToList();

            // Branch 2
            var sale = UTAPI.SetOrder(itemsToSale, osnovs[osnovs.Count - 1]);
            //var ret = UTAPI.ReturnOrder(itemsToSale, osnovs[osnovs.Count - 1]);
        }
    }
}
