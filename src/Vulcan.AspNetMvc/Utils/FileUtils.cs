using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Vulcan.AspNetMvc.Utils
{
    public class FileUtils
    {
        public static void WriteToFile(string filePath, string message)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    sw.Write(message);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("写入文件出错 路径：{0}，原因：{1}", filePath, ex.ToString());
                //EventLog.WriteEntry("application", msg, EventLogEntryType.Error, 200);
                
                throw new Exception(msg, ex);
            }
        }
    }
}
