using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SystemCommonLibrary.Reflect;

namespace SystemCommonLibrary.Serialization
{
    public static class BitSerializer
    {
        internal static void ApplyFieldsNoSerializable(FieldInfo[] fields, Type type)
        {
            if (fields != null && fields.Length > 0)
            {
                var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                var propertyNames = properties.Select(p => p.Name);

                for (int i = 0; i < fields.Length; i++)
                {
                    var fi = fields[i];
                    var fiName = fi.Name;
                    if (fiName.EndsWith("k__BackingField")
                        && fiName.Contains("<") && fiName.Contains(">"))
                    {
                        var propertyName = fiName.Replace("k__BackingField", string.Empty)
                                                .Replace("<", string.Empty).Replace(">", string.Empty);
                        if (propertyNames.Contains(propertyName))
                        {
                            var property = properties.SingleOrDefault(p => p.Name == propertyName);
                            if (property != null
                                && property.IsDefined(typeof(NoSerializeAttribute)))
                            {
                                fields[i] = null;
                            }
                        }
                    }
                    else
                    {
                        if (fi.IsDefined(typeof(NoSerializeAttribute)))
                        {
                            fields[i] = null;
                        }
                    }
                }
            }
        }

        internal static byte[] SerializeInstanceWithFields(object instance, Type type)
        {
            List<byte> datas = new List<byte>();

            var fs = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (fs != null && fs.Length > 0)
            {
                ApplyFieldsNoSerializable(fs, type);
                foreach (var field in fs)
                {
                    if (field != null)
                    {
                        var pv = field.GetValue(instance);
                        //检查是否被装箱过
                        if (pv != null && (field.FieldType.IsObject() || field.FieldType.IsAbstract))
                        {
                            var data = new List<byte>();
                            //将源类型名先序列化
                            data.AddRange(Serialize(pv.GetType().AssemblyQualifiedName));

                            //再将源类型值序列化
                            data.AddRange(Serialize(pv));

                            datas.AddRange(BitConverter.GetBytes(data.Count).Concat(data));
                        }
                        else
                        {
                            datas.AddRange(Serialize(pv));
                        }
                    }
                }
            }

            return datas.ToArray();
        }

        internal static byte[] SerializeList(IEnumerable list)
        {
            List<byte> datas = new List<byte>();

            var args = list.GetType().GetGenericArguments();
            foreach (var item in list)
            {
                if (item != null && args.Length > 0 && (args[0].IsObject() || args[0].IsAbstract))
                {
                    var data = new List<byte>();
                    data.AddRange(Serialize(item.GetType().AssemblyQualifiedName));
                    data.AddRange(Serialize(item));
                    datas.AddRange(BitConverter.GetBytes(data.Count).Concat(data));
                }
                else
                { 
                    datas.AddRange(Serialize(item));
                }
            }
                
            return datas.ToArray();
        }

        internal static byte[] SerializeDic(IDictionary dict)
        {
            List<byte> datas = new List<byte>();

            var args = dict.GetType().GetGenericArguments();
            foreach (var item in dict) 
            {
                var type = item.GetType();
                var ps = type.GetProperties();

                if (ps.Length == args.Length)
                {
                    for (int j = 0; j < ps.Length; j++)
                    {
                        var pv = ps[j].GetValue(item, null);
                        if (pv != null && (args[j].IsObject() || args[j].IsAbstract))
                        {
                            var data = new List<byte>();
                            data.AddRange(Serialize(pv.GetType().AssemblyQualifiedName));
                            data.AddRange(Serialize(pv));
                            datas.AddRange(BitConverter.GetBytes(data.Count).Concat(data));
                        }
                        else
                        { 
                            datas.AddRange(Serialize(pv));
                        }
                    }
                }
            }

            return datas.ToArray();
        }

        internal static object DeserializeInstanceWithFields(Type type, byte[] datas)
        {
            var instance = SlimTypeInfo.GetOrAddInstance(type).Instance;
            int offset = 0;

            var fs = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            ApplyFieldsNoSerializable(fs, type);

            foreach (var field in fs)
            {
                if (field != null)
                {
                    int fieldDataLength = BitConverter.ToInt32(datas, offset) + 4;
                    var fieldData = new byte[fieldDataLength];
                    Array.Copy(datas, offset, fieldData, 0, fieldDataLength);
                    offset += fieldDataLength;

                    if (fieldDataLength > 4 && (field.FieldType.IsObject() || field.FieldType.IsAbstract))
                    {
                        int lenRealNameType = BitConverter.ToInt32(fieldData, 4) + 4;
                        var dataRealNameType = new byte[lenRealNameType];
                        Array.Copy(fieldData, 4, dataRealNameType, 0, lenRealNameType);
                        Type fieldType = Deserialize<Type>(dataRealNameType);

                        int valueLength = BitConverter.ToInt32(fieldData, lenRealNameType + 4) + 4;
                        var valueData = new byte[valueLength];
                        Array.Copy(fieldData, lenRealNameType + 4, valueData, 0, valueLength);
                        object fieldValue = Deserialize(fieldType, valueData);
                        field.SetValue(instance, fieldValue);
                    }
                    else
                    {
                        object fieldValue = Deserialize(field.FieldType, fieldData);
                        field.SetValue(instance, fieldValue);
                    }
                }
            }

            return instance;
        }

        internal static object DeserializeArray(Type type, byte[] datas)
        {
            var originTypeName = type.AssemblyQualifiedName.Replace("[]", string.Empty);
            Type originType = Type.GetType(originTypeName);
            if (originType == null) return null;

            var listName = typeof(List<object>).AssemblyQualifiedName;
            var objName = typeof(object).AssemblyQualifiedName;
            var listTypeName = listName.Replace(objName, originTypeName);

            var listType = Type.GetType(listTypeName);
            if (listType == null) return null;

            var obj = DeserializeList(listType, datas);

            if (obj == null) return null;

            var toArray = listType.GetMethod("ToArray");
            var list = toArray.Invoke(obj, null);

            return list;
        }

        internal static object DeserializeList(Type type, byte[] datas)
        {
            var info = SlimTypeInfo.GetOrAddInstance(type);
            var instance = info.Instance;
            var itemType = info.ArgTypes[0];
            var offset = 0;

            while (offset < datas.Length)
            {
                int itemLength = BitConverter.ToInt32(datas, offset) + 4;

                if (itemLength > 0)
                {
                    byte[] itemData = new byte[itemLength];
                    Buffer.BlockCopy(datas, offset, itemData, 0, itemLength);
                    offset += itemLength;

                    object item;
                    if (itemType.IsClass && itemType != typeof(string))
                    {
                        int innerOffset = 0;
                        item = Deserialize(itemType, itemData, ref innerOffset);
                    }
                    else
                    {
                        item = Deserialize(itemType, itemData);
                    }

                    if (item != null)
                    {
                        info.MethodInfo.Invoke(instance, new object[] { item });
                    }
                }
            }
            return instance;
        }

        internal static object DeserializeDic(Type type, byte[] datas)
        {
            var tinfo = SlimTypeInfo.GetOrAddInstance(type);
            var instance = tinfo.Instance;

            var offset = 0;
            bool isKey = true;
            object key = null;

            while (offset < datas.Length)
            {
                int itemLength = BitConverter.ToInt32(datas, offset);
                int itemOffset = offset;
                offset += 4;
                if (isKey)
                {
                    key = Deserialize(tinfo.ArgTypes[0], datas, ref itemOffset);
                }
                else
                {
                    tinfo.MethodInfo.Invoke(instance, 
                        new object[] { key, Deserialize(tinfo.ArgTypes[1], datas, ref itemOffset) });
                }
                offset += itemLength;

                isKey = !isKey;
            }

            return instance;
        }

        public static byte[] Serialize(object param)
        {
            List<byte> datas = new List<byte>();
            if (param != null)
            {
                if (param is byte[])
                {
                    datas.AddRange((byte[])param);
                }
                else if (param is string)
                {
                    datas.AddRange(Encoding.UTF8.GetBytes((string)param));
                }
                else if (param is byte)
                {
                    datas.Add((byte)param);
                }
                else if (param is bool)
                {
                    datas.AddRange(BitConverter.GetBytes((bool)param));
                }
                else if (param is short)
                {
                    datas.AddRange(BitConverter.GetBytes((short)param));
                }
                else if (param is int)
                {
                    datas.AddRange(BitConverter.GetBytes((int)param));
                }
                else if (param is long)
                {
                    datas.AddRange(BitConverter.GetBytes((long)param));
                }
                else if (param is float)
                {
                    datas.AddRange(BitConverter.GetBytes((float)param));
                }
                else if (param is double)
                {
                    datas.AddRange(BitConverter.GetBytes((double)param));
                }
                else if (param is decimal)
                {
                    datas.AddRange(Encoding.UTF8.GetBytes(((decimal)param).ToString()));
                }
                else if (param is ushort)
                {
                    datas.AddRange(BitConverter.GetBytes((ushort)param));
                }
                else if (param is uint)
                {
                    datas.AddRange(BitConverter.GetBytes((uint)param));
                }
                else if (param is ulong)
                {
                    datas.AddRange(BitConverter.GetBytes((ulong)param));
                }
                else if (param is DateTime)
                {
                    datas.AddRange(Encoding.UTF8.GetBytes(((DateTime)param).Ticks.ToString()));
                }
                else if (param is Guid)
                {
                    datas.AddRange(Encoding.UTF8.GetBytes(((Guid)param).ToString()));
                }
                else if (param is Type)
                {
                    datas.AddRange(Encoding.UTF8.GetBytes(((Type)param).AssemblyQualifiedName));
                }
                else if (param is Enum)
                {
                    var enumValType = Enum.GetUnderlyingType(param.GetType());

                    if (enumValType == typeof(byte))
                    {
                        datas.AddRange(new byte[] { (byte)param });
                    }
                    else if (enumValType == typeof(short))
                    {
                        datas.AddRange(BitConverter.GetBytes((short)param));
                    }
                    else if (enumValType == typeof(int))
                    {
                        datas.AddRange(BitConverter.GetBytes((int)param));
                    }
                    else
                    {
                        datas.AddRange(BitConverter.GetBytes((long)param));
                    }
                }
                else
                {
                    var type = param.GetType();
                    if (type.IsClass || type.IsStruct())
                    {
                        if (type.IsGenericType || type.IsArray)
                        {
                            if (SlimTypeInfo.DicTypes.Contains(type.Name))
                                datas.AddRange(SerializeDic((IDictionary)param));
                            else if (SlimTypeInfo.ListTypes.Contains(type.Name) || type.IsArray)
                                datas.AddRange(SerializeList((IEnumerable)param));
                            else
                                throw new NotImplementedException($"{type.Name} Not Implemented");
                        }
                        else
                        {
                            datas.AddRange(SerializeInstanceWithFields(param, type));
                        }
                    }
                }
            }
            
            return BitConverter.GetBytes(datas.Count).Concat(datas).ToArray();
        }

        public static T Deserialize<T>(byte[] datas)
        {
            int offset = 0;
            return (T)Deserialize(typeof(T), datas, ref offset);
        }

        public static object Deserialize(Type type, byte[] datas)
        {
            int offset = 0;
            return Deserialize(type, datas, ref offset);
        }

        public static object Deserialize(Type type, byte[] datas, ref int offset)
        {
            dynamic obj = null;
            var len = 0;
            len = BitConverter.ToInt32(datas, offset);
            byte[] data = null;
            offset += 4;
            if (len > 0)
            {
                data = new byte[len];
                Buffer.BlockCopy(datas, offset, data, 0, len);
                offset += len;

                //先检查是否被装箱过
                if (type.IsObject())
                {
                    var lenRealType = BitConverter.ToInt32(data, 0) + 4;
                    byte[] dataTypeName = new byte[lenRealType];
                    Buffer.BlockCopy(data, 0, dataTypeName, 0, lenRealType);
                    string realTypeName = Deserialize<string>(dataTypeName);

                    //将获得的类型赋值后处理真正的数据
                    type = Type.GetType(realTypeName);
                    var srcData = data;
                    var lenData = BitConverter.ToInt32(data, lenRealType);
                    data = new byte[lenData];
                    Buffer.BlockCopy(srcData, lenRealType + 4, data, 0, lenData);
                }
                
                if (type == typeof(string))
                {
                    obj = Encoding.UTF8.GetString(data);
                }
                else if (type == typeof(byte))
                {
                    obj = data[0];
                }
                else if (type == typeof(bool))
                {
                    obj = (BitConverter.ToBoolean(data, 0));
                }
                else if (type == typeof(short))
                {
                    obj = (BitConverter.ToInt16(data, 0));
                }
                else if (type == typeof(int))
                {
                    obj = (BitConverter.ToInt32(data, 0));
                }
                else if (type == typeof(long))
                {
                    obj = (BitConverter.ToInt64(data, 0));
                }
                else if (type == typeof(float))
                {
                    obj = (BitConverter.ToSingle(data, 0));
                }
                else if (type == typeof(double))
                {
                    obj = (BitConverter.ToDouble(data, 0));
                }
                else if (type == typeof(decimal))
                {
                    var sData = Encoding.UTF8.GetString(data);
                    obj = decimal.Parse(sData); ;
                }
                else if (type == typeof(ushort))
                {
                    obj = (BitConverter.ToUInt16(data, 0));
                }
                else if (type == typeof(uint))
                {
                    obj = (BitConverter.ToUInt32(data, 0));
                }
                else if (type == typeof(ulong))
                {
                    obj = (BitConverter.ToUInt64(data, 0));
                }
                else if (type == typeof(DateTime))
                {
                    var sData = Encoding.UTF8.GetString(data);
                    var ticks = long.Parse(sData);
                    obj = (new DateTime(ticks));
                }
                else if (type == typeof(Guid))
                {
                    var sData = Encoding.UTF8.GetString(data);
                    obj = Guid.Parse(sData);
                }
                else if (type == typeof(Type) || type.IsRuntimeType())
                {
                    var sData = Encoding.UTF8.GetString(data);
                    obj = Type.GetType(sData);
                }
                else if (type.BaseType == typeof(Enum))
                {
                    var numType = Enum.GetUnderlyingType(type);

                    if (numType == typeof(byte))
                    {
                        obj = Enum.ToObject(type, data[0]);
                    }
                    else if (numType == typeof(short))
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt16(data, 0));
                    }
                    else if (numType == typeof(int))
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt32(data, 0));
                    }
                    else
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt64(data, 0));
                    }
                }
                else if (type == typeof(byte[]))
                {
                    obj = data;
                }
                else if (type.IsGenericType || type.IsArray)
                {
                    if (SlimTypeInfo.ListTypes.Contains(type.Name))
                    {
                        obj = DeserializeList(type, data);
                    }
                    else if (SlimTypeInfo.DicTypes.Contains(type.Name))
                    {
                        obj = DeserializeDic(type, data);
                    }
                    else if (type.IsArray)
                    {
                        obj = DeserializeArray(type, data);
                    }
                    else
                    {
                        throw new NotImplementedException("未定义的类型：" + type.ToString());
                    }
                }
                else if (type.IsClass || type.IsStruct())
                {
                    obj = DeserializeInstanceWithFields(type, data);
                }
                else
                {
                    throw new NotImplementedException("未定义的类型：" + type.ToString());
                }

            }
            else
            {
                if (type.IsGenericType)
                {
                    var tInfo = SlimTypeInfo.GetOrAddInstance(type);
                    obj = tInfo.Instance;
                }
                else
                {
                    obj = null;
                }
            }

            return obj;
        }

    }
}
