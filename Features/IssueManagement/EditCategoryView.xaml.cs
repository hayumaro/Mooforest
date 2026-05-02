using System.Windows;

namespace Mooforest.Features.IssueManagement {
    public partial class EditCategoryView : Window {
        public EditCategoryView() {
            InitializeComponent();
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(InputNewName.Text)) {
                MessageBox.Show("カテゴリー名を入力してください");
                return;
            }
            if (InputTarget.SelectedValue is int selectedId) {
                IssueManagementModel.UpdateCategory(selectedId, InputNewName.Text);
            } else {
                IssueManagementModel.AddCategory(InputNewName.Text);
            }
            Close();
        }

        private void ClearCategory_Click(object sender, RoutedEventArgs e) {
            InputTarget.SelectedIndex = -1;
        }
    }
}
