using System;
using System.Collections.Generic;
using System.Text;

namespace Mycord_WPF
{
    public static class VMFactory
    {
        private static Dictionary<Type, object> vmCache = new Dictionary<Type, object>();

        public static object GetViewModel(Type type)
        {
            if (vmCache.ContainsKey(type) == false)
            {
                var viewModel = Activator.CreateInstance(type);
                if (viewModel == null)
                {
                throw new InvalidOperationException("Couldn't get view Model" + type);
                }
                vmCache.Add(type, viewModel);
            }
            return vmCache[type];
        }

        public static void PushViewModel(Type type, Object obj)
        {
            if (vmCache.ContainsKey(type) == false)
            {
                if (obj == null)
                {
                    throw new InvalidOperationException("Couldn't push view Model" + type);
                }
                vmCache.Add(type, obj);
            }
        }
    }
}
