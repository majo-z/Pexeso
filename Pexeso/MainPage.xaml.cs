using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Pexeso
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Observable collection holds game objects that the ListView displays in the Game History page
        // https://www.c-sharpcorner.com/UploadFile/5ef5aa/binding-collection-to-listview-control-in-uwp-explained/
        private ObservableCollection<Game> _gameHistory; 
        // keep track of current score
        private int _currentScore;
        private Random _rnd = new Random();
        private Rectangle _firstRectangle;
        private Rectangle _secondRectangle;
        private int _clickNo;
        private int _gridSize;
        private int _totalTilesLeft;
        public MainPage()
        {
            InitializeComponent();
            _gameHistory = new ObservableCollection<Game>(); // read from local storage.
             _gameHistory.Add(new Game(DateTime.Now, 10));
             _gameHistory.Add(new Game(DateTime.Now, 20));
            ListView.ItemsSource = _gameHistory;
            _gridSize = 2;
            InitGrid(_gridSize);
            //FillPositions(_gridSize);
            // GeneratePossiblePositions(8);
        }

        //crate and draw rectangles
        private void InitGrid(int size)
        {
            for (int i = 0; i < size; i++){
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        
        // create set of all possible positions
        private List<string> GeneratePossiblePositions(int size)
        {

            var list = new List<string>();

            for (var row = 0; row < size; row++)
            {
                for (var col = 0; col < size; col++)
                {
                    list.Add(row + "_" + col);
                }

            }

            //shuffle list
            //https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052
            var random = new Random();

            var shuffledList = list.OrderBy(x => random.Next()).ToList();

            return shuffledList;

        }
        
        private Rectangle GenerateRectangle(int imageNum, string tag)
        {
            Rectangle rect = new Rectangle();
            ImageBrush brush = new ImageBrush(); // imagebrush is an object
            Uri uri = new Uri("ms-appx:///Assets/Images/" + imageNum + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage bitmap = new BitmapImage(uri);

            brush.ImageSource = bitmap;

            rect.Fill = brush;
            rect.Tag = tag; // assign id as the tag

            return rect;
        }

        private void AddRectangleToGrid(Rectangle rect, int row, int col)
        {
            rect.SetValue(Grid.RowProperty, row);
            rect.SetValue(Grid.ColumnProperty, col);
            rect.Width = GameGrid.Width / _gridSize;
            rect.Height = GameGrid.Height / _gridSize;
            GameGrid.Children.Add(rect);
        }

        private void FillPositions(int size)
        {
            // get all possible positions
      
            var usedNumbers = new HashSet<int>(); // keep track of all the image numbers used so far
            var allPositions = new Stack<string>(GeneratePossiblePositions(size));
            while (allPositions.Count > 0) // keep going until no positions are left
            {
              
                

                // https://stackoverflow.com/questions/2706500/how-do-i-generate-a-random-int-number-in-c
                
                int imageNum = _rnd.Next(1, 62);   // creates a number between 1 and 61

                // ensure that there is only one pair of each image.
                while (usedNumbers.Contains(imageNum))
                {
                    imageNum = _rnd.Next(1, 62);
                }

                usedNumbers.Add(imageNum);

                // create 2 rectanges and connect them
                var rect1 = GenerateRectangle(1, imageNum.ToString());

                var rect2 = GenerateRectangle(1, imageNum.ToString());
           

                // take two off the top
                string first = allPositions.Pop(); // "0_0"
                string second = allPositions.Pop(); // "1_6" 

                // https://stackoverflow.com/questions/8928601/how-can-i-split-a-string-with-a-string-delimiter
                var row1Col1 = first.Split('_');//string[] row1Col1 = first.Split('_');
                var row2Col2 = second.Split('_'); // string[] ro2Col2 = second.Split('_');

                // https://stackoverflow.com/questions/1019793/how-can-i-convert-string-to-int
                int row1 = Int32.Parse(row1Col1[0]);
                int col1 = Int32.Parse(row1Col1[1]);
                int row2 = Int32.Parse(row2Col2[0]);
                int col2 = Int32.Parse(row2Col2[1]);

                rect1.Tapped += Rectangle_Tapped;
                rect2.Tapped += Rectangle_Tapped;


                AddRectangleToGrid(rect1, row1, col1);
                AddRectangleToGrid(rect2, row2, col2);
                // place them on the grid

            }

            // calculate the total number of tiles that are in the grid.
            _totalTilesLeft = _gridSize * _gridSize;

        }

        //prevents filckering old images when reset the game(clear caches)
        private void ResetImages()
        {
            GameGrid.RowDefinitions.Clear(); // delete all the rows
            GameGrid.ColumnDefinitions.Clear(); // delete all the columns
            //create new grit size and fill the positions with new tiles
            InitGrid(_gridSize);
            FillPositions(_gridSize);

            GameGrid.RowDefinitions.Clear(); // delete all the rows
            GameGrid.ColumnDefinitions.Clear(); // delete all the columns
            //create new grit size and fill the positions with new tiles
            InitGrid(_gridSize);
            FillPositions(_gridSize);
        }


        //game reset button
        private void GameReset_Click(object sender, RoutedEventArgs e)
        {
            ResetImages();
            _clickNo = 0;
            _currentScore = 0;
            CurrentScore.Text = "Current Score: " + _currentScore;

        }

        private void ToggleImage(Rectangle rect)
        {
            ImageBrush brush = new ImageBrush(); // imagebrush is an object
            Uri uri = new Uri("ms-appx:///Assets/Images/" + rect.Tag.ToString() + ".png", UriKind.RelativeOrAbsolute);
            BitmapImage bitmap = new BitmapImage(uri);

            brush.ImageSource = bitmap;

            rect.Fill = brush;
        }

        private void SetToDefault(Rectangle rect)
        {
            ImageBrush brush = new ImageBrush(); // imagebrush is an object
            Uri uri = new Uri("ms-appx:///Assets/Images/1.png", UriKind.RelativeOrAbsolute);
            BitmapImage bitmap = new BitmapImage(uri);

            brush.ImageSource = bitmap;

            rect.Fill = brush;
        }

        // this method gets called when a user taps a rectangle
        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // get the rectangle that was clicked
            var rect = (Rectangle) sender;
            // on first click, the tile flips
            if (_clickNo == 0)
            {
                ToggleImage(rect);
                _firstRectangle = rect;
                _clickNo = 1;
            }
            else if (_clickNo == 1)
            {  // on second click, tile flips
                if (_firstRectangle == rect)
                {
                    return;
                }
                ToggleImage(rect);
                _secondRectangle = rect;
                _clickNo = 2;

                bool gameOver = _totalTilesLeft == 2;
                if (gameOver)
                {
                    _currentScore += 10;
                    CurrentScore.Text = "Current Score: " + _currentScore;
                    // TODO handle end of game
                    // adding a game to the game history observable collection will make it appear in the game history page.
                    _gameHistory.Add(new Game(DateTime.Now, _currentScore));
                    // reset grid, display high score


                    return;
                }

            }
            else if (_clickNo == 2)
            { // ton 3rd click, check the previous 2 rectangles to see if they match
                if (rect == _firstRectangle || rect ==_secondRectangle)
                { // ignore clicks on the same two rectangles.
                    return;
                }
                bool rectanglesMatch = _firstRectangle.Tag.ToString() == _secondRectangle.Tag.ToString();
                if (rectanglesMatch)
                {
                    // remove the evant handler so it can't be clicked again
                    _firstRectangle.Tapped -= Rectangle_Tapped;
                    _secondRectangle.Tapped -= Rectangle_Tapped;
                    _totalTilesLeft -= 2; // we have 2 fewer tiles.
                    _currentScore += 10; // user gets 10 points when getting a match
                }
                else
                {
                    // reset the 2 images that didn't match
                    SetToDefault(_firstRectangle);
                    SetToDefault(_secondRectangle);
                    _currentScore -= 5; // user loses 5 if the make a mistake.
                    // the minimum score is 0, prevent negative scores.
                    if (_currentScore < 0)
                    {
                        _currentScore = 0;
                    }

                }
                ToggleImage(rect); // display the new image
                _firstRectangle = rect; // save it
                _clickNo = 1;
            }
           
            // if they're the same. They stay up
            // if they're not the same. Thty stay up
            // on a third click, if they were the same, the stay up -> remove event handler
            // if they weren't the same, they go down. -> keep event handler

            CurrentScore.Text = "Current Score: " + _currentScore;

        }
    }
}
