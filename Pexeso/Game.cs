using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pexeso
{
    class Game
    {
        public string Date { get; set; }
        public int Score { get; set; }

        public String ToLocalStorageFormat()
        {
            return Date+ "," + Score;
        }

        public Game()
        {

        }

        public Game(DateTime date, int score)
        {
         
            Date = date.DayOfWeek + " the "  +date.Day + "th";
            Score = score;
        }

    }
}
