using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Mooforest.Features.IssueManagement {
    public partial class IssueManagementView : UserControl {
        // いま画面に表示しているIssueのリスト。
        public static ObservableCollection<Issue> Issues { get; private set; } = [];

        public IssueManagementView() {
            InitializeComponent();
            IssueManagementModel.Initialize();
            IssueManagementModel.LoadOpenIssues(Issues);
            DataContext = this;
        }

        public void Reload() {
            if (DisplayClosed.IsChecked == true) {
                IssueManagementModel.LoadClosedIssues(Issues);
            } else {
                IssueManagementModel.LoadOpenIssues(Issues);
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (sender is not DataGrid dg) return;
            dg.SelectedItem = null;
        }

        private void DisplayClosed_Checked(object sender, EventArgs e) {
            IssueManagementModel.LoadClosedIssues(Issues);
        }

        private void DisplayClosed_Unchecked(object sender, EventArgs e) {
            IssueManagementModel.LoadOpenIssues(Issues);
        }

        private void Title_Click(object sender, RoutedEventArgs e) {
            if (sender is not Button button) return;
            var issue = Issues.FirstOrDefault(x => x.Id == (int)button.Tag)!;
            var histories = IssueManagementModel.LoadHistories(issue.Id);
            var detailWindow = new IssueDetailView(issue, histories) {
                Owner = Application.Current.MainWindow as MainWindow
            };
            detailWindow.Show();
        }

        private void ParentId_Click(object sender, RoutedEventArgs e) {
            if (sender is not Button button) return;
            MessageBox.Show(button.Content is null ? "親課題はありません" : button.Content.ToString());
        }

        private void AddIssue_Click(object sender, RoutedEventArgs e) {
            var addIssueView = new AddIssueView() {
                Owner = Application.Current.MainWindow as MainWindow
            };
            addIssueView.Show();
        }
    }
}
