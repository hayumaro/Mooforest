using System.Windows;
using System.Windows.Controls;

namespace Mooforest.Features.IssueManagement {
    public partial class IssueManagementView : UserControl {
        public IssueManagementView() {
            InitializeComponent();
            IssueManagementModel.Initialize();
            IssueManagementModel.LoadIssuesWithoutCloses();
            DataContext = new IssueManagementModel();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (sender is not DataGrid dg) return;
            dg.SelectedItem = null;
        }

        private void DisplayClosed_Checked(object sender, EventArgs e) {
            if (sender is not CheckBox cb) return;
            if (cb.IsChecked == true) {
                IssueManagementModel.LoadIssues();
            } else {
                IssueManagementModel.LoadIssuesWithoutCloses();
            }
        }

        private void Title_Click(object sender, RoutedEventArgs e) {
            if (sender is not Button button) return;
            var issue = IssueManagementModel.Issues.FirstOrDefault(x => x.Id == (int)button.Tag)!;
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
