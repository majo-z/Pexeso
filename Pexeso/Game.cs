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

        public int GameNumber { get; set; }

        public String ToLocalStorageFormat()
        {
            return GameNumber + "," + Score + "," + Date;
        }

        public Game()
        {

        }

        public Game(int gameNo, int score, DateTime date)
        {
            GameNumber = gameNo;           
            Score = score;
            //date formatting...https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            Date = date.ToString("dd-MMM-yyyy") + " - " + date.ToString("H:mm:ss");
        }

    }
}
