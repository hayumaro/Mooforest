using System.Windows;
using System.Windows.Controls;

namespace Mooforest {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            var tag = ((ListViewItem)Menu.SelectedItem).Tag;
            switch (tag) {
                case "ToDo":
                    break;
                default:
                    break;
            }
        }
    }
}
