using System.Windows;

namespace Mooforest.Features.IssueManagement {
    public partial class AddHistoryView : Window {
        public Issue OwnIssue;

        public AddHistoryView(Issue issue) {
            InitializeComponent();

            // Initialize Data
            OwnIssue = issue;

            // Initialize UIs
            Title += $"{OwnIssue.Title} ({OwnIssue.Id})";
            InputStatus.SelectedItem = IssueManagementModel.Statuses.FirstOrDefault(x => x.Id == OwnIssue.StatusId);
        }

        private void AddHistoryClick(object sender, RoutedEventArgs e) {
            if (InputStatus.SelectedIndex == -1) {
                MessageBox.Show("状態を指定してください");
                return;
            }
            var status = (Status)InputStatus.SelectedItem;
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("内容を書いてください");
                return;
            }
            IssueManagementModel.AddHistory(OwnIssue, status, InputDescription.Text, InputToDo.Text);
            Close();
        }
    }
}
