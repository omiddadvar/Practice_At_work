using Tesseract;

namespace ALPR;

public partial class MainPage : ContentPage
{
    private byte[] _currentImageData;

    public MainPage()
    {
        InitializeComponent();
    }
    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                await LoadImage(result.FullPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    await LoadImage(photo.FullPath);
                }
            }
            else
            {
                await DisplayAlert("Error", "Camera not supported", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task LoadImage(string imagePath)
    {
        try
        {
            // Read image as byte array
            _currentImageData = File.ReadAllBytes(imagePath);

            // Display image
            SelectedImage.Source = ImageSource.FromFile(imagePath);
            StatusLabel.Text = "Image loaded successfully";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load image: {ex.Message}", "OK");
        }
    }

    private async void OnExtractTextClicked(object sender, EventArgs e)
    {
        if (_currentImageData == null)
        {
            await DisplayAlert("Error", "Please select an image first", "OK");
            return;
        }

        try
        {
            StatusLabel.Text = "Processing...";
            ResultLabel.Text = string.Empty;

            var extractedText = await ExtractTextFromImage(_currentImageData);
            ResultLabel.Text = extractedText;
            StatusLabel.Text = "Text extracted successfully";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"OCR failed: {ex.Message}", "OK");
            StatusLabel.Text = "OCR failed";
        }
    }

    private async Task<string> ExtractTextFromImage(byte[] imageData)
    {
        return await Task.Run(() =>
        {
            try
            {
                // Get tessdata path based on platform
                string tessDataPath = GetTessDataPath();

                using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromMemory(imageData))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Tesseract processing failed: {ex.Message}");
            }
        });
    }

    private string GetTessDataPath()
    {
#if ANDROID
        // For Android - copy from assets to a writable location
        return CopyTessDataToStorage();
#elif WINDOWS
            // For Windows - use the application directory
            return Path.Combine(AppContext.BaseDirectory, "tessdata");
#else
            throw new PlatformNotSupportedException();
#endif
    }

#if ANDROID
    private string CopyTessDataToStorage()
    {
        var tessDataDir = Path.Combine(FileSystem.AppDataDirectory, "tessdata");
        Directory.CreateDirectory(tessDataDir);

        var trainedDataFiles = new[] { "eng.traineddata" }; // Add more languages as needed

        foreach (var file in trainedDataFiles)
        {
            var destFile = Path.Combine(tessDataDir, file);
            if (!File.Exists(destFile))
            {
                using var assetStream = Android.App.Application.Context.Assets.Open($"tessdata/{file}");
                using var fileStream = File.Create(destFile);
                assetStream.CopyTo(fileStream);
            }
        }

        return tessDataDir;
    }
#endif
}

