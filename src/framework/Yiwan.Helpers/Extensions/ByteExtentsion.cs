using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yiwan.Helpers.Extensions
{
    public static class ByteExtentsion
    {
        /// <summary>
        /// 检索bytes[]指定片段，返回新实例
        /// </summary>
        public static byte[] Subbytes(this byte[] bytes, int index, int length)
        {
            if (bytes == null) return null;
            byte[] newBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                if (bytes.Length > index + i) newBytes[i] = bytes[index + i];
            }
            return newBytes;
        }
    }
}
