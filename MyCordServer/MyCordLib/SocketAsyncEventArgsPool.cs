using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MyCordLib
{
    internal class SocketAsyncEventArgsPool
    {
        Stack<SocketAsyncEventArgs> m_pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs data)
        {
            if (data == null)
            {
                Console.WriteLine("Items added to a SocketAsyncEventArgsPool cannot be null");
                return;
            }

            lock (m_pool)
            {
                m_pool.Push(data);
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        public int Count
        {
            get
            {
                return m_pool.Count;
            }
        }
    }
}
