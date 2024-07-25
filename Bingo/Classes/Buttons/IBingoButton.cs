using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bingo.Classes.Buttons
{
    public interface IBingoButton
    {
        event EventHandler OnClick;
        int Id { get; set; }
        string Content { get; set; }
        bool CurrentState { get; set; }
    }
}
