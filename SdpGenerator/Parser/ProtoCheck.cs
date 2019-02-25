using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public static class ProtoCheck
    {
        public static void Check(ProtoResult result)
        {
            Dictionary<string, Token> allNames = new Dictionary<string, Token>();
            foreach (var entity in result.Enums)
            {
                if (allNames.TryGetValue(entity.Name.Value, out Token exitName))
                {
                    throw new Exception(string.Format("{0} =>Type name has exited in {1} ", entity.Name, exitName));
                }
                allNames.Add(entity.Name.Value, entity.Name);

                for (int i=0; i<entity.Fields.Count; ++i)
                {
                    EnumField field = entity.Fields[i];
                    for (int j = i + 1; j < entity.Fields.Count; ++j)
                    {
                        if (field.Name.Value == entity.Fields[j].Name.Value)
                        {
                            throw new Exception(string.Format("{0} => Enum field repeated in", entity.Fields[j].Name));
                        }
                    }
                }
            }

            foreach (var entity in result.Structs)
            {
                if (allNames.TryGetValue(entity.Name.Value, out Token exitName))
                {
                    throw new Exception(string.Format("{0} =>Type name has exited in {1} ", entity.Name, exitName));
                }
                allNames.Add(entity.Name.Value, entity.Name);

                HashSet<int> vIndex = new HashSet<int>();

                HashSet<string> includeFiles = new HashSet<string>();
                for (int i = 0; i < entity.Fields.Count; ++i)
                {
                    var field = entity.Fields[i];
                    //索引检查
                    if (field.Index.IntValue <= 0)
                    {
                        throw new Exception(string.Format("{0} => Struct index must be   greater than 0", field.Index));
                    }
                    if (vIndex.Contains(field.Index.IntValue))
                    {
                        throw new Exception(string.Format("{0} => Struct index repeated", field.Index));
                    }
                    vIndex.Add(field.Index.IntValue);

                    //类型检查
                    field.Type.TypeType = ToFieldType(field.Type.Type, result);
                    if (field.Type.TypeType == FieldType.Vector)
                    {
                        FieldType paramType = ToFieldType(field.Type.ExternTypes[0].Name, result);
                        if (paramType == FieldType.Vector || paramType == FieldType.Map)
                            throw new Exception(string.Format("Wrong value type in {0}", field.Type.ExternTypes[0].Name));
                        field.Type.ExternTypes[0].Type = paramType;
                    }
                    else if (field.Type.TypeType == FieldType.Map)
                    {
                        FieldType keyType = ToFieldType(field.Type.ExternTypes[0].Name, result);
                        FieldType valueType = ToFieldType(field.Type.ExternTypes[1].Name, result);
                        if (keyType != FieldType.BaseType || field.Type.ExternTypes[0].Name.Value == "bytes")
                            throw new Exception(string.Format("Wrong key type in {0}", field.Type.ExternTypes[0].Name));

                        if (valueType == FieldType.Vector || valueType == FieldType.Map)
                            throw new Exception(string.Format("Wrong value type in {0}", field.Type.ExternTypes[1].Name));
                        field.Type.ExternTypes[0].Type = keyType;
                        field.Type.ExternTypes[1].Type = valueType;
                    }
                    //记录类型引用
                    if (field.Type.TypeType == FieldType.Struct || field.Type.TypeType == FieldType.Enum)
                    {
                        entity.IncludeCustoType.Add(field.Type.Type.Value);
                    }
                    else if (field.Type.TypeType == FieldType.Vector || field.Type.TypeType == FieldType.Map)
                    {
                        foreach (var ext in field.Type.ExternTypes)
                        {
                            if (ext.Type == FieldType.Struct || ext.Type == FieldType.Enum)
                            {
                                entity.IncludeCustoType.Add(ext.Name.Value);
                            }
                        }
                    }
                    //字段名检查
                    for (int j = i + 1; j < entity.Fields.Count; ++j)
                    {
                        if (field.Name.Value == entity.Fields[j].Name.Value)
                        {
                            throw new Exception(string.Format("{0} => Struct field name repeated", field.Name));
                        }
                    }

                }
            }
        }

        public static FieldType ToFieldType(Token token, ProtoResult result)
        {
            if (ProtoParser.BaseType.Contains(token.Value))
                return FieldType.BaseType;
            if (ProtoParser.VectorType.Contains(token.Value))
                return FieldType.Vector;
            if (ProtoParser.MapType.Contains(token.Value))
                return FieldType.Map;
            if (result.Structs.Exists(obj => obj.Name.Value == token.Value))
                return FieldType.Struct;
            if (result.Enums.Exists(obj => obj.Name.Value == token.Value))
                return FieldType.Struct;

            throw new Exception(string.Format("UnKnown type {0}", token));
        }
    }
}
