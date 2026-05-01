using System.Windows;
using System.Windows.Controls;

namespace Mooforest.Features.IssueManagement {
    public partial class EditIssueView : Window {
        public Issue OwnIssue;

        public EditIssueView(Issue issue) {
            InitializeComponent();
            OwnIssue = issue;
            InitializeInputs();
        }

        private void InitializeInputs() {
            InputTitle.Text = OwnIssue.Title;
            InputDescription.Text = OwnIssue.Description;
            InputToDo.Text = OwnIssue.ToDo;
            var status = IssueManagementModel.Statuses.FirstOrDefault(x => x.Id == OwnIssue.Id);
            InputStatus.SelectedItem = status;
        }

        private int SelectedStatusId() {
            return (int)((ComboBoxItem)InputStatus.SelectedItem).Tag;
        }

        private void EditIssue_Click(object sender, RoutedEventArgs e) {
            var titleChanged = InputTitle.Text != OwnIssue.Title;
            var descChanged = InputDescription.Text != OwnIssue.Description;
            var toDoChanged = InputToDo.Text != OwnIssue.ToDo;
            var statusChanged = SelectedStatusId() != OwnIssue.StatusId;

            if (!titleChanged && !descChanged && !toDoChanged && !statusChanged) {
                MessageBox.Show("何も変更されていません");
                return;
            }
            if (string.IsNullOrWhiteSpace(InputTitle.Text)) {
                MessageBox.Show("タイトルを空欄にはできません");
                return;
            }
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("説明を空欄にはできません");
                return;
            }
            var status = IssueManagementModel.Statuses.FirstOrDefault(x => x.Id == SelectedStatusId())!;
            if (!status.IsClosed && string.IsNullOrWhiteSpace(InputToDo.Text)) {
                MessageBox.Show("完了していない課題はToDoを入力してください");
                return;
            }
            IssueManagementModel.UpdateIssue(OwnIssue.Id, InputTitle.Text, InputDescription.Text, status.Id, InputToDo.Text);
            Close();
        }
    }
}
