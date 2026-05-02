using System.Windows;

namespace Mooforest.Features.IssueManagement {
    public partial class EditIssueView : Window {
        public Issue OwnIssue;

        public EditIssueView(Issue issue) {
            InitializeComponent();
            OwnIssue = issue;
            InitializeInputs();

            Title += $"{OwnIssue.Title} ({OwnIssue.Id})";
        }

        private void InitializeInputs() {
            InputTitle.Text = OwnIssue.Title;
            InputDescription.Text = OwnIssue.Description;
            InputToDo.Text = OwnIssue.ToDo;
            InputStatus.SelectedValue = OwnIssue.StatusId;
            InputCategory.SelectedValue = OwnIssue.CategoryId;
        }

        private void EditIssue_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(InputTitle.Text)) {
                MessageBox.Show("タイトルを空欄にはできません");
                return;
            }
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("説明を空欄にはできません");
                return;
            }
            if (InputStatus.SelectedValue is not int statusId) {
                statusId = OwnIssue.StatusId;
            }
            if (InputCategory.SelectedValue is not int categoryId) {
                categoryId = OwnIssue.CategoryId;
            }
            IssueManagementModel.UpdateIssue(OwnIssue.Id, InputTitle.Text, InputDescription.Text, statusId, InputToDo.Text, categoryId);
            Close();
        }
    }
}
