﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyCordLib
{
    class Defines
    {
        public static readonly short HEADERSIZE = 2;
    }

    class CMessageConverter
    {
        private int message_size;

        private byte[] message_buffer = new byte[1024];
        private int current_position;
        private int position_to_read;
        private int remain_bytes;

        public delegate void CompletedMessageCallback(Const<byte[]> buffer);

        public CMessageConverter()
        {
            this.message_size = 0;
            this.current_position = 0;
            this.position_to_read = 0;
            this.remain_bytes = 0;
        }

        private bool read_until(byte[] buffer, ref int src_position, int offset, int transffered)
        {
            if (this.current_position >= offset + transffered)
            {
                return false;
            }

            int copy_size = this.position_to_read - this.current_position;

            if (this.remain_bytes < copy_size)
            {
                copy_size = this.remain_bytes;
            }

            Array.Copy(buffer, src_position, this.message_buffer, this.current_position, copy_size);

            src_position += copy_size;

            this.current_position += copy_size;

            this.remain_bytes -= copy_size;

            if (this.current_position < this.position_to_read)
            {
                return false;
            }

            return true;
        }

        public void on_receive(byte[] buffer, int offset, int transffered, CompletedMessageCallback callback)
        {
            this.remain_bytes = transffered;

            int src_position = offset;

            while (this.remain_bytes > 0)
            {
                bool completed = false;

                if (this.current_position < Defines.HEADERSIZE)
                {
                    this.position_to_read = Defines.HEADERSIZE;

                    completed = read_until(buffer, ref src_position, offset, transffered);
                    if (!completed)
                    {
                        return;
                    }

                    this.message_size = get_body_size();

                    this.position_to_read = this.message_size + Defines.HEADERSIZE;
                }

                completed = read_until(buffer, ref src_position, offset, transffered);

                if (completed)
                {
                    callback(new Const<byte[]>(this.message_buffer));

                    clear_buffer();
                }
            }
        }

        private int get_body_size()
        {
            Type type = Defines.HEADERSIZE.GetType();
            if (type.Equals(typeof(Int16)))
            {
                return BitConverter.ToInt16(this.message_buffer, 0);
            }

            return BitConverter.ToInt32(this.message_buffer, 0);
        }

        private void clear_buffer()
        {
            Array.Clear(this.message_buffer, 0, this.message_buffer.Length);

            this.current_position = 0;
            this.message_size = 0;
        }
    }
}
