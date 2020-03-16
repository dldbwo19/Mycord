using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Mycord_WPF
{
    public class ViewCache : Page
    {
        private Type contentType;
        public Type ContentType
        {
            get { return this.contentType; }
            set
            {
                if(this.contentType == value)
                {
                    return;
                }
                this.contentType = value;
                this.Content = ViewFactory.GetView(this.contentType);
            }
        }

        public ViewCache()
        {
            this.Unloaded += this.viewcache_unloaded;
        }

        private void viewcache_unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= this.viewcache_unloaded;
            this.Content = null;
        }
    }
}
