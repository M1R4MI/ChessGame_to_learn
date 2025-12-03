using ChessLogic;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for DifficultyMenu.xaml
    /// </summary>
    public partial class DifficultyMenu : UserControl
    {
        public event Action<AILevel> DifficultySelected;
        public DifficultyMenu()
        {
            InitializeComponent();
        }

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            DifficultySelected?.Invoke(AILevel.Easy);
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            DifficultySelected?.Invoke(AILevel.Medium);
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            DifficultySelected?.Invoke(AILevel.Hard);
        }
    }
}
