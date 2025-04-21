using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace SpellWork.Parser
{
    public class DBFileNameAttribute : Attribute
    {
        public string Filename { get; }

        public DBFileNameAttribute(string fileName)
        {
            Filename = fileName;
        }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RelationFieldAttribute : Attribute
    {
        public RelationFieldAttribute()
        {

        }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class IndexAttribute : Attribute
    {
        public IndexAttribute()
        {

        }
    }
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ArraySizeAttribute : Attribute
    {
        /// <summary>
        /// The size of the member if that member is an array.
        /// </summary>
        public int SizeConst { get; set; }

        public ArraySizeAttribute(int sizeConst)
        {
            SizeConst = sizeConst;
        }
    }
    public static class Parser
    {
        public static int[] GetClassSize<T>(bool skipArray = false, bool HasIndexTable = true)
        {
            List<int> columnSizes = new List<int>();

            Type type = typeof(T);

            //Console.WriteLine("Definition: " + type);

            foreach (var f in type.GetFields().Where(f => f.IsPublic))
            {
                if (skipArray && ((HasIndexTable && f.GetCustomAttribute(typeof(IndexAttribute)) != null) || f.GetCustomAttribute(typeof(RelationFieldAttribute)) != null))
                    continue;
                if (f.FieldType != typeof(string))
                {
                    if (f.FieldType.IsArray)
                    {
                        int SizeConst = 1;
                        object[] atts = f.GetCustomAttributes(typeof(ArraySizeAttribute), false);
                        if (atts.Length > 0)
                            SizeConst = (atts[0] as ArraySizeAttribute).SizeConst;

                        if (!skipArray)
                        {
                            //Console.WriteLine(String.Format("Name: {0} Type: {1} Size: {2}x{3}", f.Name, f.FieldType, SizeConst, Marshal.SizeOf(f.FieldType.GetElementType())));
                            for (int i = 0; i < SizeConst; i++)
                                columnSizes.Add(Marshal.SizeOf(f.FieldType.GetElementType()));
                        }
                        else
                        {
                            columnSizes.Add(Marshal.SizeOf(f.FieldType.GetElementType()));
                        }
                    }
                    else
                    {
                        //Console.WriteLine(String.Format("Name: {0} Type: {1} Size: {2}", f.Name, f.FieldType, Marshal.SizeOf(f.FieldType)));
                        columnSizes.Add(Marshal.SizeOf(f.FieldType));
                    }
                }
                else
                {
                    //Console.WriteLine(String.Format("Name: {0} Type: {1} Size: {2}", f.Name, f.FieldType, 4));
                    columnSizes.Add(4);
                }
            }
            //columnSizes.RemoveAt(columnSizes.Count - 1);
            return columnSizes.ToArray<int>();
        }
        public static TypeCode[] GetColumnTypes<T>()
        {
            List<TypeCode> columnTypes = new List<TypeCode>();

            Type type = typeof(T);

            foreach (var f in type.GetFields().Where(f => f.IsPublic))
            {
                if (f.FieldType != typeof(string))
                {
                    if (f.FieldType.IsArray)
                    {
                        int SizeConst = 1;
                        object[] atts = f.GetCustomAttributes(typeof(ArraySizeAttribute), false);
                        if (atts.Length > 0)
                            SizeConst = (atts[0] as ArraySizeAttribute).SizeConst;


                        //Console.WriteLine(String.Format("Name: {0} Type: {1} Size: {2}x{3}", f.Name, f.FieldType, SizeConst, Marshal.SizeOf(f.FieldType.GetElementType())));
                        for (int i = 0; i < SizeConst; i++)
                            columnTypes.Add(Type.GetTypeCode(f.FieldType.GetElementType()));
                    }
                    else
                    {
                        //Console.WriteLine(String.Format("Name: {0} Type: {1} Size: {2}", f.Name, f.FieldType, Marshal.SizeOf(f.FieldType)));
                        columnTypes.Add(Type.GetTypeCode(f.FieldType));
                    }
                }
                else
                {
                    //Console.WriteLine(String.Format("Name: {0} Type: {1} Size: {2}", f.Name, f.FieldType, Marshal.SizeOf(f.FieldType)));
                    columnTypes.Add(Type.GetTypeCode(f.FieldType));
                }
            }
            return columnTypes.ToArray<TypeCode>();
        }
        public static readonly short[] CommonDataBits = new short[] { 0, 16, 24, 0, 0 }; //String Int16 Byte Float Int32
        
        
        public static string BuildText(int build)
        {
            var first = Builds.First();
            var last = Builds.Last();
            string lastText = $"{first.Value} ({build})";

            if (build <= first.Key)
                return lastText; //First build
            else if (build >= last.Key)
                return $"{last.Value} ({build})"; //Last build

            foreach (var b in Builds)
            {
                if (build < b.Key)
                    return lastText;

                lastText = $"{b.Value} ({build})";
            }

            return lastText;
        }
        private static readonly SortedDictionary<int, string> Builds = new SortedDictionary<int, string>()
        {
            {3368,"Alpha 0.5.3"},
            {3494,"Alpha 0.5.5"},
            {3694,"Beta 0.7.0"},
            {3702,"Beta 0.7.1"},
            {3712,"Beta 0.7.6"},
            {3734,"Beta 0.8.0"},
            {3807,"Beta 0.9.0"},
            {3810,"Beta 0.9.1"},
            {3892,"Beta 0.10.0"},
            {3925,"Beta 0.11.0"},
            {3988,"Beta 0.12.0"},
            {3980,"Classic 1.0.0"},
            {3989,"Classic 1.0.1"},
            {4044,"Classic 1.1.0"},
            {4062,"Classic 1.1.1"},
            {4125,"Classic 1.1.2"},
            {4147,"Classic 1.2.0"},
            {4150,"Classic 1.2.1"},
            {4196,"Classic 1.2.2"},
            {4211,"Classic 1.2.3"},
            {4222,"Classic 1.2.4"},
            {4284,"Classic 1.3.0"},
            {4297,"Classic 1.3.1"},
            {4341,"Classic 1.4.0"},
            {4364,"Classic 1.4.1"},
            {4375,"Classic 1.4.2"},
            {4442,"Classic 1.5.0"},
            {4499,"Classic 1.5.1"},
            {4500,"Classic 1.6.0"},
            {4544,"Classic 1.6.1"},
            {4671,"Classic 1.7.0"},
            {4695,"Classic 1.7.1"},
            {4735,"Classic 1.8.0"},
            {4769,"Classic 1.8.1"},
            {4784,"Classic 1.8.2"},
            {4807,"Classic 1.8.3"},
            {4878,"Classic 1.8.4"},
            {4937,"Classic 1.9.0"},
            {4983,"Classic 1.9.1"},
            {4996,"Classic 1.9.2"},
            {5059,"Classic 1.9.3"},
            {5086,"Classic 1.9.4"},
            {5195,"Classic 1.10.0"},
            {5230,"Classic 1.10.1"},
            {5302,"Classic 1.10.2"},
            {5428,"Classic 1.11.0"},
            {5464,"Classic 1.11.2"},
            {5595,"Classic 1.12.0"},
            {5875,"Classic 1.12.1"},
            {6005,"Classic 1.12.2"},
            {6080,"TBC 2.0.0"},
            {6180,"TBC 2.0.1"},
            {6299,"TBC 2.0.3"},
            {6314,"TBC 2.0.4"},
            {6320,"TBC 2.0.5"},
            {6337,"TBC 2.0.6"},
            {6383,"TBC 2.0.7"},
            {6403,"TBC 2.0.8"},
            {6448,"TBC 2.0.10"},
            {6546,"TBC 2.0.12"},
            {6729,"TBC 2.1.0"},
            {6739,"TBC 2.1.1"},
            {6803,"TBC 2.1.2"},
            {6898,"TBC 2.1.3"},
            {7272,"TBC 2.2.0"},
            {7318,"TBC 2.2.2"},
            {7359,"TBC 2.2.3"},
            {7561,"TBC 2.3.0"},
            {7741,"TBC 2.3.2"},
            {7799,"TBC 2.3.3"},
            {8089,"TBC 2.4.0"},
            {8125,"TBC 2.4.1"},
            {8278,"TBC 2.4.2"},
            {8606,"TBC 2.4.3"},
            {8820,"WotLK 3.0.1"},
            {9061,"WotLK 3.0.2"},
            {9183,"WotLK 3.0.3"},
            {9506,"WotLK 3.0.8"},
            {9551,"WotLK 3.0.9"},
            {9757,"WotLK 3.1.0"},
            {9835,"WotLK 3.1.1"},
            {9889,"WotLK 3.1.2"},
            {9947,"WotLK 3.1.3"},
            {10314,"WotLK 3.2.0"},
            {10505,"WotLK 3.2.2"},
            {11159,"WotLK 3.3.0"},
            {11599,"WotLK 3.3.2"},
            {11723,"WotLK 3.3.3"},
            {12340,"WotLK 3.3.5"},
            {12759,"Cata 4.0.0"},
            {13205,"Cata 4.0.1"},
            {13329,"Cata 4.0.3"},
            {13623,"Cata 4.0.6"},
            {14007,"Cata 4.1.0"},
            {14480,"Cata 4.2.0"},
            {14545,"Cata 4.2.2"},
            {15050,"Cata 4.3.0"},
            {15211,"Cata 4.3.2"},
            {15354,"Cata 4.3.3"},
            {15595,"Cata 4.3.4"},
            {15851,"MoP 5.0.1"},
            {15882,"MoP 5.0.3"},
            {16016,"MoP 5.0.4"},
            {16135,"MoP 5.0.5"},
            {16357,"MoP 5.1.0"},
            {16826,"MoP 5.2.0"},
            {17128,"MoP 5.3.0"},
            {17399,"MoP 5.4.0"},
            {17538,"MoP 5.4.1"},
            {17688,"MoP 5.4.2"},
            {18019,"MoP 5.4.7"},
            {18414,"MoP 5.4.8"},
            {18761,"WoD 6.0.1"},
            {19041,"WoD 6.0.2"},
            {19342,"WoD 6.0.3"},
            {19711,"WoD 6.1.0"},
            {19865,"WoD 6.1.2"},
            {20338,"WoD 6.2.0"},
            {20363,"WoD 6.2.1"},
            {20574,"WoD 6.2.2"},
            {20886,"WoD 6.2.3"},
            {21742,"WoD 6.2.4"},
            {21953,"Legion 7.0.3"},
            {22578,"Legion 7.1.0"},
            {23360,"Legion 7.1.5"},
            {23835,"Legion 7.2.0"},
            {24461,"Legion 7.2.5"},
            {24492,"Legion 7.3.0"},
            {25326,"Legion 7.3.2"},
            {25600,"Legion 7.3.5"},
            {25902,"BfA 8.0.1"}
        };
        public static bool IsBuild(int build, Expansion expansion)
        {
            switch (expansion)
            {
                case Expansion.Alpha:
                    return build <= (int)ExpansionFinalBuild.Alpha;
                case Expansion.Beta:
                    return build > (int)ExpansionFinalBuild.Alpha && build <= (int)ExpansionFinalBuild.Beta;
                case Expansion.Classic:
                    return build > (int)ExpansionFinalBuild.Beta && build <= (int)ExpansionFinalBuild.Classic;
                case Expansion.TBC:
                    return build > (int)ExpansionFinalBuild.Classic && build <= (int)ExpansionFinalBuild.TBC;
                case Expansion.WotLK:
                    return build > (int)ExpansionFinalBuild.TBC && build <= (int)ExpansionFinalBuild.WotLK;
                case Expansion.Cata:
                    return build > (int)ExpansionFinalBuild.WotLK && build <= (int)ExpansionFinalBuild.Cata;
                case Expansion.MoP:
                    return build > (int)ExpansionFinalBuild.Cata && build <= (int)ExpansionFinalBuild.MoP;
                case Expansion.WoD:
                    return build > (int)ExpansionFinalBuild.MoP && build <= (int)ExpansionFinalBuild.WoD;
                case Expansion.Legion:
                    return build > (int)ExpansionFinalBuild.WoD;
            }

            return false;
        }
    }
    public enum TextWowEnum
    {
        enUS,
        enGB,
        koKR,
        frFR,
        deDE,
        enCN,
        zhCN,
        enTW,
        zhTW,
        esES,
        esMX,
        ruRU,
        ptPT,
        ptBR,
        itIT,
        Unk,
    }
    [Flags]
    public enum HeaderFlags : short
    {
        None = 0x0,
        OffsetMap = 0x1,
        SecondIndex = 0x2,
        IndexMap = 0x4,
        Unknown = 0x8,
        Compressed = 0x10,
    }
    public enum CompressionType
    {
        None = 0,
        Immediate = 1,
        Sparse = 2,
        Pallet = 3,
        PalletArray = 4,
        SignedImmediate = 5
    }

    public enum Expansion
    {
        Alpha,
        Beta,
        Classic,
        TBC,
        WotLK,
        Cata,
        MoP,
        WoD,
        Legion,
        BfA
    }

    public enum ExpansionFinalBuild
    {
        Alpha = 3494,
        Beta = 3988,
        Classic = 6005,
        TBC = 8606,
        WotLK = 12340,
        Cata = 15595,
        MoP = 18414,
        WoD = 21742,
        Legion = 25901
    }
    [Serializable]
    public class Table
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Build { get; set; }
        [XmlElement("Field")]
        public List<Field> Fields { get; set; }
        [XmlIgnore]
        public Field Key { get; private set; }
        [XmlIgnore]
        public bool Changed { get; set; } = false;
        [XmlIgnore]
        public string BuildText { get; private set; }

        public void Load()
        {
            Key = Fields.FirstOrDefault(x => x.IsIndex);
            BuildText = Parser.BuildText(Build);
        }
    }

    [Serializable]
    public class Field
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute, DefaultValue(1)]
        public int ArraySize { get; set; } = 1;
        [XmlAttribute, DefaultValue(false)]
        public bool IsIndex { get; set; } = false;
        [XmlAttribute, DefaultValue(false)]
        public bool AutoGenerate { get; set; } = false;
        [XmlAttribute, DefaultValue("")]
        public string DefaultValue { get; set; } = "";
        [XmlAttribute, DefaultValue("")]
        public string ColumnNames { get; set; } = "";
        [XmlIgnore]
        public string InternalName { get; set; }
    }
}
