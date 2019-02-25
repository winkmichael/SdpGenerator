using System;
using System.Collections.Generic;

namespace Parser
{
    public static class ProtoParser
    {
        public static List<string> BaseType = new List<string>(){
            "int", "uint" , "long", "ulong", "bool",
            "float", "double" , "string", "bytes"
        };

        public static List<string> VectorType = new List<string>() { "List"};

        public static List<string> MapType = new List<string>() { "Map"};

        private static RegexMatch IntMatch = new RegexMatch("^[+-]?([0-9]{1,})$");
        private static RegexMatch UIntMatch = new RegexMatch("^[+]?([0-9]{1,})$");
        private static RegexMatch VarNameMatch = new RegexMatch("^[a-zA-Z_]([a-zA-Z0-9_]{1,})$");
        private static RegexMatch TypeNameMatch = new RegexMatch("^[a-zA-Z_]([a-zA-Z0-9_]{1,})([.][a-zA-Z_]([a-zA-Z0-9_]{1,}))?$");
        private static SymbolMatch CommaMatch = new SymbolMatch(',');
        private static SymbolMatch SemicolonMatch = new SymbolMatch(';');
        private static SymbolMatch EquallSignMatch = new SymbolMatch('=');
        private static SymbolMatch OpeningbraceMatch = new SymbolMatch('{');
        private static SymbolMatch CloseingbraceMatch = new SymbolMatch('}');
        private static SymbolMatch OpeningAngleMatch = new SymbolMatch('<');
        private static SymbolMatch CloseingAngleMatch = new SymbolMatch('>');
        private static SymbolMatch OpeningBracketMatch = new SymbolMatch('(');
        private static SymbolMatch CloseingBracketMatch = new SymbolMatch(')');
        private static KeyWordMatch EnumMatch = new KeyWordMatch("enum");
        private static KeyWordMatch StructMatch = new KeyWordMatch("struct");
        private static KeyWordMatch MessageMatch = new KeyWordMatch("message");
//         private static KeyWordMatch InterfaceMatch = new KeyWordMatch("interface");
//         private static KeyWordMatch ServiceMatch = new KeyWordMatch("service");
//         private static KeyWordMatch ImportMatch = new KeyWordMatch("import");
//         private static KeyWordMatch RpcMatch = new KeyWordMatch("rpc");
//         private static KeyWordMatch NamespaceMatch = new KeyWordMatch("namespace");
        private static StringMatch NormalStringMatch = new StringMatch();
        private static KeyWordsMatch BaseTypeMatch = new KeyWordsMatch(BaseType);
        private static KeyWordsMatch VectorMatch = new KeyWordsMatch(VectorType);
        private static KeyWordsMatch MapMatch = new KeyWordsMatch(MapType);

        public static string ItToError(TokenIterator it, string extertMsg = "")
        {
            return "Parse faile in file : " + it.Value.FileName + " in line : " + it.Value.LineNum + " near : " + it.Value.Value + extertMsg;
        }

        public static void ThrowError(TokenIterator it, string extertMsg = "")
        {
            throw new Exception(ItToError(it, extertMsg));
        }

        public static EnumField ParseEnumField(TokenIterator it)
        {
            EnumField field = new EnumField();
            do
            {
                if (VarNameMatch.Match(it, ref field.Name))
                {
                    if (EquallSignMatch.Match(it))
                        if (IntMatch.Match(it, ref field.Value) && CommaMatch.Match(it))
                            break;
                    if (CommaMatch.Match(it))
                        break;
                    if (CloseingbraceMatch.Match(it))
                    {
                        --it;
                        break;
                    }
                }
                ThrowError(it);
            } while (false);
            return field;
        }

        public static EnumEntity ParseEnumEntity(TokenIterator it)
        {
            if (EnumMatch.Match(it))
            {
                EnumEntity entity = new EnumEntity();
                if (VarNameMatch.Match(it, ref entity.Name) && OpeningbraceMatch.Match(it))
                {
                    while (true)
                    {
                        if (CloseingbraceMatch.Match(it))
                            break;
                        var field = ParseEnumField(it);
                        entity.Fields.Add(field);
                    }
                    return entity;
                }
                throw new Exception(ItToError(it));
            }
            return null;
        }

        public static TypeEntity ParseTypeEntity(TokenIterator it)
        {
            do
            {
                TypeEntity entity = new TypeEntity();
                if (VectorMatch.Match(it, ref entity.Type))
                {
                    Token token = null;
                    if (!OpeningAngleMatch.Match(it) || !TypeNameMatch.Match(it, ref token) || !CloseingAngleMatch.Match(it))
                        break;
                    //entity.Params.Add(token);
                    entity.ExternTypes.Add(new ExternTypeEntity() { Name = token });
                }
                else if (MapMatch.Match(it, ref entity.Type))
                {
                    Token token1 = null;
                    Token token2 = null;
                    if (!OpeningAngleMatch.Match(it) || !BaseTypeMatch.Match(it, ref token1) || !CommaMatch.Match(it) || !TypeNameMatch.Match(it, ref token2) || !CloseingAngleMatch.Match(it))
                        break;
                    //entity.Params.Add(token1);
                    //entity.Params.Add(token2);
                    entity.ExternTypes.Add(new ExternTypeEntity() { Name = token1 });
                    entity.ExternTypes.Add(new ExternTypeEntity() { Name = token2 });
                }
                else if (!BaseTypeMatch.Match(it, ref entity.Type) && !TypeNameMatch.Match(it, ref entity.Type))
                {
                    break;
                }
                return entity;
            } while (false);
            throw new Exception(ItToError(it));
        }

        public static StructField ParseStructField(TokenIterator it)
        {
            StructField field = new StructField();
            if (IntMatch.Match(it, ref field.Index))
            {
                do
                {
                    var typeEntity = ParseTypeEntity(it);
                    if (typeEntity == null)
                        break;
                    field.Type = typeEntity;
                    if (!VarNameMatch.Match(it, ref field.Name))
                        break;
                    if (!SemicolonMatch.Match(it))
                        break;

                    return field;
                } while (false);
                ThrowError(it);
            }
            return null;
        }

        public static StructEntity ParseStructEntity(TokenIterator it)
        {
            if (StructMatch.Match(it))
            {
                StructEntity entity = new StructEntity();
                if (VarNameMatch.Match(it, ref entity.Name))
                {
                    if (OpeningbraceMatch.Match(it))
                    {
                        while (true)
                        {
                            if (CloseingbraceMatch.Match(it))
                                break;
                            var filed = ParseStructField(it);
                            if (filed == null)
                                ThrowError(it);
                            entity.Fields.Add(filed);
                        }
                        return entity;
                    }
                    ThrowError(it);
                }
            }
            return null;
        }
        
        public static void ParseFile(string strFileName, ProtoResult result)
        {
            Tokenizer tokenizer = new Tokenizer();
            if (tokenizer.Load(strFileName))
            {
                TokenIterator it = tokenizer.Iterator();
                while (true)
                {
                    var enumEntity = ParseEnumEntity(it);
                    if (enumEntity != null)
                    {
                        result.Enums.Add(enumEntity);
                        continue;
                    }

                    var stEntity = ParseStructEntity(it);
                    if (stEntity != null)
                    {
                        result.Structs.Add(stEntity);
                        continue;
                    }
                    break;
                }
            }
        }
        

    }
}
