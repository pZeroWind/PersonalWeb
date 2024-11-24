using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.Managers
{
    public static partial class ExtensionManager
    {
        public static string GetFileName(this string? path)
        {
            if (path != null)
            {
                string? fileName = Path.GetFileNameWithoutExtension(path);
                if (fileName != null) return fileName;
            } 
            return string.Empty;
        }

        public static string ToHex(this long num)
        {
            return $"{num:X}";
        }

        public static long ToID(this string hex)
        {
            if (string.IsNullOrEmpty(hex)) return 0 ;
            return long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }

        public static string ToDateString(this DateTime recordTime)
        {
            TimeSpan timeSpan = DateTime.Now - recordTime;
            if (timeSpan.TotalSeconds < 60)
            {
                return $"{Math.Floor(timeSpan.TotalSeconds)}秒前";
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                return $"{Math.Floor(timeSpan.TotalMinutes)}分钟前";
            }
            else if (timeSpan.TotalHours < 24)
            {
                return $"{Math.Floor(timeSpan.TotalHours)}小时前";
            }
            else if (timeSpan.TotalDays <= 7)
            {
                return $"{Math.Floor(timeSpan.TotalDays)}天前";
            }
            else
            {
                return recordTime.ToString($"yyyy年MM月dd日");
            }
        }
    }
}
