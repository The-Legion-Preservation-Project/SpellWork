using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using SpellWork.Extensions;
using System.Reflection;

namespace SpellWork.Parser
{
    public class DBReader<T> : Dictionary<int, T> where T : class, new()
    {
        T definition { get; set; }
        public DBReader(string dbcPath, string locale)
        {
            definition = new T();

            DBFileNameAttribute nameAttr = (DBFileNameAttribute)definition.GetType().GetCustomAttribute(typeof(DBFileNameAttribute));
            if (nameAttr == null)
                throw new Exception("Missing DBFileName Attribute at " + definition.GetType());
            FileName = nameAttr.Filename + ".db2";
            Read(dbcPath, locale);
        }
        public string ErrorMessage { get; set; }

		private List<Tuple<int, short>> OffsetMap = new List<Tuple<int, short>>();
		private string FileName;

		#region Read Methods
		private DBHeader ExtractHeader(BinaryReader dbReader)
		{
			DBHeader header = null;
			string signature = dbReader.ReadString(4);

			if (string.IsNullOrWhiteSpace(signature))
				return null;

			if (signature[0] != 'W')
				signature = signature.Reverse();

			switch (signature)
			{
				case "WDB5":
					header = new WDB5();
					break;
				case "WDB6":
					header = new WDB6();
					break;
				case "WDC1":
					header = new WDC1();
					break;
			}

			header?.ReadHeader(ref dbReader, signature);
			return header;
		}

		public void Read(MemoryStream stream, string dbFile)
		{

			FileName = dbFile;
			stream.Position = 0;

			using (var dbReader = new BinaryReader(stream, Encoding.UTF8))
			{
				DBHeader header = ExtractHeader(dbReader);
				long pos = dbReader.BaseStream.Position;

				//No header - must be invalid
				if (header == null)
					throw new Exception("Unknown file type.");

				if (header.CheckRecordSize && header.RecordSize == 0)
					throw new Exception("File contains no records.");
				if (header.CheckRecordCount && header.RecordCount == 0)
					throw new Exception("File contains no records.");

				//DBEntry entry = new DBEntry(header, dbFile);
				//if (header.CheckTableStructure && entry.TableStructure == null)
				//	throw new Exception("Definition missing.");

				if (header is WDC1 wdc1)
				{
					Dictionary<int, string> StringTable = wdc1.ReadStringTable(dbReader);
					wdc1.LoadDefinitionSizes<T>();

					//Read the data
					using (MemoryStream ms = new MemoryStream(header.ReadData(dbReader, pos)))
					using (BinaryReader dataReader = new BinaryReader(ms, Encoding.UTF8))
					{
						//wdc1.AddRelationshipColumn(entry);
						//wdc1.SetColumnMinMaxValues(entry);
						ReadIntoTable(header, dataReader, StringTable);
					}

					stream.Dispose();
					//return entry;
				}
				else
				{
					stream.Dispose();
					throw new Exception($"Invalid filetype.");
				}
			}
		}

		public void Read(string dbcPath, string locale)
        {
            var fileName = Path.Combine(dbcPath, locale, FileName);
			Read(new MemoryStream(File.ReadAllBytes(fileName)), FileName);
		}

        public FieldStructureEntry[] GetBits(DBHeader Header)
        {
            //if (!Header.IsTypeOf<WDB5>())
            //	return new FieldStructureEntry[Data.Columns.Count];

            List<FieldStructureEntry> bits = new List<FieldStructureEntry>();
            if (Header is WDC1 header)
            {
                var fields = header.ColumnMeta;
                for (int i = 0; i < fields.Count; i++)
                {
                    short bitcount = (short)(Header.FieldStructure[i].BitCount == 64 ? Header.FieldStructure[i].BitCount : 0); // force bitcounts
                    for (int x = 0; x < fields[i].ArraySize; x++)
                        bits.Add(new FieldStructureEntry(bitcount, 0));
                }
            }

            return bits.ToArray();
        }
        public void ReadIntoTable(DBHeader Header, BinaryReader dbReader, Dictionary<int, string> StringTable)
		{
			if (Header.RecordCount == 0)
				return;

            TypeCode[] columnTypes = Parser.GetColumnTypes<T>(); //entry.Data.Columns.Cast<DataColumn>().Select(x => Type.GetTypeCode(x.DataType)).ToArray();

			FieldStructureEntry[] bits = GetBits(Header);

            int recordcount = Math.Max(Header.OffsetLengths.Length, (int)Header.RecordCount);

			uint recordsize = Header.RecordSize + (uint)(Header.HasIndexTable ? 4 : 0);
			if (Header.InternalRecordSize > 0)
				recordsize = Header.InternalRecordSize;

			//entry.Data.BeginLoadData();

			for (uint i = 0; i < recordcount; i++)
			{
				//Offset map has variable record lengths
				if (/*entry.Header.IsTypeOf<HTFX>() || */Header.HasOffsetTable)
					recordsize = (uint)Header.OffsetLengths[i];

				//Store start position
				long offset = dbReader.BaseStream.Position;

                T temp = new T();
                FieldInfo[] fields = temp.GetType().GetFields();

                foreach (FieldInfo field in fields)
                {
                    int SizeConst = 1;
                    object[] atts = field.GetCustomAttributes(typeof(ArraySizeAttribute), false);
                    if (atts.Length > 0)
                    {
                        SizeConst = (atts[0] as ArraySizeAttribute).SizeConst;
                        Array arr = Array.CreateInstance(field.FieldType.GetElementType(), SizeConst);
                        field.SetValue(temp, arr);
                    }
                }
                fields = temp.GetType().GetFields();

                int j = 0;
                int arrayCount = 0;
                int arraySize = 0;
                for (int x = 0; x < fields.Length; ++x)
                {
                    FieldInfo field = fields[x];
                    //Console.WriteLine("field: " + field.FieldType);
                    if (arrayCount >= arraySize)
                    {
                        arrayCount = 0;
                        arraySize = 0;
                    }
                    object[] att = field.GetCustomAttributes(typeof(ArraySizeAttribute), false);
                    if (att.Length > 0)
                    {
                        arraySize = (att[0] as ArraySizeAttribute).SizeConst;
                        if (arrayCount < arraySize - 1)
                            --x;
                    }
                    switch (columnTypes[j])
                    {
                        case TypeCode.Boolean:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadBoolean(), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadBoolean());
                            break;
                        case TypeCode.SByte:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadSByte(), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadSByte());
                            break;
                        case TypeCode.Byte:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadByte(), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadByte());
                            break;
                        case TypeCode.Int16:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadInt16(), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadInt16());
                            break;
                        case TypeCode.UInt16:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadUInt16(), arrayCount);
                            }
                            else
                            {
                                field.SetValue(temp, dbReader.ReadUInt16());
                                if (field.GetCustomAttribute(typeof(RelationFieldAttribute)) != null)
                                    dbReader.ReadUInt16(); // read padding
                            }
                            break;
                        case TypeCode.Int32:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadInt32(bits[j]), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadInt32(bits[j]));
                            break;
                        case TypeCode.UInt32:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadUInt32(bits[j]), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadUInt32(bits[j]));
                            break;
                        case TypeCode.Int64:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadInt64(bits[j]), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadInt64(bits[j]));
                            break;
                        case TypeCode.UInt64:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadUInt64(bits[j]), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadUInt64(bits[j]));
                            break;
                        case TypeCode.Single:
                            if (field.FieldType.IsArray)
                            {
                                object fieldValue = field.GetValue(temp);
                                ((Array)fieldValue).SetValue(dbReader.ReadSingle(), arrayCount);
                            }
                            else
                                field.SetValue(temp, dbReader.ReadSingle());
                            break;
                        case TypeCode.String:
                            if (Header.HasOffsetTable)
                            {
                                field.SetValue(temp, dbReader.ReadStringNull());
                            }
                            else
                            {
                                int stindex = Header.GetStringOffset(dbReader, j, i);
                                if (StringTable.ContainsKey(stindex))
                                {
                                    field.SetValue(temp, StringTable[stindex]);
                                }
                                else
                                {
                                    field.SetValue(temp, "String not found");
                                    ErrorMessage = "Strings not found in string table";
                                }
                            }
                            break;
                        default:
                            throw new Exception($"Unknown field type at column {i}.");
                    }

                    ++arrayCount;
                    ++j;
                }
                // add to storage by ID
                foreach (FieldInfo field in fields)
                {
                    if (field.GetCustomAttribute(typeof(IndexAttribute)) != null)
                    {
                        Add(field.GetValue(temp).ToInt32(), temp);
                        break;
                    }
                }


                //Scrub to the end of the record
                if (dbReader.BaseStream.Position - offset < recordsize)
					dbReader.BaseStream.Position += (recordsize - (dbReader.BaseStream.Position - offset));
				else if (dbReader.BaseStream.Position - offset > recordsize)
					throw new Exception("Definition exceeds record size");
                //if (dbReader.BaseStream.Position != recordsize)
                //    throw new Exception("Structure not fully read for " + temp.GetType() + ". Read position " + (dbReader.BaseStream.Position) + " expected " + recordsize);
            }

			Header.Clear();
			//entry.Data.EndLoadData();
		}
		#endregion


	}
}
