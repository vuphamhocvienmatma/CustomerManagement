using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class DocumentHelper
    {

        public static bool isCheckFormatFile(string fileName)
        {
            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "jpg", "jpeg", "png", "mp3", "mp4" };
            if (!supportedTypes.Contains(fileName)) return true;
            return false;
        }
        public static bool isCheckMaxSizeFile(long fileSize)
        {
            var maxSize = 2e+7;
            if (fileSize > maxSize)
            {
                return true;
            }
            return false;
        }
    }
}
