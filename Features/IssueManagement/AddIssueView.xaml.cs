using System.Windows;
using System.Windows.Markup;

namespace Mooforest.Features.IssueManagement {
    public partial class AddIssueView : Window {
        public AddIssueView() {
            InitializeComponent();
        }

        private void AddIssueButton_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(InputTitle.Text)) {
                MessageBox.Show("タイトルを入力してください");
                return;
            }
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("内容を入力してください");
                return;
            }
            if (string.IsNullOrWhiteSpace(InputToDo.Text)) {
                MessageBox.Show("次にやることを入力してください");
                return;
            }
            IssueManagementModel.InsertIssue(InputTitle.Text, InputDescription.Text, InputToDo.Text);
            MessageBox.Show("課題を登録しました");
            Close();
        }
    }
}
