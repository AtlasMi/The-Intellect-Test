using System.Collections.Generic;

namespace The_Intellect_Test
{
    class ImagesClass
    {
        public static string NumberImage(int i)
        {
            string s = null;
            Dictionary<int, string> numi = new Dictionary<int, string>();

            numi.Add(1, "images/pic1.png");
            numi.Add(2, "images/pic2.png");
            numi.Add(3, "images/pic3.png");
            numi.Add(4, "images/pic4.png");
            numi.Add(5, "images/pic5.png");
            numi.Add(6, "images/pic6.png");
            numi.Add(7, "images/pic7.png");
            numi.Add(8, "images/pic8.png");
            numi.Add(9, "images/pic9.png");

            foreach (var a in numi)
            {
                if (i == a.Key)
                {
                    s = a.Value.ToString();
                    break;
                }
            }

            return s;
        }
    }
}
