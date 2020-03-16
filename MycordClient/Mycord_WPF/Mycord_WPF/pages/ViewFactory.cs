using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Mycord_WPF
{
    public static class ViewFactory
    {
        private static Dictionary<Type, object> view_cache = new Dictionary<Type, object>();

        public static object GetView(Type type)
        {
            if (view_cache.ContainsKey(type) == false)
            {
                var page = Activator.CreateInstance(type);
                if(page == null)
                {
                    MessageBox.Show($"Can't create page {type}");
                    return null;
                }
                view_cache.Add(type, page);
            }


            return view_cache[type];
        }
    }
}
