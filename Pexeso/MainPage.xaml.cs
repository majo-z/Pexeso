using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        private readonly ObservableCollection<Game> _gameHistory; 
        // keep track of current game score
        private int _currentScore;

        private readonly Random _rnd = new Random();
        private Rectangle _firstRectangle;
        private Rectangle _secondRectangle;
        private int _clickNo;
        private int _gridSize;
        private int _totalTilesLeft;
        public MainPage()
        {
            InitializeComponent();
             //ApplicationData.Current.LocalSettings.Values["games"] = null;//enable to wipe out the local storage
            _gameHistory = LocalStorage.Load(); // read from local storage and populate the observable collection.
             //_gameHistory.Add(new Game(DateTime.Now, 10));
             //_gameHistory.Add(new Game(DateTime.Now, 20));
            ListView.ItemsSource = _gameHistory;
            _gridSize = 4;
            //InitGrid(_gridSize);
            DisplayScores();
            //FillPositions(_gridSize);
            // GeneratePossiblePositions(8);
        }

        //crate and draw rectangles
        private static void InitGrid(int size, Grid grid)
        {
            for (var i = 0; i < size; i++){
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        
        // create set of all possible positions
        private static IEnumerable<string> GeneratePossiblePositions(int size)
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
        
        private static Rectangle GenerateRectangle(int num, string tag)
        {
            var rect = new Rectangle();
            var brush = new ImageBrush(); // imagebrush is an object
            var uri = new Uri("ms-appx:///Assets/Images/" + num + ".png", UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage(uri);

            brush.ImageSource = bitmap;

            rect.Fill = brush;
            rect.Tag = tag; // assign id as the tag

            return rect;
        }

        private void AddRectangleToGrid(FrameworkElement rect, int row, int col, Panel grid)
        {
            rect.SetValue(Grid.RowProperty, row);
            rect.SetValue(Grid.ColumnProperty, col);
            rect.Width = grid.Width / _gridSize;
            rect.Height = grid.Height / _gridSize;
            grid.Children.Add(rect);

        }

        private void FillPositions(int size, Panel grid)
        {
            // get all possible positions
      
            var usedNumbers = new HashSet<int>(); // keep track of all the image numbers used so far
            var allPositions = new Stack<string>(GeneratePossiblePositions(size));
            while (allPositions.Count > 0) // keep going until no positions are left
            {
              
                // https://stackoverflow.com/questions/2706500/how-do-i-generate-a-random-int-number-in-c
                
                var imageNum = _rnd.Next(1, 61); // creates a number between 1 and 61

                // ensure that there is only one pair of each image.
                while (usedNumbers.Contains(imageNum))
                {
                    imageNum = _rnd.Next(1, 61);
                }

                usedNumbers.Add(imageNum);

                // create 2 rectanges and connect them
                var rect1 = GenerateRectangle(imageNum, imageNum.ToString());

                var rect2 = GenerateRectangle(imageNum, imageNum.ToString());
           

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

                //rect1.Tapped += Rectangle_Tapped; //==============================================================================
                //rect2.Tapped += Rectangle_Tapped;//===========================================================================================================

                AddRectangleToGrid(rect1, row1, col1, grid);
                AddRectangleToGrid(rect2, row2, col2, grid);
                // place them on the grid

            }

            // calculate the total number of tiles that are in the grid.
            _totalTilesLeft = _gridSize * _gridSize;
        }


        //New game button
        private async void NewGame_Click(object sender, RoutedEventArgs e)
        {

            GameOver.Visibility = Visibility.Collapsed;//hide won message when new game started
            Outer.Children.Clear(); // delete inner grid that contains the images

            Grid gameGrid = new Grid();
            InitGrid(_gridSize, gameGrid);
            FillPositions(_gridSize, gameGrid);
            Outer.Children.Add(gameGrid);
            CurrentScore.Text = "Game Score: 0";

            int delayTime;
            switch (_gridSize)
            {
                case 4:
                    delayTime = 4000;
                    break;
                case 6:
                    delayTime = 8000;
                    break;
                case 8:
                    delayTime = 12000;
                    break;
                default: throw new ArgumentException("grid size should be 4, 6 or 8");
            }

            //causes the delay to allow the user to see the images before they are flipped
            //https://msdn.microsoft.com/en-us/library/hh194873(v=vs.110).aspx
            await Task.Delay(delayTime);

            //set every rectangle image as question mark
            var children = gameGrid.Children;
            foreach (var child in children)
            {
                var rect = child as Rectangle;
                SetToDefault(rect);
                rect.Tapped += Rectangle_Tapped;//=============================================================================================================
            }

            _clickNo = 0;
            _currentScore = 0;
            CurrentScore.Text = "Game Score: " + _currentScore;

        }

        private static void ToggleImage(Shape rect)
        {
            var brush = new ImageBrush(); // imagebrush is an object
            var uri = new Uri("ms-appx:///Assets/Images/" + rect.Tag + ".png", UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage(uri);

            brush.ImageSource = bitmap;

            rect.Fill = brush;
        }

        private static void SetToDefault(Shape rect)
        {
            var brush = new ImageBrush(); // imagebrush is an object
            var uri = new Uri("ms-appx:///Assets/Images/100.png", UriKind.RelativeOrAbsolute);
            var bitmap = new BitmapImage(uri);

            brush.ImageSource = bitmap;

            rect.Fill = brush;
        }

        private void DisplayScores()
        {
            HighScore.Text = "High Score: " + LocalStorage.LoadHighScore();
            CurrentScore.Text = "Game Score: " + _currentScore;
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
                    if (_currentScore > LocalStorage.LoadHighScore())
                    {
                        LocalStorage.SaveHighScore(_currentScore);
                    }

                    DisplayScores();
                   
                    // adding a game to the game history observable collection will make it appear in the game history page.                 
                    _gameHistory.Add(new Game(LocalStorage.Load().Count + 1, _currentScore, _gridSize, DateTime.Now)); 
                    // save all the games to local storage.
                    LocalStorage.Save(_gameHistory);

                    //display Game over message
                    GameOver.Visibility = Visibility.Visible;

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
                    // remove the event handler so it can't be clicked again
                    _firstRectangle.Tapped -= Rectangle_Tapped;
                    _secondRectangle.Tapped -= Rectangle_Tapped;
                    _totalTilesLeft -= 2; // we have 2 fewer tiles.
                    _currentScore += 10; // user gets 10 points when getting a match
                    if (_currentScore > LocalStorage.LoadHighScore())
                    {
                        LocalStorage.SaveHighScore(_currentScore);
                    }
                }
                else
                {
                    // reset the 2 images that didn't match
                    SetToDefault(_firstRectangle);
                    SetToDefault(_secondRectangle);
                    _currentScore -= 1; // user loses 5 if the make a mistake.
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
            DisplayScores();

        }
        /*
         * Event handles that get called when the user selects a new grid size.
         */
        private void FourByFour_OnClick(object sender, RoutedEventArgs e)
        {
            _gridSize = 4;
            MainPivot.SelectedIndex = 0;
            NewGame_Click(null, null); // calling the NewGame_Click event handler
        }
        private void SixBySix_OnClick(object sender, RoutedEventArgs e)
        {
            _gridSize = 6;
            MainPivot.SelectedIndex = 0;
            NewGame_Click(null, null);
        }
        private void EightByEight_OnClick(object sender, RoutedEventArgs e)
        {
            _gridSize = 8;
            MainPivot.SelectedIndex = 0;
            NewGame_Click(null, null);
        }

        private void ResetHistory_OnClick(object sender, RoutedEventArgs e)
        {
            ResetButtons.Visibility = Visibility.Visible;
            
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            ResetButtons.Visibility = Visibility.Collapsed;
            LocalStorage.ClearGames(); // delete from local storage
            _gameHistory.Clear();
            HighScore.Text = "High Score: 0";
            CurrentScore.Text = "Game Score: 0";

        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            ResetButtons.Visibility = Visibility.Collapsed;
        }
    }
}
