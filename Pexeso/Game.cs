using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pexeso
{   //Game classes to represent game history
    class Game
    {
        public string Date { get; set; }
        public int Score { get; set; }

        public int GameNumber { get; set; }

        public int GridSize { get; set; }

        public String ToLocalStorageFormat()
        {
            return GameNumber + "," + Score + "," + GridSize +  "," + Date;
        }

        public Game()
        {

        }

        public Game(int gameNo, int score, int gridSize, DateTime date)
        {
            GameNumber = gameNo;           
            Score = score;
            GridSize = gridSize;
            //date formatting...https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            Date = date.ToString("dd-MMM-yyyy") + " - " + date.ToString("H:mm:ss");
        }

    }
}
