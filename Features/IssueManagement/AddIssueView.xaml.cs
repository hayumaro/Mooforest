using System.Windows;

namespace Mooforest.Features.IssueManagement {
    public partial class AddIssueView : Window {
        public AddIssueView() {
            InitializeComponent();
        }

        private void AddIssue_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(InputTitle.Text)) {
                MessageBox.Show("タイトルを入力してください");
                return;
            }
            if (string.IsNullOrWhiteSpace(InputDescription.Text)) {
                MessageBox.Show("内容を入力してください");
                return;
            }
            if (InputCategory.SelectedIndex == -1) {
                MessageBox.Show("カテゴリーを選んでください");
                return;
            }
            IssueManagementModel.InsertIssue(InputTitle.Text, InputDescription.Text, InputToDo.Text, (int)InputCategory.SelectedValue);
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ImInstance?.Reload();
            Close();
        }
    }
}
