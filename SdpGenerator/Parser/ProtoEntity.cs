using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public enum FieldType
    {
        BaseType,
        Enum,
        Struct,
        Vector,
        Map,
    }

    public class EnumField
    {
        public Token Name;
        public Token Value;
    }

    public class EnumEntity
    {
        public Token Name;
        public List<EnumField> Fields = new List<EnumField>();
    }

    public class ExternTypeEntity
    {
        public Token Name;
        public FieldType Type;
    }

    public class TypeEntity
    {
        public Token Type;
        public FieldType TypeType;
        //泛型扩展类型
        public List<ExternTypeEntity> ExternTypes = new List<ExternTypeEntity>();
    }

    public class StructField
    {
        public Token Name;
        public Token Index;
        public TypeEntity Type;
    }

    public class StructEntity
    {
        public Token Name;
        public List<StructField> Fields = new List<StructField>();
        public bool IsMessage = false;

        public HashSet<string> IncludeCustoType = new HashSet<string>();
    }

    public class ProtoResult
    {
        public List<StructEntity> Structs = new List<StructEntity>();
        public List<EnumEntity> Enums = new List<EnumEntity>();
    }

}
