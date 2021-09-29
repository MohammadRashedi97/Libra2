using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Libra2
{
    /// <summary>
    /// Interaction logic for BookListView.xaml
    /// </summary>
    public partial class BookListView : UserControl
    {
        string thumbnailPath = ConfigurationManager.AppSettings["ThumbnailPath"];
        public BookListView()
        {
            InitializeComponent();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var parentListViewItem = Helper.GetParentListViewItem<ListViewItem>(sender as Button);
            Book book = Helper.getBookFromListView(parentListViewItem);
            BookDetailWindow bookDetailWindow = new BookDetailWindow(book);
            bookDetailWindow.Show();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var parentListViewItem = Helper.GetParentListViewItem<ListViewItem>(sender as Button);
            Book book = Helper.getBookFromListView(parentListViewItem);
            SQLConnector.DeleteBook(book);

            ObservableCollection<Book> books = SQLConnector.LoadAllBooks();
            // Get Open LibraryWindow
            var window = Application.Current.Windows.OfType<LibraryWindow>().SingleOrDefault(w => w.IsActive);
            window.bookListView.booklist.ItemsSource = books;

            // Refresh View
            ICollectionView view = CollectionViewSource.GetDefaultView(window.bookListView.booklist.ItemsSource);
            view.Refresh();

            MessageBox.Show("Book Successfully Deleted.");
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get Parent Grid Name
            dynamic grid = this.Parent;
            var gridName = grid.Name;

            if (gridName == "resultsGrid")
            {
                // Get Book Info
                Book book = Helper.getSelectedItemFromListView(booklist);

                string savePath = thumbnailPath + "\\" + book.Title + "_thumbnail.jpg";

                DownloadBookThumbnail(book.Thumbnail, savePath);

                book.Thumbnail = savePath;

                SQLConnector.AddNewBook(book);
            }
            else if (gridName == "libraryGrid")
            {
                Book book = Helper.getSelectedItemFromListView(booklist);

                BookDetailWindow bookDetailWindow = new BookDetailWindow(book);
                bookDetailWindow.Show();
            }
        }

        private void DownloadBookThumbnail(string thumbnail, string savePath)
        {
            //Downloading and Saving the thumnbail
            using (WebClient client = new WebClient())
            {
                Uri thumbnailUri = new Uri(thumbnail);

                client.DownloadFileAsync(thumbnailUri, savePath);
                client.DownloadFileCompleted += DownloadFileCompleted;
            }
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            LibraryWindow library = new LibraryWindow();
            library.Show();

            System.Threading.Thread.Sleep(500);
            MessageBox.Show("Book added to the library.");

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void booklist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(booklist.SelectedItems.Count == 1)
            {
                // Get Parent Grid Name
                dynamic grid = this.Parent;
                var gridName = grid.Name; 
                if(gridName == "libraryGrid")
                {
                    // Get Open LibraryWindow
                    var window = Application.Current.Windows.OfType<LibraryWindow>().SingleOrDefault(w => w.IsActive);

                    window.editToolbarButton.IsEnabled = true;
                    window.editToolbarButton.Foreground = Brushes.Black;

                    window.deleteToolbarButton.IsEnabled = true;
                    window.deleteToolbarButton.Foreground = Brushes.Black;
                }
            }
        }
    }
}
