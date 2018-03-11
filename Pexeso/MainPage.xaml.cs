using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private Random _rnd = new Random();
        private Rectangle _firstRectangle;
        private Rectangle _secondRectangle;
        private int _clickNo;
        private int _gridSize;
        public MainPage()
        {
            this.InitializeComponent();
            _gridSize = 4;
            InitGrid(_gridSize);
            FillPositions(_gridSize);
            // GeneratePossiblePositions(8);
        }

        //crate and draw rectangles
        private void InitGrid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                gameGrid.RowDefinitions.Add(new RowDefinition());
                gameGrid.ColumnDefinitions.Add(new ColumnDefinition());
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
            Uri uri = new Uri("ms-appx:///Assets/Images/" + imageNum.ToString() + ".png", UriKind.RelativeOrAbsolute);
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
            rect.Width = 40;
            rect.Height = 40;
            gameGrid.Children.Add(rect);
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

        }
       
        
        
        
        

        //game reset button
        private void gameReset_Click(object sender, RoutedEventArgs e)
        {
            gameGrid.RowDefinitions.Clear(); // delete all the rows
            gameGrid.ColumnDefinitions.Clear(); // delete all the columns
            InitGrid(_gridSize);
            FillPositions(_gridSize);

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

            }
            else if (_clickNo == 2)
            {// this is the 3rd click, check the previous 2 rectangles to see if they match
                if (rect == _firstRectangle || rect ==_secondRectangle)
                { // ignore clicks on the same two rectangles.
                    return;
                }
                bool rectanglesMatch = _firstRectangle.Tag.ToString() == _secondRectangle.Tag.ToString();
                if (rectanglesMatch)
                {
                    // remove the evant handler so it cant be clicked again
                    _firstRectangle.Tapped -= Rectangle_Tapped;
                    _secondRectangle.Tapped -= Rectangle_Tapped;
                }
                else
                {
                    // reset the 2 images that didn't match
                    SetToDefault(_firstRectangle);
                    SetToDefault(_secondRectangle);

                }
                ToggleImage(rect); // display the new image
                _firstRectangle = rect; // save it
                _clickNo = 1;
            }
           
            // if they're the same. They stay up
            // if they're not the same. Thty stay up
            // on a third click, if they were the same, the stay up -> remove event handler
            // if they weren't the same, they go down. -> keep event handler


            //var rect = (Rectangle) sender;
            /*
            var rect = sender as Rectangle;
            _current = rect;

            // this is the first click.
            if (_previous == null)
            {
                ToggleImage(_current);
                _previous = _current;
                return;
            }
            else // the user has clicked on a previous tile.
            { // attempt to match

                bool tilesAreTheSame = _current.Tag == _previous.Tag;
                if (!tilesAreTheSame)
                {
                    SetToDefault(_current);
                    SetToDefault(_previous);
                    _previous = null;
                    _current = null;
                }
            }

            // compare both tiles.
            if (_current != null)
            {
                ToggleImage(_current);
            }
 
            
            

            _previous = _current;
            */
        }
    }
}
