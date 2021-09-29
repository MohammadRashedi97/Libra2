using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Libra2
{
    static class Helper
    {

        public static T GetParentListViewItem<T>(FrameworkElement child) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent != null && !(parent is T))
                return (T)GetParentListViewItem<T>((FrameworkElement)parent);
            return (T)parent;
        }

        public static Book getSelectedItemFromListView(ListView bookList)
        {
            Book selectedBook = (Book) bookList.SelectedItem;
            return selectedBook;
        }

        public static Book getBookFromListView(ListViewItem item)
        {
            Book book = (Book)item.Content;
            return book;
        }
    }
}
