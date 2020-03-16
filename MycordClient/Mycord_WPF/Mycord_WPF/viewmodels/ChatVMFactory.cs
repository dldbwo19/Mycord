using System;
using System.Collections.Generic;
using System.Text;

namespace Mycord_WPF
{
    public static class ChatVMFactory
    {
        private static Dictionary<string, ChatPageVM> vmCache = new Dictionary<string, ChatPageVM>();

        public static ChatPageVM GetViewModel(string RoomName)
        {
            if (!vmCache.ContainsKey(RoomName)) 
            {
                return null;
            }
            return vmCache[RoomName];
        }

        public static void PushViewModel(string RoomName, ChatPageVM chatPageVM)
        {
            if (vmCache.ContainsKey(RoomName) == false)
            {
                if (chatPageVM == null)
                {
                    throw new InvalidOperationException("Couldn't push view Model");
                }
                vmCache.Add(RoomName, chatPageVM);
            }
        }

        public static void DeleteViewModel(string RoomName)
        {
            if (vmCache.ContainsKey(RoomName))
            {
                vmCache.Remove(RoomName);
            }
        }
    }
}
