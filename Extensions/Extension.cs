using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace eProject3_Vehicle_Showroom_Management.Extensions
{
    public class Extension
    {
        public Extension()
        {
        }

        public static string ConvertToBase64(string imagepath)
        {
            using (Image image = Image.FromFile(imagepath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
    }
}