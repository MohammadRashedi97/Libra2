using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
    /// Interaction logic for BookDetailWindow.xaml
    /// </summary>
    public partial class BookDetailWindow : Window
    {
        public BookDetailWindow(Book book)
        {
            InitializeComponent();
            this.DataContext = book;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = getBookData();

            SQLConnector.UpdateBookInfo(book);

            var detailWindow = Window.GetWindow(this);
            detailWindow.Close();

            // Get Open LibraryWindow
            var window = Application.Current.Windows.OfType<LibraryWindow>().SingleOrDefault(w => w.IsActive);
            // Refresh View
            ICollectionView view = CollectionViewSource.GetDefaultView(window.bookListView.booklist.ItemsSource);
            view.Refresh();

        }

        private Book getBookData()
        {
            Book book = (Book)this.DataContext;
            book.Title = bookTitleTextBox.Text;
            book.Author = authorTextBox.Text;
            book.Publisher = publisherTextBox.Text;
            book.PublicationDate = publicationDateTextBox.Text;
            book.Thumbnail = bookThumbnail.Source.ToString().Replace("file:///", "");

            if (Int32.Parse(pageCountTextBox.Text) >= 0)
                book.PageCount = Int32.Parse(pageCountTextBox.Text);
            else
                MessageBox.Show("Please enter a valid number");

            if (Double.Parse(ratingTextBox.Text) >= 0.0)
                book.Rating = Double.Parse(ratingTextBox.Text);
            else
                MessageBox.Show("Please enter a valid rating");

            book.Description = descriptionTextBox.Text;

            return book;
        }

        private void bookThumbnail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            ofd.InitialDirectory = @"C:\Users\PISHTAZ\source\repos\Libra2\Thumbnails\";
            if (ofd.ShowDialog() == true)
            {
                Uri imageUri = new Uri(ofd.FileName);
                BitmapImage image = new BitmapImage(imageUri);
                bookThumbnail.Source = image;
            }
        }
    }
}
