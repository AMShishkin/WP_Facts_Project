using Facts.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Facts
{
    public partial class ListPage : PhoneApplicationPage
    {
        private BitmapImage _bitmapImage;
        private KeyValuePair<string, object> _selectPair;
        private int _indexPage = 0, _lastIndexItem;

        public ListPage()
        {
            InitializeComponent();

            _bitmapImage = new BitmapImage();
            _selectPair = new KeyValuePair<string, object>();
            _indexPage = _lastIndexItem = this.IndexPage = 0;
        }

        // ----------------------------------------------------------------------------------------------------------------- //
        private int IndexPage
        {
            get { return _indexPage; }
            set { if (value < AppHelper.Storage.Count && value >= 0) _indexPage = value; }
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            SwipeAnimation.Begin();
            this.ApplicationBar.Mode = (AppHelper.AppBar) ? Microsoft.Phone.Shell.ApplicationBarMode.Default : Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
            TBFirst.TextAlignment = TBSecond.TextAlignment = TBThird.TextAlignment = (AppHelper.TextStyle) ? TextAlignment.Left : TextAlignment.Center;
            if (AppHelper.Storage.Contains("LAST_INDEX_PAGE")) AppHelper.Storage.Remove("LAST_INDEX_PAGE");
            if (AppHelper.Storage.Contains("IS_RATE")) AppHelper.Storage.Remove("IS_RATE");
            if (AppHelper.Storage.Contains("APP_TEXT_STYLE")) AppHelper.Storage.Remove("APP_TEXT_STYLE");
            if (AppHelper.Storage.Contains("APP_BAR")) AppHelper.Storage.Remove("APP_BAR");
            if (AppHelper.Storage.Count > 0) this.SelectedIndexLogic(false);
            else this.EmptyList();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!AppHelper.Storage.Contains("LAST_INDEX_PAGE")) AppHelper.Storage.Add("LAST_INDEX_PAGE", AppHelper.PageIndex);
            if (!AppHelper.Storage.Contains("IS_RATE")) AppHelper.Storage.Add("IS_RATE", AppHelper.IsRate);
            if (!AppHelper.Storage.Contains("APP_BAR")) AppHelper.Storage.Add("APP_BAR", AppHelper.AppBar);
            if (!AppHelper.Storage.Contains("APP_TEXT_STYLE")) AppHelper.Storage.Add("APP_TEXT_STYLE", AppHelper.TextStyle);
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void MainPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 1) this.IndexPage--;
            else if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 2) this.IndexPage++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 0) this.IndexPage++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 2) this.IndexPage--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 0) this.IndexPage--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 1) this.IndexPage++;
            _lastIndexItem = MainPivot.SelectedIndex;
            SVFirst.ScrollToVerticalOffset(0.0d);
            SVSecond.ScrollToVerticalOffset(0.0d);
            SVThird.ScrollToVerticalOffset(0.0d);
            if (AppHelper.Storage.Count > 0) this.SelectedIndexLogic(true);
            else this.EmptyList();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void SelectedIndexLogic(bool start)
        {
            _selectPair = AppHelper.Storage.ElementAt(this.IndexPage);
            if (MainPivot.SelectedIndex == 0) TBFirst.Text = BaseLineDB.ResourceManager.GetString(_selectPair.Value.ToString());
            else if (MainPivot.SelectedIndex == 1) TBSecond.Text = BaseLineDB.ResourceManager.GetString(_selectPair.Value.ToString());
            else TBThird.Text = BaseLineDB.ResourceManager.GetString(_selectPair.Value.ToString());
            TBNumber.Text = "#" + _selectPair.Key;
            _bitmapImage.UriSource = new Uri("/Resources/im" + _selectPair.Key + ".jpg", UriKind.Relative);
            ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void EmptyList()
        {
            ((ApplicationBarIconButton)this.ApplicationBar.Buttons[0]).IsEnabled = ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IsEnabled = false;
            _bitmapImage.UriSource = new Uri("/Resources/imError.jpg", UriKind.Relative);
            ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;
            TBFirst.Text = TBSecond.Text = TBThird.Text = "Пусто";
            TBNumber.Text = "****";
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppListRemove_Click(object sender, EventArgs e)
        {
            if (AppHelper.Storage.Count > 0)
            {
                AppHelper.Storage.Remove(_selectPair.Key);
                if (AppHelper.Storage.Count <= 0) this.EmptyList();
                else
                {
                    this.IndexPage--;
                    this.SelectedIndexLogic(false);
                }
            }
            else this.EmptyList();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppShow_Click(object sender, EventArgs e)
        {
            ShareStatusTask _shareStatusTask = new ShareStatusTask();
            _shareStatusTask.Status = "[И это факт #" + _selectPair.Key + "]\n" + BaseLineDB.ResourceManager.GetString(_selectPair.Value.ToString());
            _shareStatusTask.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        //private void AppSave_Click(object sender, EventArgs e)
        //{
        //    if (AppHelper.IsTrial) MessageBox.Show("Данная функция доступна в полной версии программы", "Сохранить совет в виде изображения", MessageBoxButton.OK);
        //    else
        //    {
        //        var streamResourceInfo = Application.GetResourceStream(new Uri("Background.png", UriKind.Relative));
        //        var image = new BitmapImage();
        //        image.SetSource(streamResourceInfo.Stream);
        //        var writeableBitmap = new WriteableBitmap(image);
        //        var bitmap = new BitmapImage(_bitmapImage.UriSource);
        //        var im = new Image() { Width = 480, Height = 350, Source = bitmap, Stretch = Stretch.Fill };

        //        var textBlockTitle = new TextBlock
        //        {
        //            FontSize = 28,
        //            Width = 200,
        //            Height = 60,
        //            FontWeight = FontWeights.Bold,
        //            TextAlignment = TextAlignment.Right,
        //            TextWrapping = TextWrapping.Wrap,
        //            Foreground = new SolidColorBrush(Color.FromArgb(210, 210, 210, 210)),
        //            Text = "СОВЕТ #" + _selectPair.Key,
        //        };

        //        var textBlockText = new TextBlock
        //        {
        //            FontSize = 26,
        //            Width = 470,
        //            Height = 450,
        //            TextAlignment = TextAlignment.Center,
        //            TextWrapping = TextWrapping.Wrap,
        //            FontWeight = FontWeights.Bold,
        //            Foreground = new SolidColorBrush(Color.FromArgb(210, 210, 210, 210)),
        //            Text = BaseLineDB.ResourceManager.GetString("String" + _selectPair.Key),
        //        };

        //        writeableBitmap.Render(textBlockTitle, new TranslateTransform { X = 270, Y = 8, });
        //        writeableBitmap.Render(textBlockText, new TranslateTransform { X = 5, Y = 400, });

        //        MessageBox.Show("Совет сохранен в библиотеку фотографий.", "Выполнено", MessageBoxButton.OK);

        //        writeableBitmap.Render(im, new TranslateTransform { X = 0, Y = 40, });
        //        writeableBitmap.Invalidate();

        //        using (var mediaLibrary = new MediaLibrary())
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                writeableBitmap.SaveJpeg(stream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
        //                stream.Seek(0, SeekOrigin.Begin);
        //                mediaLibrary.SavePicture("Cовет_" + AppHelper.PageIndex + ".jpg", stream);
        //            }
        //        }
        //    }
        //}
    }
}