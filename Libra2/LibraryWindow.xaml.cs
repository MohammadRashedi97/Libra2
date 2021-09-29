using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Libra2
{
    /// <summary>
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            InitializeComponent();
            ObservableCollection<Book> books = SQLConnector.LoadAllBooks();
            bookListView.booklist.ItemsSource = books;

            editToolbarButton.IsEnabled = false;
            editToolbarButton.Foreground = Brushes.Gray;

            deleteToolbarButton.IsEnabled = false;
            deleteToolbarButton.Foreground = Brushes.Gray;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow mainWindow = new SearchWindow();
            mainWindow.Show();
        }

        // Hiding Overflow on the toolbar
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void editToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = Helper.getSelectedItemFromListView(bookListView.booklist);
            BookDetailWindow bookDetailWindow = new BookDetailWindow(book);
            bookDetailWindow.Show();
        }

        private void deleteToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = Helper.getSelectedItemFromListView(bookListView.booklist);
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
    }
}
