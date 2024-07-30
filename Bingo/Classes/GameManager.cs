using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public int RandomValue(GameType gameType, int counter)
        {
            Random random = new Random();
            int max;
            if (gameType == GameType.Numbers)
                max = 75;
            else
                max = counter;
            if (numbers.Count == 74)
                numbers.RemoveAll(x => x <= max);

            int generated;
            do
            {
                generated = random.Next(1, max);
            } while (numbers.Contains(generated));
            numbers.Add(generated);
            return generated;

        }


    }
}