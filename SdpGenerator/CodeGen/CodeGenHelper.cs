using System.IO;
using System.Text;

namespace CodeGen
{
    public static class CodeGenHelper
    {
        public static StringBuilder AppendTable(this StringBuilder sb, int tableNum)
        {
            for (int i = 0; i < tableNum; ++i)
                sb.Append('\t');
            return sb;
        }

        public static StringBuilder NewLine(this StringBuilder sb, int tableNum = 0)
        {
            sb.Append('\n');
            for (int i = 0; i < tableNum; ++i)
                sb.Append('\t');
            return sb;
        }

        public static void WriteFile(string filePath, string txt)
        {
            if (File.Exists(filePath))
            {
                //检查是否有改变，如果没有改变则不生成，防止C++文件重新编译
                if (File.ReadAllText(filePath) == txt)
                    return;
            }
            File.WriteAllText(filePath, txt);
        }

        public static string InputDirHandle(string dir)
        {
            dir = dir.Replace("\\", "/");
            if (!dir.EndsWith('/'))
                dir += "/";
            return dir;
        }
    }
}
