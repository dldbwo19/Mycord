using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MyCordLib
{
    internal class BufferManager
    {
        private int m_numBytes;
        private byte[] m_buffer;
        private Stack<int> m_freeIndexPool;
        private int m_currentIndex;
        private int m_bufferSize;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        public void initBuffer()
        {
            m_buffer = new byte[m_numBytes];
        }

        /// <summary>
        /// 이런 식으로 작업을 했을 경우의 이 점은 힙 메모리의 파편화가 되지 않는다.
        /// 하나의 큰 버퍼를 생성하고 그 버퍼의 영역별로 UserOperation 버퍼로 사용하기 때문이다.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }

                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        /// <summary>
        /// m_freeIndexPool 메모리는 args.userToken이 해제되는 경우
        /// 예를 들면, 사용자가 서버에서 로그아웃을 했을 경우에 서버의 버퍼 메모리에서
        /// 해당 사용자에 대한 요청과 응답이 받을 필요 없기 때문에 버퍼를 해제하는 것이다.
        /// </summary>
        /// <param name="args"></param>
        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
