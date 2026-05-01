using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Mooforest.Features.IssueManagement {
    public partial class AddHistoryView : Window {
        public Issue TargetIssue;

        public AddHistoryView(Issue issue) {
            InitializeComponent();
            TargetIssue = issue;
            foreach (var status in IssueManagementModel.Statuses) {
                InputStatus.Items.Add(new ComboBoxItem { Content = status.Name, Tag = status.Id });
                // 初期値指定
                if (issue.StatusId == status.Id) {
                    InputStatus.SelectedIndex = InputStatus.Items.Count - 1;
                }
            }
        }

        private void AddHistory_CLick(object sender, RoutedEventArgs e) {
            if (InputStatus.SelectedIndex == -1) {
                MessageBox.Show("ステータスを指定してください");
                return;
            }
            var selectedItem = (ComboBoxItem)InputStatus.SelectedItem;
            var status = IssueManagementModel.Statuses.FirstOrDefault(x => x.Id == (int)selectedItem.Tag)!;
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("内容を書いてください");
                return;
            }
            // 完了済みの課題は次にやることが空欄でもよい
            if (!status.IsClosed && string.IsNullOrWhiteSpace(InputToDo.Text)) {
                MessageBox.Show("次にやることを書いてください");
                return;
            }
            IssueManagementModel.InsertHistory(TargetIssue.Id, status.Id, InputDescription.Text);
            IssueManagementModel.UpdateToDo(TargetIssue.Id, InputToDo.Text);
            Close();
        }
    }
}
