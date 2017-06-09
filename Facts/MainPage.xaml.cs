using Facts.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;



namespace Facts
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly BitmapImage _bitmapImage;
        private const int TRIALCOUNT = 30;
        private readonly Uri _uriHeart, _uriHeartBreak;
        private MessageBoxResult _messageResult;
        private int _noRateIndexPage, _lastIndexItem;
        private Dictionary<int, string> liveTileContent;

        public MainPage()
        {
            InitializeComponent();
            _uriHeart = new Uri("/Assets/appbar.heart.png", UriKind.Relative);
            _uriHeartBreak = new Uri("/Assets/appbar.heart.break.png", UriKind.Relative);
            _bitmapImage = new BitmapImage();

            if (!AppHelper.Storage.Contains("LAST_INDEX_PAGE")) AppHelper.Storage.Add("LAST_INDEX_PAGE", AppHelper.PageIndex);
            if (!AppHelper.Storage.Contains("IS_RATE")) AppHelper.Storage.Add("IS_RATE", false);
            if (!AppHelper.Storage.Contains("APP_BAR")) AppHelper.Storage.Add("APP_BAR", false);
            if (!AppHelper.Storage.Contains("APP_TEXT_STYLE")) AppHelper.Storage.Add("APP_TEXT_STYLE", false);


            AppHelper.PageIndex = (int)AppHelper.Storage["LAST_INDEX_PAGE"];
            AppHelper.IsRate = (bool)AppHelper.Storage["IS_RATE"];
            AppHelper.AppBar = (bool)AppHelper.Storage["APP_BAR"];
            AppHelper.TextStyle = (bool)AppHelper.Storage["APP_TEXT_STYLE"];

            PBarLeft.Maximum = PBarRight.Maximum = AppHelper.MAXSIZEBASEDB = (AppHelper.IsTrial) ? TRIALCOUNT : 100;

            liveTileContent = new Dictionary<int, string>();
            liveTileContent.Add(1, "70% всех проданных в мире лодок используются для рыболовства!");
            liveTileContent.Add(2, "Первым продуктом, имевшим штрих-код, стала жвачка Wrigley!");
            liveTileContent.Add(3, "Миг – единица времени длительностью в одну сотую секунды!");
            liveTileContent.Add(4, "В 1830-х годах кетчуп продавали как лекарство!");
            liveTileContent.Add(5, "Сердце ежа бьется 300 раз в минуту!");

            LikeEffect.Completed += LikeEffect_Completed;
            RemoveEffect.Completed += RemoveEffect_Completed;
        }

        // ----------------------------------------------------------------------------------------------------------------- //
        private void LikeEffect_Completed(object sender, EventArgs e)
        {
            LikeLogo.Visibility = System.Windows.Visibility.Visible;
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void RemoveEffect_Completed(object sender, EventArgs e)
        {
            LikeLogo.Visibility = System.Windows.Visibility.Collapsed; 
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void LikeLogic()
        {
            if (AppHelper.Storage.Contains(AppHelper.PageIndex.ToString()))
            {
                LikeLogo.Visibility = System.Windows.Visibility.Visible;
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = "удалить";
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IconUri = _uriHeartBreak;
                LikeEffect.Begin();
            }
            else
            {   
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).Text = "добавить";
                ((ApplicationBarIconButton)this.ApplicationBar.Buttons[1]).IconUri = _uriHeart;
                RemoveEffect.Begin();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void SelectedIndexLogic()
        {
            // Демо 
            if (AppHelper.IsTrial && AppHelper.PageIndex == TRIALCOUNT)
            {
                TBFirst.Text = TBSecond.Text = TBThird.Text = "Понравилось приложение?\nХочется еще больше интересных фактов со всего мира?\nПриобретите полную версию приложения и наслаждайтесь сотнями интересных и необычных фактов со всего мира!\n( ... -> полная версия )\n\nВ полной версии приложения Вы получите:\n+ Еще больше интересных фактов\n+ Полное отсутствие рекламы\n+ Лучшие факты со всего мира\n\nP.S.\nСамые интересные и увлекательные факты находятся в полной версии программы";
                _bitmapImage.UriSource = new Uri("/Resources/imFull.jpg", UriKind.Relative);
                TBSelect.Text = "**";
                ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;
                PBarLeft.Value = PBarRight.Value = TRIALCOUNT;
            }
            else
            {
                if (MainPivot.SelectedIndex == 0) TBFirst.Text = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex);
                else if (MainPivot.SelectedIndex == 1) TBSecond.Text = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex);
                else TBThird.Text = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex);
                _bitmapImage.UriSource = new Uri("/Resources/im" + AppHelper.PageIndex + ".jpg", UriKind.Relative);
                TBSelect.Text = "#" + AppHelper.PageIndex;
                ImageFirst.Source = ImageSecond.Source = ImageThird.Source = _bitmapImage;
                PBarLeft.Value = PBarRight.Value = AppHelper.PageIndex;
                this.LikeLogic();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void Assessment()
        {
            if (_noRateIndexPage >= 20)
            {
                MPStartAnimation.Stop();
                _noRateIndexPage = -10;
                _messageResult = MessageBox.Show("Помогите мне сделать программу лучше! Оставьте отзыв и оцените приложение!", "Понравилось приложение?", MessageBoxButton.OKCancel);
            }
            else _noRateIndexPage++;

            if (_messageResult == MessageBoxResult.Cancel)
            {
             //   MPStartAnimation.Begin();
                _messageResult = MessageBoxResult.None;
            }
            else if (_messageResult == MessageBoxResult.OK)
            {
                AppHelper.Storage["IS_RATE"] = AppHelper.IsRate = true;
                AppHelper.Storage.Save();
                MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                marketplaceReviewTask.Show();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void SelectAdvice(string text)
        {
            var tbSelectIndex = 0;
            try
            {
                for (var i = 0; i < text.Length; i++)
                    if (text[i] == ',' || text[i] == '.' || text[i] == '#' || i > 5)
                    {
                        text = text.Substring(0, i);
                        break;
                    }
                                 
                tbSelectIndex = (text != "") ? Convert.ToInt32(text) : tbSelectIndex = -1;
                TBSelect.Text = (tbSelectIndex > AppHelper.MAXSIZEBASEDB || tbSelectIndex < 0) ? "ФАКТ #" + AppHelper.PageIndex : "ФАКТ #" + text;
                AppHelper.PageIndex = tbSelectIndex;
                this.SelectedIndexLogic();
            }
            catch (Exception)
            {
                AppHelper.PageIndex = 0;
                this.SelectedIndexLogic();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SwipeAnimation.Begin();
            this.ApplicationBar.Mode = (AppHelper.AppBar) ? Microsoft.Phone.Shell.ApplicationBarMode.Default : Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
            TBFirst.TextAlignment = TBSecond.TextAlignment = TBThird.TextAlignment = (AppHelper.TextStyle) ? TextAlignment.Left : TextAlignment.Center;
            if (!AppHelper.IsTrial && ApplicationBar.MenuItems.Count >= 3) ApplicationBar.MenuItems.RemoveAt(2);
            this.LikeLogic();
            Adv.Visibility = (AppHelper.IsTrial) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var appTile = ShellTile.ActiveTiles.First();
            var appTileDate = new StandardTileData()
            {
                Title = "И это факт",
                BackTitle = "И это факт",
                BackContent = liveTileContent.ElementAt(new Random().Next(0, liveTileContent.Count)).Value,
            };
            appTile.Update(appTileDate);
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppConf_Click(object sender, EventArgs e)
        {
          //NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppShow_Click(object sender, EventArgs e)
        {
            ShareStatusTask sharedStatusTask = new ShareStatusTask() { Status = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex) };
            sharedStatusTask.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppListAdd_Click(object sender, EventArgs e)
        {
            if (AppHelper.IsTrial) MessageBox.Show("Данная возможсть станет доступной после покупки полной версии приложения\n\n( ... -> Полная версия )", "Сохранить факт в избранное", MessageBoxButton.OK);
            else
            {
                if (!AppHelper.Storage.Contains(AppHelper.PageIndex.ToString())) AppHelper.Storage.Add(AppHelper.PageIndex.ToString(), "String" + AppHelper.PageIndex);
                else AppHelper.Storage.Remove(AppHelper.PageIndex.ToString());
                AppHelper.Storage.Save();
                this.LikeLogic();
                this.SelectedIndexLogic();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppList_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
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
        //            Text = "СОВЕТ #" + AppHelper.PageIndex,
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
        //            Text = BaseLineDB.ResourceManager.GetString("String" + AppHelper.PageIndex),
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
        private void MainPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 1) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 2) AppHelper.PageIndex++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 0) AppHelper.PageIndex++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 2) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 0) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 1) AppHelper.PageIndex++;
            _lastIndexItem = MainPivot.SelectedIndex;
            SVFirst.ScrollToVerticalOffset(0.0d);
            SVSecond.ScrollToVerticalOffset(0.0d);
            SVThird.ScrollToVerticalOffset(0.0d);
            this.SelectedIndexLogic();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void MainPivot_LoadedPivotItem(object sender, PivotItemEventArgs e)
        {
            if (!AppHelper.IsRate) this.Assessment();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void TBSelect_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TBSelect.Text = "";
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void TBSelect_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.SelectAdvice(TBSelect.Text);
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void MainPivot_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            this.Focus();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppFull_Click(object sender, EventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;
            marketplaceDetailTask.Show();
        }
    }
}