using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pexeso
{
    class Game
    {   //https://msdn.microsoft.com/en-us/library/system.datetime.hour(v=vs.110).aspx
        //date formatting
        public string Date { get; set; }
        public int Score { get; set; }

        public int GameNumber { get; set; }

        public String ToLocalStorageFormat()
        {
            return GameNumber + "," + Date + "," + Score;
        }

        public Game()
        {

        }

        public Game(DateTime date, int score, int gemNo)
        {
            GameNumber = gemNo;
            Date = date.DayOfWeek + " the "  +date.Day + "th";
            Score = score;
        }

    }
}
