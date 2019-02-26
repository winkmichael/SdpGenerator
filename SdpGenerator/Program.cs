using CodeGen;
using ITDM;
using Parser;
using System;

namespace SdpGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleCmdLine c = new ConsoleCmdLine();
                CmdLineString srcDir = new CmdLineString("src", true, "源文件目录");
                CmdLineString csharpDir = new CmdLineString("csharp", false, "生成C#文件的目录");
                CmdLineString cppDir = new CmdLineString("cpp", false, "生成C++文件的目录");
                CmdLineString nameSpace = new CmdLineString("namespace", false, "命名空间");
                c.RegisterParameter(srcDir);
                c.RegisterParameter(csharpDir);
                c.RegisterParameter(cppDir);
                c.Parse(args);
                ProtoResult result = Builder.BuildProto(srcDir);
                if (csharpDir.Exists)
                    Builder.GenCSharpFile(result, CodeGenHelper.InputDirHandle(csharpDir), nameSpace);
                if (cppDir.Exists)
                    Builder.GenCppFile(result, CodeGenHelper.InputDirHandle(cppDir), nameSpace);
            }
            catch (System.Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();

                Console.WriteLine("Please press any key to continue ... ");
                Console.ReadKey();
            }
            
        }
    }
}
