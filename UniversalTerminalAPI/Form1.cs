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
            var good = UTAPI.GetGoodRestInfo(goods.Last().Item);
            var osnovs = UTAPI.GetOsnovanList();
            var good = UTAPI.GetGoodRestInfo(goods[9].Item);
        }
    }
}
