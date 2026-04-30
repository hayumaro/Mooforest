using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mooforest.Features.IssueManagement {
    public partial class IssueManagementView : UserControl {
        public IssueManagementModel Model;

        public IssueManagementView() {
            InitializeComponent();
            Model = new IssueManagementModel();
            Model.LoadIssues();
            DataContext = Model;
        }

        private void IssueManagementView_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.F5) {
                Model.LoadIssues();
            }
            e.Handled = true;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (sender is not DataGrid dg) return;
            dg.SelectedItem = null;
        }

        private void Title_Click(object sender, RoutedEventArgs e) {
            if (sender is not Button button) return;
            var issue = Model.Issues.FirstOrDefault(x => x.Id == (int)button.Tag)!;
            var histories = Model.LoadHistories(issue.Id);
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
