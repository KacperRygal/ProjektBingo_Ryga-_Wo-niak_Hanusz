using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bingo.Classes.Buttons
{
    public abstract class ButtonFactory
    {
        public abstract IBingoButton CreateButton();
    }
}
