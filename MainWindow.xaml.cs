using System.Windows;
using System.Windows.Controls;

namespace Mooforest {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
#if DEBUG
            Menu.SelectedIndex = 0;
#endif
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            var tag = ((ListViewItem)Menu.SelectedItem).Tag;
            switch (tag) {
                case "IssueManagement":
                    FeatureView.Content = new Features.IssueManagement.IssueManagementView();
                    break;
                case "Schedule":
                    break;
                case "Memo":
                    break;
                default:
                    break;
            }
        }
    }
}
