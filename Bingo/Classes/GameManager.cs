using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.Classes
{
    public class GameManager
    {

        public GameManager() 
        { 
            numbers = new List<int>();
        }


        private List<int> numbers;

        public int RandomNumber()
        {
            Random random = new Random();
            int generated;
            do
            {
                generated = random.Next(1, 75);
            } while (numbers.Contains(generated));
            numbers.Add(generated);
            return generated;
        }
    }
}
