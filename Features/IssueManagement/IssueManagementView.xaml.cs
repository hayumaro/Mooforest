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
            IssueManagementModel.GetIssues(Issues, false, null);
            DataContext = this;
            InputCategory.SelectedIndex = -1;
            InputStatus.SelectedIndex = 0;
        }

        public void Reload() {
            GetFilteredIssues();
        }

        private bool StatusFilterIsClosed() {
            if (InputStatus.SelectedItem is not ComboBoxItem item) return true;
            return item.Content.ToString() == "クローズ";
        }

        private void GetFilteredIssues() {
            IssueManagementModel.GetIssues(Issues, StatusFilterIsClosed(), (int?)InputCategory.SelectedValue);
        }

        private void ClearCategory_Click(object sender, RoutedEventArgs e) {
            InputCategory.SelectedIndex = -1;
        }

        private void ClearStatus_Click(object sender, RoutedEventArgs e) {
            InputStatus.SelectedIndex = -1;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (sender is not DataGrid dg) return;
            dg.SelectedItem = null;
        }

        private void FilterChanged(object sender, EventArgs e) {
            GetFilteredIssues();
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
