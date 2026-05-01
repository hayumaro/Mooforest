using System.Windows.Controls;
using System.Windows.Input;

namespace Mooforest.Features.Memo {
    public partial class MemoView : UserControl {
        public MemoModel Model;

        public MemoView() {
            InitializeComponent();
            MemoModel.Initialize();
            Model = new MemoModel();
            DataContext = Model;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                MemoModel.Save(MemoTitle.Text, MemoContent.Text);
                e.Handled = true;
            }
        }

        private void MenuList_SelectionChanged(object sender, EventArgs e) {
            if (MemoList.SelectedItem is not Memo memo) return;
            MemoModel.CurrentId = memo.Id;
            MemoTitle.Text = memo.Title;
            MemoContent.Text = MemoModel.LoadContent(memo.Id);
        }
    }
}
