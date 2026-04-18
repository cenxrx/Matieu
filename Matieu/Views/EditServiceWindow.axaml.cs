using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Matieu
{
    public partial class EditServiceWindow : Window
    {
        private Service? _service;
        private List<Collection> _collections = new();
        private string _selectedImagePath = "";
        private static readonly string resourcesBase = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Resources", "pr");

        public EditServiceWindow(Service? service)
        {
            InitializeComponent();
            _service = service;

            this.FindControl<Button>("cancelBtn")!.Click += (s, e) => Close();
            this.FindControl<Button>("saveBtn")!.Click += OnSave;
            this.FindControl<Button>("pickImageBtn")!.Click += OnPickImage;

            var categoryBox = this.FindControl<ComboBox>("categoryBox")!;
            categoryBox.ItemsSource = new List<string> { "Custom", "Cosplay" };

            if (service == null)
            {
                this.FindControl<TextBlock>("windowTitle")!.Text = "Добавление услуги";
                categoryBox.SelectedIndex = 0;
            }
            else
            {
                this.FindControl<TextBlock>("windowTitle")!.Text = "Редактирование услуги";
                this.FindControl<TextBox>("nameBox")!.Text = service.Name;
                this.FindControl<TextBox>("descBox")!.Text = service.Description;
                this.FindControl<TextBox>("priceBox")!.Text = service.Price.ToString("F2");
                categoryBox.SelectedItem = service.Category;

                if (!string.IsNullOrEmpty(service.ImagePath))
                {
                    var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Resources", service.ImagePath.Replace("/", "\\"));
                    if (File.Exists(fullPath))
                    {
                        this.FindControl<Image>("previewImage")!.Source = new Bitmap(fullPath);
                        this.FindControl<TextBlock>("imageNameText")!.Text = Path.GetFileName(fullPath);
                    }
                }
            }

            LoadCollections();
        }

        private async void OnPickImage(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Выберите фото",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Изображения") { Patterns = new[] { "*.jpg", "*.jpeg", "*.png" } }
                }
            });

            if (files.Count == 0) return;

            var file = files[0];
            var localPath = file.TryGetLocalPath();
            if (string.IsNullOrEmpty(localPath)) return;

            _selectedImagePath = localPath;
            this.FindControl<Image>("previewImage")!.Source = new Bitmap(localPath);
            this.FindControl<TextBlock>("imageNameText")!.Text = Path.GetFileName(localPath);
        }

        private async void LoadCollections()
        {
            _collections.Clear();
            _collections.Add(new Collection { Id = 0, Name = "Без коллекции" });

            using var conn = Database.GetConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("SELECT id, name FROM collections ORDER BY name", conn);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                _collections.Add(new Collection { Id = reader.GetInt32(0), Name = reader.GetString(1) });

            var combo = this.FindControl<ComboBox>("collectionBox")!;
            combo.ItemsSource = _collections.Select(c => c.Name).ToList();

            if (_service != null && _service.CollectionId.HasValue)
            {
                var idx = _collections.FindIndex(c => c.Id == _service.CollectionId.Value);
                combo.SelectedIndex = idx >= 0 ? idx : 0;
            }
            else
            {
                combo.SelectedIndex = 0;
            }
        }

        private string CopyImageToResources(string sourcePath, string category)
        {
            var destDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "pr", category);
            Directory.CreateDirectory(destDir);

            var fileName = Path.GetFileName(sourcePath);
            var destPath = Path.Combine(destDir, fileName);

            if (File.Exists(destPath))
            {
                var name = Path.GetFileNameWithoutExtension(fileName);
                var ext = Path.GetExtension(fileName);
                fileName = $"{name}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                destPath = Path.Combine(destDir, fileName);
            }

            File.Copy(sourcePath, destPath);
            return $"pr/{category}/{fileName}";
        }

        private async void OnSave(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var name = this.FindControl<TextBox>("nameBox")!.Text ?? "";
            var desc = this.FindControl<TextBox>("descBox")!.Text ?? "";
            var priceStr = this.FindControl<TextBox>("priceBox")!.Text ?? "";
            var category = this.FindControl<ComboBox>("categoryBox")!.SelectedItem?.ToString() ?? "";
            var collectionIdx = this.FindControl<ComboBox>("collectionBox")!.SelectedIndex;
            var errorText = this.FindControl<TextBlock>("errorText")!;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(priceStr))
            {
                errorText.Text = "Заполните название и цену";
                errorText.IsVisible = true;
                return;
            }

            if (!decimal.TryParse(priceStr.Replace(",", "."), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal price))
            {
                errorText.Text = "Неверный формат цены";
                errorText.IsVisible = true;
                return;
            }

            int? collectionId = collectionIdx > 0 ? _collections[collectionIdx].Id : null;

            string? imagePath = null;
            if (!string.IsNullOrEmpty(_selectedImagePath))
                imagePath = CopyImageToResources(_selectedImagePath, category);
            else if (_service != null)
                imagePath = _service.ImagePath;

            using var conn = Database.GetConnection();
            await conn.OpenAsync();

            if (_service == null)
            {
                using var cmd = new NpgsqlCommand(
                    "INSERT INTO services (name, description, price, category, collection_id, image_path, updated_at) VALUES (@n, @d, @p, @c, @col, @img, NOW())",
                    conn);
                cmd.Parameters.AddWithValue("n", name);
                cmd.Parameters.AddWithValue("d", desc);
                cmd.Parameters.AddWithValue("p", price);
                cmd.Parameters.AddWithValue("c", category);
                cmd.Parameters.AddWithValue("col", (object?)collectionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("img", (object?)imagePath ?? DBNull.Value);
                await cmd.ExecuteNonQueryAsync();
            }
            else
            {
                using var cmd = new NpgsqlCommand(
                    "UPDATE services SET name=@n, description=@d, price=@p, category=@c, collection_id=@col, image_path=@img, updated_at=NOW() WHERE id=@id",
                    conn);
                cmd.Parameters.AddWithValue("n", name);
                cmd.Parameters.AddWithValue("d", desc);
                cmd.Parameters.AddWithValue("p", price);
                cmd.Parameters.AddWithValue("c", category);
                cmd.Parameters.AddWithValue("col", (object?)collectionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("img", (object?)imagePath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("id", _service.Id);
                await cmd.ExecuteNonQueryAsync();
            }

            Close();
        }
    }
}
