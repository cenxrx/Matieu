using Avalonia.Controls;

namespace Matieu
{
    public partial class ConfirmDialog : Window
    {
        public ConfirmDialog(string message)
        {
            InitializeComponent();
            this.FindControl<TextBlock>("messageText")!.Text = message;
            this.FindControl<Button>("yesBtn")!.Click += (s, e) => Close(true);
            this.FindControl<Button>("noBtn")!.Click += (s, e) => Close(false);
        }
    }
}
