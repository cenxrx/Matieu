using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Matieu
{
    public partial class MainWindow : Window
    {
        private User _currentUser;
        private List<Service> _allServices = new();
        private List<Service> _filtered = new();
        private List<Collection> _collections = new();
        private int _page = 0;
        private const int PageSize = 3;
        private string _category = "Custom";

        public MainWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;

            this.FindControl<Button>("closeBtn")!.Click += OnClose;
            this.FindControl<Button>("minimizeBtn")!.Click += (s, e) => WindowState = WindowState.Minimized;
            this.FindControl<Button>("logoutBtn")!.Click += (s, e) => { new LoginWindow().Show(); Close(); };
            this.FindControl<Button>("prevBtn")!.Click += (s, e) => { if (_page > 0) { _page--; RenderPage(); } };
            this.FindControl<Button>("nextBtn")!.Click += (s, e) => { if ((_page + 1) * PageSize < _filtered.Count) { _page++; RenderPage(); } };
            this.FindControl<Button>("servicesBtn")!.Click += (s, e) => LoadServices();

            var tabs = this.FindControl<TabControl>("categoryTabs")!;
            tabs.SelectionChanged += (s, e) =>
            {
                _category = tabs.SelectedIndex == 0 ? "Custom" : "Cosplay";
                _page = 0;
                ApplyFilter();
            };

            var searchBox = this.FindControl<TextBox>("searchBox")!;
            searchBox.TextChanged += (s, e) => { _page = 0; ApplyFilter(); };

            var collectionFilter = this.FindControl<ComboBox>("collectionFilter")!;
            collectionFilter.SelectionChanged += (s, e) => { _page = 0; ApplyFilter(); };

            if (_currentUser.RoleName == "moderator" || _currentUser.RoleName == "admin")
            {
                var addBtn = this.FindControl<Button>("addServiceBtn")!;
                addBtn.IsVisible = true;
                addBtn.Click += (s, e) => OpenAddService();
            }

            LoadCollections();
            LoadServices();
        }

        private async void LoadCollections()
        {
            _collections.Clear();
            _collections.Add(new Collection { Id = 0, Name = "Все коллекции" });

            using var conn = Database.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name FROM collections ORDER BY name", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                _collections.Add(new Collection { Id = reader.GetInt32(0), Name = reader.GetString(1) });

            var combo = this.FindControl<ComboBox>("collectionFilter")!;
            combo.ItemsSource = _collections.Select(c => c.Name).ToList();
            combo.SelectedIndex = 0;
        }

        private async void LoadServices()
        {
            _allServices.Clear();
            using var conn = Database.GetConnection();
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(
                @"SELECT s.id, s.name, s.description, s.price, s.category,
                         s.collection_id, COALESCE(c.name,''), s.image_path, s.updated_at
                  FROM services s LEFT JOIN collections c ON s.collection_id = c.id
                  ORDER BY s.name", conn);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                _allServices.Add(new Service
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    Category = reader.GetString(4),
                    CollectionId = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    CollectionName = reader.GetString(6),
                    ImagePath = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    UpdatedAt = reader.GetDateTime(8)
                });
            }

            _page = 0;
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var search = this.FindControl<TextBox>("searchBox")!.Text?.ToLower() ?? "";
            var combo = this.FindControl<ComboBox>("collectionFilter")!;
            var selectedCollection = combo.SelectedIndex > 0 ? _collections[combo.SelectedIndex].Name : "";

            _filtered = _allServices
                .Where(s => s.Category == _category)
                .Where(s => string.IsNullOrEmpty(search) || s.Name.ToLower().Contains(search))
                .Where(s => string.IsNullOrEmpty(selectedCollection) || s.CollectionName == selectedCollection)
                .OrderBy(s => s.Name)
                .ToList();

            RenderPage();
        }

        private void RenderPage()
        {
            var panel = this.FindControl<StackPanel>("servicesPanel")!;
            panel.Children.Clear();

            var pageItems = _filtered.Skip(_page * PageSize).Take(PageSize).ToList();
            int total = _filtered.Count;

            foreach (var svc in pageItems)
                panel.Children.Add(BuildServiceCard(svc));

            int from = total == 0 ? 0 : _page * PageSize + 1;
            int to = Math.Min((_page + 1) * PageSize, total);
            this.FindControl<TextBlock>("pageInfo")!.Text = $"{from}-{to} из {total}";
        }

        private Border BuildServiceCard(Service svc)
        {
            var card = new Border
            {
                BorderBrush = Avalonia.Media.Brushes.Black,
                BorderThickness = new Avalonia.Thickness(1),
                Padding = new Avalonia.Thickness(12),
                Background = Avalonia.Media.Brushes.White,
                Margin = new Avalonia.Thickness(0, 0, 0, 10)
            };

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(130, GridUnitType.Pixel));
            grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));

            var imgBorder = new Border
            {
                Width = 120,
                Height = 120,
                BorderBrush = Avalonia.Media.Brushes.Black,
                BorderThickness = new Avalonia.Thickness(1)
            };

            var img = new Image { Stretch = Avalonia.Media.Stretch.UniformToFill };

            var imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Resources", svc.ImagePath.Replace("/", "\\"));

            if (File.Exists(imgPath))
                img.Source = new Bitmap(imgPath);
            else
            {
                var placeholder = new TextBlock
                {
                    Text = "Фото",
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };
                imgBorder.Child = placeholder;
            }

            if (img.Source != null)
                imgBorder.Child = img;

            Grid.SetColumn(imgBorder, 0);

            var info = new StackPanel
            {
                Margin = new Avalonia.Thickness(15, 0, 0, 0),
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top
            };
            info.Children.Add(new TextBlock { Text = svc.Name, FontSize = 14, Margin = new Avalonia.Thickness(0, 0, 0, 5) });
            
            if (!string.IsNullOrEmpty(svc.CollectionName))
                info.Children.Add(new TextBlock { Text = svc.CollectionName, Foreground = Avalonia.Media.Brushes.Gray, FontSize = 11, Margin = new Avalonia.Thickness(0, 0, 0, 5) });
            
            info.Children.Add(new TextBlock { Text = $"{svc.Price:N0} руб.", FontSize = 13, Margin = new Avalonia.Thickness(0, 0, 0, 5) });
            
            if (!string.IsNullOrEmpty(svc.Description))
                info.Children.Add(new TextBlock 
                { 
                    Text = svc.Description, 
                    FontSize = 11, 
                    Foreground = Avalonia.Media.Brushes.Gray,
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    Margin = new Avalonia.Thickness(0, 5, 0, 5)
                });

            if (_currentUser.RoleName == "moderator" || _currentUser.RoleName == "admin")
            {
                var editBtn = new Button
                {
                    Content = "Редактировать",
                    Margin = new Avalonia.Thickness(0, 10, 0, 0),
                    Padding = new Avalonia.Thickness(12, 6),
                    Height = 32,
                    Background = Avalonia.Media.Brushes.White,
                    Foreground = Avalonia.Media.Brushes.Black,
                    BorderBrush = Avalonia.Media.Brushes.Black,
                    BorderThickness = new Avalonia.Thickness(1)
                };
                var capturedSvc = svc;
                editBtn.Click += (s, e) => OpenEditService(capturedSvc);
                info.Children.Add(editBtn);
            }

            Grid.SetColumn(info, 1);
            grid.Children.Add(imgBorder);
            grid.Children.Add(info);
            card.Child = grid;
            return card;
        }

        private void OpenEditService(Service svc)
        {
            var win = new EditServiceWindow(svc);
            win.Closed += (s, e) => LoadServices();
            win.ShowDialog(this);
        }

        private void OpenAddService()
        {
            var win = new EditServiceWindow(null);
            win.Closed += (s, e) => LoadServices();
            win.ShowDialog(this);
        }

        private async void OnClose(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new ConfirmDialog("Закрыть приложение?");
            var result = await dialog.ShowDialog<bool>(this);
            if (result) Close();
        }
    }
}
