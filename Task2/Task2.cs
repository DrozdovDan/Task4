using static System.Text.Encoding;

namespace Task2
{
    public class Task2
    {
       
        public static void Main(string[] args)
        {
            var fileName = args[0];
            var e1 = GetEncoding(args[1]);
            var e2 = GetEncoding(args[2]);
            StreamReader w1 = new StreamReader(fileName, e1);
            var text = w1.ReadToEnd();
            w1.Close();
            StreamWriter w2 = new StreamWriter(fileName, false, e2);
            w2.Write(text);
            w2.Close();
        }
    }
}
