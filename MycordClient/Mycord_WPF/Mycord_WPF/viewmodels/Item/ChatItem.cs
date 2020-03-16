using System;
using System.Collections.Generic;
using System.Text;

namespace Mycord_WPF
{
    public class ChatItem : BaseViewModel
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public DateTimeOffset SendTime { get; set; }
    }
}
