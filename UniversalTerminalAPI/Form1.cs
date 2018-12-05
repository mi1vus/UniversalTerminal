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
        UTAPI API = new UTAPI();
        public Form1()
        {
            InitializeComponent();

        }
    }
}
