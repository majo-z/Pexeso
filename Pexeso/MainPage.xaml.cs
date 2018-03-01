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
        public MainPage()
        {
            this.InitializeComponent();
            InitGrid(8);
            GeneratePossiblePositions(8);
        }

        //crate and draw rectangles
        private void InitGrid(int size)
        {
            for (int i = 0; i < size; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    rect.SetValue(Grid.RowProperty, row);
                    rect.SetValue(Grid.ColumnProperty, col);
                    rect.Width = 50;
                    rect.Height = 50;
                    GameGrid.Children.Add(rect);
                }

            }
        }

        // create set of all possible positions
        private List<string> GeneratePossiblePositions(int size)
        {
            //HashSet<string> set = new HashSet<string>();
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

        // loop through each one
        // take two off the top
        // create 2 rectanges
        // assign same id to both
        // place them on the grid

    }
}
