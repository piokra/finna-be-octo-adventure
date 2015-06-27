using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNWF.Utill
{
    public static class FileFactory
    {
        public static String readString(String file_path)
        {
            try
            {
                return File.ReadAllText(file_path);
            }
            catch (Exception e)
            {
                return e.ToString();
                
            }
            return "";
        }
    }
}
