using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Astana
{
    public class MyDataGrid : DataGridView
    {
        protected override bool DoubleBuffered { get => true;}
    }
}
