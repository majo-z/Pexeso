using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Pexeso
{
    // LocalStorage is a class that provides Save/Load helper methods to interact with the system storage.
    class LocalStorage
    {
        // save every game to local storage
        public static void Save(ObservableCollection<Game> games)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var game in games)
            {
                sb.Append(game.ToLocalStorageFormat() + ";");
            }
            // http://geekswithblogs.net/hmloo/archive/2012/04/08/how-to-remove-the-last-character-from-stringbuilder.aspx
            // remove the last character which is a ";" this results in an extra game when there is just one.
            sb.Length--;
            ApplicationData.Current.LocalSettings.Values["games"] = sb.ToString();
        }

        // load all games from local storage.
        public static ObservableCollection<Game> Load()
        {
            ObservableCollection<Game> games = new ObservableCollection<Game>();
            // no values saved already
            if (ApplicationData.Current.LocalSettings.Values["games"] == null)
            {
                return games; // an empty list of games
            }

            //otherwise, there is some information saved in local storage
            string gamesAsStr = ApplicationData.Current.LocalSettings.Values["games"] as string;

            // 10,Saturday-15,Sunday

            string[] individualGames = gamesAsStr.Split(';');
            foreach (var game in individualGames)
            {
                string[] details = game.Split(',');
                // { 10 Saturday }
                Game g = new Game();  
                g.Date = details[0];
                g.Score = Int32.Parse(details[1]);
                games.Add(g);
            }

            return games;
        }
    }
}
