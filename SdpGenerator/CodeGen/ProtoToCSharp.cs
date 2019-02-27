using Parser;
using System.Text;
namespace CodeGen
{
    public static class ProtoToCSharp
    {
        public static string TypeToFileName(string name, string nameSpace)
        {
            return string.Format("{0}.cs", name);
        }

        public static string EunmToFile(EnumEntity entity, string nameSpace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("//本文件由工具生成，请勿直接修改").NewLine();
            int tableCount = 0;
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.Append("namespace ").Append(nameSpace).NewLine().Append('{').NewLine().NewLine();
                tableCount = 1;
            }
            sb.AppendTable(tableCount).AppendFormat("public enum {0}", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount).Append('{').NewLine();
            foreach (var field in entity.Fields)
            {
                sb.AppendTable(tableCount + 1).Append(field.Name.Value);
                if (field.Value != null)
                {
                    sb.Append(" = ").Append(field.Value.Value);
                }
                sb.Append(',').NewLine();
            }
            sb.AppendTable(tableCount).Append('}').NewLine();

            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.Append('}').NewLine();
            }
            return sb.ToString();
        }

        public static string StructToFile(StructEntity entity, string nameSpace)
        {
            StringBuilder sb = new StringBuilder();
            //生成文件头
            sb.Append("//本文件由工具生成，请勿直接修改").NewLine();
            sb.Append("using System;").NewLine();
            sb.Append("using System.Collections.Generic;").NewLine();

            int tableCount = 0;
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.Append("namespace ").Append(nameSpace).NewLine().Append('{').NewLine();
                tableCount = 1;
            }
            //生成类
            sb.NewLine().AppendTable(tableCount).Append("[Serializable]").NewLine();
            sb.AppendTable(tableCount).AppendFormat("public partial class {0} : Sdp.IStruct", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount).Append('{').NewLine();
            //生成字段
            foreach (var field in entity.Fields)
            {
                sb.AppendTable(tableCount + 1).AppendField(field).NewLine();
            }
            sb.NewLine();
            //生成序列化接口
            sb.AppendTable(tableCount + 1).Append("public void Visit(Sdp.ISdp sdp)").NewLine();
            sb.AppendTable(tableCount + 1).Append('{').NewLine();
            foreach (var field in entity.Fields)
            {
                sb.AppendTable(tableCount + 2);
                if (field.Type.TypeType == FieldType.Enum)
                {
                    sb.AppendFormat("sdp.VisitEunm({0}, \"{1}\", false, ref {1});", field.Index.IntValue, field.Name.Value);
                }
                else if (field.Type.TypeType == FieldType.Struct)
                {
                    sb.AppendFormat("sdp.VisitStruct({0}, \"{1}\", false, ref {1});", field.Index.IntValue, field.Name.Value);
                }
                else if (field.Type.TypeType == FieldType.Vector)
                {
                    sb.AppendFormat("sdp.VisitList({0}, \"{1}\", false, ref {1});", field.Index.IntValue, field.Name.Value);
                }
                else if (field.Type.TypeType == FieldType.Map)
                {
                    sb.AppendFormat("sdp.VisitMap({0}, \"{1}\", false, ref {1});", field.Index.IntValue, field.Name.Value);
                }
                else
                {
                    sb.AppendFormat("sdp.Visit({0}, \"{1}\", false, ref {1});", field.Index.IntValue, field.Name.Value);
                }
                sb.NewLine();
            }
            sb.AppendTable(tableCount + 1).Append('}').NewLine();

            sb.AppendTable(tableCount).Append('}').NewLine();
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.Append('}').NewLine();
            }
            return sb.ToString();
        }
        private static StringBuilder AppendField(this StringBuilder sb, StructField field)
        {
            sb.Append("public ");
            sb.AppendFieldTypeString(field);
            sb.Append(' ').Append(field.Name.Value);
            if (field.Type.TypeType > FieldType.Enum )
            {
                sb.Append(" = new ").AppendFieldTypeString(field).Append("()");
            }
            sb.Append(';');
            return sb;
        }

        private static StringBuilder AppendFieldTypeString(this StringBuilder sb, StructField field)
        {
            sb.Append(ToTypeNameString(field.Type));
            if (field.Type.TypeType == FieldType.Vector)
            {
                sb.Append('<');
                sb.Append(ToExternTypeString(field.Type.ExternTypes[0]));
                sb.Append('>');
            }
            else if (field.Type.TypeType == FieldType.Map)
            {
                sb.Append('<');
                sb.Append(ToExternTypeString(field.Type.ExternTypes[0]));
                sb.Append(',');
                sb.Append(ToExternTypeString(field.Type.ExternTypes[1]));
                sb.Append('>');
            }
            return sb;
        }

        private static string ToTypeNameString(TypeEntity type)
        {
            if (type.Type.Value == "bytes")
                return string.Format("bytes[]");
            if (type.TypeType == FieldType.Vector)
                return "List";
            if (type.TypeType == FieldType.Map)
                return "Dictionary";
            return type.Type.Value;
        }

        private static string ToExternTypeString(ExternTypeEntity type)
        {
            if (type.Name.Value == "bytes")
                return string.Format("bytes[]");
            return type.Name.Value;
        }
    }

}
