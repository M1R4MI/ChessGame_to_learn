using ChessLogic.Enums;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl
    {
        public event Action<GameMode> GameModeSelected;
        public StartMenu()
        {
            InitializeComponent();
        }

        private void Human_Click(object sender, RoutedEventArgs e)
        {
            GameModeSelected?.Invoke(GameMode.HumanVsHuman);
        }

        private void Bot_Click(object sender, RoutedEventArgs e)
        {
            GameModeSelected?.Invoke(GameMode.HumanVsBot);
        }
    }
}
