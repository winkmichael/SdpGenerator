using Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGen
{
    public static class ProtoToCpp
    {
        public static string TypeToFileName(string typeName, string nameSpace)
        {
            return string.Format("{0}.h", typeName);
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
            sb.AppendTable(tableCount).AppendFormat("enum class{0} : int", entity.Name.Value).NewLine();
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
            sb.Append("#pragma once").NewLine();
            sb.Append("#include <string>").NewLine();
            sb.Append("#include <vector>").NewLine();
            sb.Append("#include <map>").NewLine();
            sb.Append("#include <algorithm>").NewLine();
            //生成头文件引用
            foreach (var type in entity.IncludeCustomType)
            {
                sb.AppendFormat("#include \"{0}\"", TypeToFileName(type, nameSpace)).NewLine();
            }
            int tableCount = 0;
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.Append("namespace ").Append(nameSpace).NewLine().Append('{').NewLine();
                tableCount = 1;
            }
            //生成结构体
            sb.AppendTable(tableCount).AppendFormat("struct {0}", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount).Append('{').NewLine();
            //生成字段
            foreach (var field in entity.Fields)
            {
                sb.AppendTable(tableCount + 1).AppendField(field).NewLine();
            }
            sb.NewLine();
            //生成构造函数
            sb.AppendTable(tableCount + 1).AppendFormat("{0}() = default;", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount + 1).AppendFormat("const char *getName() const {0} return \"{1}\"; {2} ", '{', entity.Name.Value, '}').NewLine();

            //反序列化接口
            sb.AppendTable(tableCount + 1).Append("template <typename T>").NewLine();
            sb.AppendTable(tableCount + 1).Append("void visit(T &t, bool )").NewLine();
            sb.AppendTable(tableCount + 1).Append('{').NewLine();
            foreach (var field in entity.Fields)
            {
                if (field.Type.TypeType == FieldType.Enum)
                    sb.AppendTable(tableCount + 2).AppendFormat("t.visit({0}, false, \"{1}\", (int&){1});", field.Index.IntValue, field.Name.Value).NewLine();
                else
                    sb.AppendTable(tableCount + 2).AppendFormat("t.visit({0}, false, \"{1}\", {1});", field.Index.IntValue, field.Name.Value).NewLine();
            }
            sb.AppendTable(tableCount + 1).Append('}').NewLine();
            //只读序列化接口
            sb.AppendTable(tableCount + 1).Append("template <typename T>").NewLine();
            sb.AppendTable(tableCount + 1).Append("void visit(T &t, bool ) const").NewLine();
            sb.AppendTable(tableCount + 1).Append('{').NewLine();
            foreach (var field in entity.Fields)
            {
                if (field.Type.TypeType == FieldType.Enum)
                    sb.AppendTable(tableCount + 2).AppendFormat("t.visit({0}, false, \"{1}\", (int&){1});", field.Index.IntValue, field.Name.Value).NewLine();
                else
                    sb.AppendTable(tableCount + 2).AppendFormat("t.visit({0}, false, \"{1}\", {1});", field.Index.IntValue, field.Name.Value).NewLine();
            }
            sb.AppendTable(tableCount + 1).Append('}').NewLine();
            //swap
            sb.AppendTable(tableCount + 1).AppendFormat("void swap({0} &b)", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount + 1).Append('{').NewLine();
            foreach (var field in entity.Fields)
            {
                sb.AppendTable(tableCount + 2).AppendFormat("std::swap({0}, b.{0});", field.Name.Value).NewLine();
            }
            //比较操作符
            sb.AppendTable(tableCount + 1).AppendFormat("bool operator== (const {0} &rhs) const", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount + 1).Append('{').NewLine();
            for (int i=0; i< entity.Fields.Count; ++i)
            {
                sb.AppendTable(tableCount + 2);
                if (i == 0)
                    sb.Append("return ");
                else
                    sb.AppendTable(1).Append("&& ");
                sb.AppendFormat("{0} == rhs.{0}", entity.Fields[i].Name.Value);
                if (i == entity.Fields.Count - 1)
                    sb.Append(';');
                sb.NewLine();
            }
            sb.AppendTable(tableCount + 1).Append('}').NewLine();

            sb.AppendTable(tableCount + 1).AppendFormat("bool operator!= (const {0} &rhs) const", entity.Name.Value).NewLine();
            sb.AppendTable(tableCount + 1).Append('{').NewLine();
            sb.AppendTable(tableCount + 2).Append("return !((*this) == rhs);").NewLine();
            sb.AppendTable(tableCount + 1).Append('}').NewLine();

            //结构体结束
            sb.AppendTable(tableCount).Append('}').NewLine();

            //命名空间结束
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.Append('}').NewLine();
            }

            //生成swap
            sb.Append("namespace std").NewLine();
            sb.Append('{').NewLine();
            sb.AppendTable(1).Append("inline void swap(");
            if (!string.IsNullOrEmpty(nameSpace))
                sb.Append(nameSpace).Append("::");
            sb.Append(entity.Name.Value).Append(" &a, ");
            if (!string.IsNullOrEmpty(nameSpace))
                sb.Append(nameSpace).Append("::");
            sb.Append(entity.Name.Value).Append(" &b) ");
            sb.Append('{').Append(" a.swap(b); ").Append('}').NewLine();
            sb.Append('}').NewLine();

            return sb.ToString();
        }

        private static StringBuilder AppendField(this StringBuilder sb, StructField field)
        {
            sb.Append("public ");
            sb.AppendFieldTypeString(field);
            sb.Append(' ').Append(field.Name.Value);
            if (field.Type.TypeType == FieldType.BaseType)
            {
                if (field.Type.Type.Value != "string" && field.Type.Type.Value != "bytes")
                {
                    if (field.Type.Type.Value == "bool")
                        sb.Append(" = false");
                    else
                        sb.Append(" = 0");
                }
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
            if (type.Type.Value == "bytes" || type.Type.Value == "string")
                return "std::string";
            if (type.Type.Value == "int")
                return "int32_t";
            if (type.Type.Value == "uint")
                return "uint32_t";
            if (type.Type.Value == "long")
                return "int64_t";
            if (type.Type.Value == "ulong")
                return "uint64_t";
            if (type.TypeType == FieldType.Vector)
                return "std::vector";
            if (type.TypeType == FieldType.Map)
                return "std::map";
            return type.Type.Value;
        }

        private static string ToExternTypeString(ExternTypeEntity type)
        {
            if (type.Name.Value == "bytes" || type.Name.Value == "string")
                return "std::string";
            if (type.Name.Value == "int")
                return "int32_t";
            if (type.Name.Value == "uint")
                return "uint32_t";
            if (type.Name.Value == "long")
                return "int64_t";
            if (type.Name.Value == "ulong")
                return "uint64_t";
            return type.Name.Value;
        }
    }
}
