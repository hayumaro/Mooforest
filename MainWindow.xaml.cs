using System.Windows;
using System.Windows.Controls;

namespace Mooforest {
    public partial class MainWindow : Window {
        public Features.IssueManagement.IssueManagementView? ImInstance;
        public Features.Memo.MemoView? MemoInstance;

        public MainWindow() {
            InitializeComponent();
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            var tag = ((ListViewItem)Menu.SelectedItem).Tag;
            switch (tag) {
                case "IssueManagement":
                    ImInstance ??= new Features.IssueManagement.IssueManagementView();
                    FeatureView.Content = ImInstance;
                    break;
                case "Schedule":
                    break;
                case "Memo":
                    MemoInstance ??= new Features.Memo.MemoView();
                    FeatureView.Content = MemoInstance;
                    break;
                default:
                    break;
            }
        }
    }
}
