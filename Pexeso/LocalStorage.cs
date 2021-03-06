﻿using System;
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
        //Deletes all history of games and resets high score
        public static void ClearGames()
        {
            ApplicationData.Current.LocalSettings.Values["games"] = null;
            ApplicationData.Current.LocalSettings.Values["highScore"] = null;
        }

        // save the high score
        public static void SaveHighScore(int highScore)
        {
            ApplicationData.Current.LocalSettings.Values["highScore"] = highScore;
        }

        public static int LoadHighScore()
        {
            // no high score has been saved
            if (ApplicationData.Current.LocalSettings.Values["highScore"] == null)
            {
                return 0;
            }

            return (int) ApplicationData.Current.LocalSettings.Values["highScore"];
        }

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
            if (sb.Length > 1)
            {
                sb.Length--;
            }       
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

            if (gamesAsStr != null)
            {
                string[] individualGames = gamesAsStr.Split(';'); // { 10,Saturday;15,Sunday }
                foreach (var game in individualGames)
                {
                    string[] details = game.Split(','); // { 10 Saturday }
                    Game g = new Game();
                    g.GameNumber = Int32.Parse(details[0]);
                    g.Score = Int32.Parse(details[1]);
                    g.GridSize = Int32.Parse(details[2]);
                    g.Date = details[3];
                
                    games.Add(g);
                }
            }

            return games;
        }
    }
}
