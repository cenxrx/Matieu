using Avalonia.Controls;
using Avalonia.Input;
using Npgsql;

namespace Matieu
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var titleBar = this.FindControl<Grid>("titleBar")!;
            this.FindControl<Button>("closeBtn")!.Click += (s, e) => Close();
            this.FindControl<Button>("minimizeBtn")!.Click += (s, e) => WindowState = WindowState.Minimized;
            this.FindControl<Button>("loginBtn")!.Click += OnLogin;

            titleBar.AddHandler(PointerPressedEvent, (s, e) =>
            {
                if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && e.Source is Grid)
                    BeginMoveDrag(e);
            }, handledEventsToo: false);
        }

        private async void OnLogin(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var login = this.FindControl<TextBox>("loginBox")!.Text ?? "";
            var password = this.FindControl<TextBox>("passwordBox")!.Text ?? "";
            var errorText = this.FindControl<TextBlock>("errorText")!;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                errorText.Text = "Введите логин и пароль";
                errorText.IsVisible = true;
                return;
            }

            using var conn = Database.GetConnection();
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(
                "SELECT u.id, u.login, u.full_name, u.balance, u.role_id, r.name FROM users u JOIN roles r ON u.role_id = r.id WHERE u.login = @l AND u.password = @p",
                conn);
            cmd.Parameters.AddWithValue("l", login);
            cmd.Parameters.AddWithValue("p", password);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var user = new User
                {
                    Id = reader.GetInt32(0),
                    Login = reader.GetString(1),
                    FullName = reader.GetString(2),
                    Balance = reader.GetDecimal(3),
                    RoleId = reader.GetInt32(4),
                    RoleName = reader.GetString(5)
                };

                var main = new MainWindow(user);
                main.Show();
                Close();
            }
            else
            {
                errorText.Text = "Неверный логин или пароль";
                errorText.IsVisible = true;
            }
        }
    }
}
