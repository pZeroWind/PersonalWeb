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
            return long.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
