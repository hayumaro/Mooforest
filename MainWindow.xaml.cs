using System.Windows;
using System.Windows.Controls;

namespace Mooforest {
    public record FeatureInfo(string Name, Uri ImagePath);

    public partial class MainWindow : Window {
        public List<FeatureInfo> FeatureInfos { get; set; }

        public Features.IssueManagement.IssueManagementView? ImInstance;
        public Features.Memo.MemoView? MemoInstance;

        public MainWindow() {
            InitializeComponent();
            FeatureInfos = [
                new FeatureInfo("IssueManagement", new Uri("pack://application:,,,/Images/IssueManagement.png", UriKind.Absolute)),
                new FeatureInfo("Routine", new Uri("pack://application:,,,/Images/Routine.png", UriKind.Absolute)),
                new FeatureInfo("Diary", new Uri("pack://application:,,,/Images/Diary.png", UriKind.Absolute)),
            ];
            Menu.SelectedIndex = 1;
            DataContext = this;
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs args) {
            if (Menu.SelectedValue is not string selected) return;
            switch (selected) {
                case "IssueManagement":
                    ImInstance ??= new Features.IssueManagement.IssueManagementView();
                    FeatureView.Content = ImInstance;
                    break;
                case "Routine":
                    break;
                case "Diary":
                    break;
                default:
                    break;
            }
        }
    }
}
