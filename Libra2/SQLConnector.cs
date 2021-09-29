using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libra2
{
    static class SQLConnector
    {
        static readonly string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        
        public static ObservableCollection<Book> LoadAllBooks()
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                string command = "select * from Book";
                var output = connection.Query<Book>(command, new DynamicParameters());
                ObservableCollection<Book> books = new ObservableCollection<Book>(output);
                foreach (Book book in books)
                {
                    if (!File.Exists(book.Thumbnail))
                        book.Thumbnail = "Images/not-available.png";
                }
                return books;
            }
        }

        public static void UpdateBookInfo(Book book)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                string command =
                                "UPDATE Book SET "
                                + "Title = '" + book.Title + "',"
                                + "Author = '" + book.Author + "',"
                                + "Publisher = '" + book.Publisher + "',"
                                + "PublicationDate = '" + book.PublicationDate + "',"
                                + "ISBN = '" + book.ISBN + "',"
                                + "Thumbnail = '" + book.Thumbnail + "',"
                                + "PageCount = " + book.PageCount + ","
                                + "Rating = " + book.Rating + ","
                                + "Description = '" + book.Description + "'";

                connection.Execute("update Book set " +
                    "Title = @Title," +
                    "Author = @Author," +
                    "Publisher = @Publisher," +
                    "PublicationDate = @PublicationDate," +
                    "ISBN = @ISBN," +
                    "Thumbnail = @Thumbnail," +
                    "PageCount = @PageCount," +
                    "Rating = @Rating," +
                    "Description = @Description where Id = @Id", book);
            }
        }

        public static void DeleteBook(Book book)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                //string command = "delete from Book where Id = @Id";
                connection.Execute("delete from Book where Id = @Id", book);
            }
        }

        public static void AddNewBook(Book book)
        {
            using (IDbConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Execute("insert into Book (Title, Author, Publisher, PublicationDate," +
                    " ISBN, Thumbnail, PageCount, Rating, Description) values (@Title, @Author, @Publisher, " +
                    "@PublicationDate, @ISBN, @Thumbnail, @PageCount, @Rating, @Description)", book);
            }
        }

    }
}
