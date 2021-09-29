using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading;

namespace Libra2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            searchBox.Focus();
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = searchBox.Text;
            dynamic items = await Task.Run(() => GetSearchData(searchValue));
            List<Book> books = PopulateBooks(items);
       
            ResultsWindow resultsForm = new ResultsWindow(books);
            resultsForm.WindowState = WindowState.Maximized;
            resultsForm.Show();
            this.Close();

            foreach (Window window in Application.Current.Windows.OfType<LibraryWindow>())
            {
                ((LibraryWindow)window).Close();
            }

        }

        private dynamic GetSearchData(string searchValue)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://www.googleapis.com/books/v1/volumes?q=" + searchValue + "&maxResults=40"));

            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string jsonString;

            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            dynamic items = JsonConvert.DeserializeObject<dynamic>(jsonString);
            return items;
        }

        private List<Book> PopulateBooks(dynamic items)
        {
            List<Book> books = new List<Book>();

            foreach (var item in items.items)
            {
                Book book = new Book();

                book.Id = item.id;

                book.Title = item.volumeInfo.title;

                if (item.volumeInfo.authors != null)
                    book.Author = item.volumeInfo.authors[0];
                else book.Author = "N/A";

                if (item.volumeInfo.publisher != null)
                    book.Publisher = item.volumeInfo.publisher;
                else book.Publisher = "N/A";

                if (item.volumeInfo.industryIdentifiers != null)
                    book.ISBN = item.volumeInfo.industryIdentifiers[0].identifier;
                else book.ISBN = "N/A";

                if (item.volumeInfo.publishedDate != null)
                    book.PublicationDate = item.volumeInfo.publishedDate;
                else book.PublicationDate = "N/A";

                if (item.volumeInfo.imageLinks != null)
                    book.Thumbnail = item.volumeInfo.imageLinks.thumbnail;
                else book.Thumbnail = "N/A";

                if (item.volumeInfo.pageCount != null)
                    book.PageCount = item.volumeInfo.pageCount;
                else book.PageCount = 0;

                if (item.volumeInfo.averageRating != null)
                    book.Rating = item.volumeInfo.averageRating;
                else book.Rating = 0;

                if (item.volumeInfo.description != null)
                    book.Description = item.volumeInfo.description;
                else book.Description = "N/A";

                books.Add(book);
            }

            return books;
        }
    }
}
