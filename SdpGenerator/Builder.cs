using CodeGen;
using Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SdpGenerator
{
    public static class Builder
    {
        public static ProtoResult BuildProto(string srcDir)
        {
            ProtoResult result = new ProtoResult();
            var files = Directory.GetFiles(srcDir, "*.sdp", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                ProtoParser.ParseFile(file, result);
            }
            ProtoCheck.Check(result);
            return result;
        }

        public static void GenCppFile(ProtoResult result, string outDir, string nameSpace)
        {
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            foreach (var entity in result.Enums)
            {
                string saveFile = Path.Combine(outDir, ProtoToCpp.TypeToFileName(entity.Name.Value, nameSpace));
                string code = ProtoToCpp.EunmToFile(entity, nameSpace);
                CodeGenHelper.WriteFile(saveFile, code);
            }

            foreach (var entity in result.Structs)
            {
                string saveFile = Path.Combine(outDir, ProtoToCpp.TypeToFileName(entity.Name.Value, nameSpace));
                string code = ProtoToCpp.StructToFile(entity, nameSpace);
                CodeGenHelper.WriteFile(saveFile, code);
            }
        }

        public static void GenCSharpFile(ProtoResult result, string outDir, string nameSpace)
        {
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            foreach (var entity in result.Enums)
            {
                string saveFile = Path.Combine(outDir, ProtoToCSharp.TypeToFileName(entity.Name.Value, nameSpace));
                string code = ProtoToCSharp.EunmToFile(entity, nameSpace);
                CodeGenHelper.WriteFile(saveFile, code);
            }

            foreach (var entity in result.Structs)
            {
                string saveFile = Path.Combine(outDir, ProtoToCSharp.TypeToFileName(entity.Name.Value, nameSpace));
                string code = ProtoToCSharp.StructToFile(entity, nameSpace);
                CodeGenHelper.WriteFile(saveFile, code);
            }
        }
    }
}
