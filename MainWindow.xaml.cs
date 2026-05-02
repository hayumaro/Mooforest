using System.Windows;
using System.Windows.Controls;

namespace Mooforest {
    public record FeatureInfo(string Name, Uri ImagePath);

    public partial class MainWindow : Window {
        public List<FeatureInfo> FeatureInfos { get; set; }

        public Features.IssueManagement.IssueManagementView? ImInstance;
        public Features.Memo.MemoView? MemoInstance;
        public Features.Schedule.ScheduleView? ScheduleInstance;

        public MainWindow() {
            InitializeComponent();
            FeatureInfos = [
                new FeatureInfo("IssueManagement", new Uri("pack://application:,,,/Images/IssueManagement.png", UriKind.Absolute)),
                new FeatureInfo("Schedule", new Uri("pack://application:,,,/Images/Schedule.png", UriKind.Absolute)),
                new FeatureInfo("Routine", new Uri("pack://application:,,,/Images/Routine.png", UriKind.Absolute)),
                new FeatureInfo("Memo", new Uri("pack://application:,,,/Images/Memo.png", UriKind.Absolute)),
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
                case "Schedule":
                    ScheduleInstance ??= new Features.Schedule.ScheduleView();
                    FeatureView.Content = ScheduleInstance;
                    break;
                case "Routine":
                    break;
                case "Memo":
                    MemoInstance ??= new Features.Memo.MemoView();
                    FeatureView.Content = MemoInstance;
                    break;
                case "Diary":
                    break;
                default:
                    break;
            }
        }
    }
}
