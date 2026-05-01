using System.Windows;

namespace Mooforest.Features.IssueManagement {
    public partial class AddHistoryView : Window {
        public Issue OwnIssue;

        public AddHistoryView(Issue issue) {
            InitializeComponent();
            OwnIssue = issue;
            var status = IssueManagementModel.Statuses.FirstOrDefault(x => x.Id == OwnIssue.Id);
            InputStatus.SelectedItem = status;
        }

        private void AddHistory_CLick(object sender, RoutedEventArgs e) {
            if (InputStatus.SelectedIndex == -1) {
                MessageBox.Show("ステータスを指定してください");
                return;
            }
            var status = (Status)InputStatus.SelectedItem;
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("内容を書いてください");
                return;
            }
            // 完了済みの課題は次にやることが空欄でもよい
            if (!status.IsClosed && string.IsNullOrWhiteSpace(InputToDo.Text)) {
                MessageBox.Show("次にやることを書いてください");
                return;
            }
            IssueManagementModel.InsertHistory(OwnIssue.Id, status.Id, InputDescription.Text);
            IssueManagementModel.UpdateIssue(OwnIssue.Id, OwnIssue.Title, OwnIssue.Description, status.Id, InputToDo.Text);
            Close();
        }
    }
}
