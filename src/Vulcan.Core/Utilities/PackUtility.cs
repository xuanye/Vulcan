using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MsgPack.Serialization;
using System.IO;

namespace Vulcan.Core.Utilities
{
    public static class PackUtility
    {
        /// <summary>
        /// 打包对象到字节数组byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] Pack<T>(T item)
        {
           var serializer = SerializationContext.Default.GetSerializer<T>();
            byte[] buf = null;
            if(item != null)
            {
                using (var stream = new MemoryStream())
                {
                    serializer.Pack(stream, item);
                    buf = stream.ToArray();
                }
            }       
            return buf;            
        }

        public static byte[] Pack(Type type,object item)
        {           
            var serializer = SerializationContext.Default.GetSerializer(type);
            byte[] buf = null;
            if (item != null)
            {
                using (var stream = new MemoryStream())
                {
                    serializer.Pack(stream, item);
                    buf = stream.ToArray();
                }
            }
            return buf;
        }
        /// <summary>
        /// 从字节数组解包到到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T UnPack<T>(byte[] buf)
        {
            var serializer = SerializationContext.Default.GetSerializer<T>();
            var t = default(T);
            if(buf !=null && buf.Length > 0)
            {
                using (var stream = new MemoryStream(buf))
                {
                    t = serializer.Unpack(stream);
                }
            }       
            return t;
        }
    }
}
