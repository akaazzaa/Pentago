using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pentago
{
    public static class MainModel 
    {
       public static UserControl _content {  get; set; }
       public static System.Windows.Controls.Grid FormLoader { get; set; }

        public static void SetGrid(System.Windows.Controls.Grid grid)
        {
            FormLoader = grid;
        }
        // Ändert das usercontrol
        public static void SetNewContent(UserControl content)
        {
            _content = content;
            FormLoader.Children.Clear();
            FormLoader.Children.Add(_content);
            
        }

       
    }
}

    
