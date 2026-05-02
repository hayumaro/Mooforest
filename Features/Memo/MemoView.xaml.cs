using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mooforest.Features.Memo {
    public partial class MemoView : UserControl {
        public static ObservableCollection<Memo> Memos { get; private set; } = [];

        public MemoView() {
            InitializeComponent();
            MemoModel.Initialize();
            MemoModel.GetMemos(Memos);
            DataContext = this;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
                if (MemoList.SelectedValue is int memoId) {
                    MemoModel.Save(memoId, MemoTitle.Text, MemoContent.Text);
                } else {
                    memoId = MemoModel.SaveNew(MemoTitle.Text, MemoContent.Text);
                }
                MemoModel.GetMemos(Memos);
                MemoList.SelectedValue = memoId;
                e.Handled = true;
            }
        }

        private void MemoListSelectionChanged(object sender, EventArgs e) {
            if (MemoList.SelectedItem is not Memo memo) return;
            MemoTitle.Text = memo.Title;
            MemoContent.Text = MemoModel.GetContent(memo.Id);
        }
    }
}
