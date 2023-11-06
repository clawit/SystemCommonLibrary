/*--------------------------------------------------------------------------
* BitSerializer
* ver 1.1.0.0 (Nov. 7th, 2023) modified by KevinShen
* ver 1.0.1.0 (Nov. 25th, 2019) modified by KevinShen
* ver 1.0.0.0 (May. 16th, 2018)
*
* created by yswenli <wenguoli_520@qq.com>
* licensed under Apache License 2.0
* https://github.com/yswenli/SAEA/blob/0a02687e370f8015946f70471a95ca2120f25145/Src/SAEA.Common/SAEASerialize.cs
*--------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SystemCommonLibrary.Reflect;

namespace SystemCommonLibrary.Serialization
{
    public static class BitSerializer
    {
        #region Serialize

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static byte[] Serialize(object param)
        {
            List<byte> datas = new List<byte>();

            var len = 0;

            byte[] data = null;

            if (param == null)
            {
                len = 0;
            }
            else
            {
                if (param is string)
                {
                    data = Encoding.UTF8.GetBytes((string)param);
                }
                else if (param is byte)
                {
                    data = new byte[] { (byte)param };
                }
                else if (param is bool)
                {
                    data = BitConverter.GetBytes((bool)param);
                }
                else if (param is short)
                {
                    data = BitConverter.GetBytes((short)param);
                }
                else if (param is int)
                {
                    data = BitConverter.GetBytes((int)param);
                }
                else if (param is long)
                {
                    data = BitConverter.GetBytes((long)param);
                }
                else if (param is float)
                {
                    data = BitConverter.GetBytes((float)param);
                }
                else if (param is double)
                {
                    data = BitConverter.GetBytes((double)param);
                }
                else if (param is decimal)
                {
                    var str = ((decimal)param).ToString();
                    data = Encoding.UTF8.GetBytes(str);
                }
                else if (param is DateTime)
                {
                    var str = ((DateTime)param).Ticks.ToString();
                    data = Encoding.UTF8.GetBytes(str);
                }
                else if (param is Guid)
                {
                    var str = ((Guid)param).ToString();
                    data = Encoding.UTF8.GetBytes(str);
                }
                else if (param is Type)
                {
                    var str = ((Type)param).AssemblyQualifiedName;
                    data = Encoding.UTF8.GetBytes(str);
                }
                else if (param is Enum)
                {
                    var enumValType = Enum.GetUnderlyingType(param.GetType());

                    if (enumValType == typeof(byte))
                    {
                        data = new byte[] { (byte)param };
                    }
                    else if (enumValType == typeof(short))
                    {
                        data = BitConverter.GetBytes((Int16)param);
                    }
                    else if (enumValType == typeof(int))
                    {
                        data = BitConverter.GetBytes((Int32)param);
                    }
                    else
                    {
                        data = BitConverter.GetBytes((Int64)param);
                    }
                }
                else if (param is byte[])
                {
                    data = (byte[])param;
                }
                else
                {
                    var type = param.GetType();

                    if (type.IsGenericType || type.IsArray)
                    {
                        if (SlimTypeInfo.DicTypes.Contains(type.Name))
                            data = SerializeDic((System.Collections.IDictionary)param);
                        else if (SlimTypeInfo.ListTypes.Contains(type.Name) || type.IsArray)
                            data = SerializeList((System.Collections.IEnumerable)param);
                        else
                            data = SerializeClass(param, type);
                    }
                    else if (type.IsClass)
                    {
                        data = SerializeClass(param, type);
                    }

                }
                if (data != null)
                    len = data.Length;
            }
            datas.AddRange(BitConverter.GetBytes(len));
            if (len > 0)
            {
                datas.AddRange(data);
            }
            return datas.ToArray();
        }

        private static byte[] SerializeClass(object obj, Type type)
        {
            if (obj == null) return null;

            List<byte> datas = new List<byte>();

            var len = 0;

            byte[] data = null;

            var fs = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            
            if (fs != null && fs.Length > 0)
            {
                var fields = new List<(Type, object)>();
                ApplyPropertySerializable(fs, type);
                foreach (var field in fs)
                {
                    if (field != null)
                    {
                        fields.Add((field.FieldType, field.GetValue(obj)));
                    }
                }
                
                data = SerializeFields(fields);

                len = data.Length;
            }


            if (len > 0)
                return data;
            else
                return null;
        }

        private static byte[] SerializeList(System.Collections.IEnumerable param)
        {
            if (param != null)
            {
                List<byte> slist = new List<byte>();

                var itemtype = param.AsQueryable().ElementType;

                foreach (var item in param)
                {
                    if (itemtype.IsClass && itemtype != typeof(string))
                    {
                        var ps = itemtype.GetProperties();

                        if (ps != null && ps.Length > 0)
                        {
                            List<object> clist = new List<object>();
                            foreach (var p in ps)
                            {
                                clist.Add(p.GetValue(item, null));
                            }
                            var clen = 0;
                            var cdata = Serialize(clist.ToArray());
                            if (cdata != null)
                            {
                                clen = cdata.Length;
                            }
                            slist.AddRange(BitConverter.GetBytes(clen));
                            slist.AddRange(cdata);
                        }
                    }
                    else
                    {
                        var clen = 0;
                        var cdata = Serialize(item);
                        if (cdata != null)
                        {
                            clen = cdata.Length;
                        }
                        slist.AddRange(BitConverter.GetBytes(clen));
                        slist.AddRange(cdata);
                    }
                }
                if (slist.Count > 0)
                {
                    return slist.ToArray();
                }
            }
            return null;
        }

        private static byte[] SerializeDic(System.Collections.IDictionary param)
        {
            if (param != null && param.Count > 0)
            {
                List<byte> list = new List<byte>();
                foreach (var item in param)
                {
                    var type = item.GetType();
                    var ps = type.GetProperties();
                    if (ps != null && ps.Length > 0)
                    {
                        List<object> clist = new List<object>();
                        foreach (var p in ps)
                        {
                            clist.Add(p.GetValue(item, null));
                        }
                        var clen = 0;

                        var cdata = Serialize(clist.ToArray());

                        if (cdata != null)
                        {
                            clen = cdata.Length;
                        }

                        if (clen > 0)
                        {
                            list.AddRange(cdata);
                        }
                    }
                }
                return list.ToArray();
            }
            return null;
        }

        private static void ApplyPropertySerializable(FieldInfo[] fields, Type type)
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

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        private static byte[] SerializeFields(List<(Type Name, object Value)> fields)
        {
            List<byte> datas = new List<byte>();

            if (fields != null)
            {
                foreach (var field in fields)
                {
                    if (field.Name.IsObject())
                    {
                        List<byte> objParam = new List<byte>();
                        var realTypeName = field.Value.GetType().AssemblyQualifiedName;
                        objParam.AddRange(Serialize(realTypeName));
                        objParam.AddRange(Serialize(field.Value));

                        datas.AddRange(Serialize(objParam.ToArray()));
                    }
                    else
                        datas.AddRange(Serialize(field.Value));
                }
            }

            return datas.Count == 0 ? null : datas.ToArray();
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="types"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static object[] Deserialize(Type[] types, byte[] datas)
        {
            List<object> list = new List<object>();

            int offset = 0;

            for (int i = 0; i < types.Length; i++)
            {
                list.Add(Deserialize(types[i], datas, ref offset));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            int offset = 0;
            return (T)Deserialize(typeof(T), data, ref offset);
        }

        public static object Deserialize(string typeName, byte[] data)
        {
            var type = Type.GetType(typeName);
            int offset = 0;
            return Deserialize(type, data, ref offset);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="datas"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object Deserialize(Type type, byte[] datas, ref int offset)
        {
            dynamic obj = null;

            var len = 0;

            byte[] data = null;

            len = BitConverter.ToInt32(datas, offset);
            offset += 4;
            if (len > 0)
            {
                data = new byte[len];
                Buffer.BlockCopy(datas, offset, data, 0, len);
                offset += len;

                if (type.IsObject())
                {
                    var lenTypeName = BitConverter.ToInt32(data, 0) + 4;
                    byte[] dataTypeName = new byte[lenTypeName];
                    Buffer.BlockCopy(data, 0, dataTypeName, 0, lenTypeName);
                    var realTypeName = (string)Deserialize(typeof(string).AssemblyQualifiedName, dataTypeName);

                    //将获得的类型赋值后处理真正的数据
                    type = Type.GetType(realTypeName);
                    var srcData = data;
                    len = BitConverter.ToInt32(data, lenTypeName);
                    data = new byte[len];
                    Buffer.BlockCopy(srcData, lenTypeName + 4, data, 0, len);
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
                    var dstr = Encoding.UTF8.GetString(data);

                    obj = decimal.Parse(dstr); ;
                }
                else if (type == typeof(DateTime))
                {
                    var dstr = Encoding.UTF8.GetString(data);
                    var ticks = long.Parse(dstr);
                    obj = (new DateTime(ticks));
                }
                else if (type == typeof(Guid))
                {
                    var dstr = Encoding.UTF8.GetString(data);
                    obj = Guid.Parse(dstr);
                }
                else if (type == typeof(Type))
                {
                    var dstr = Encoding.UTF8.GetString(data);
                    obj = Type.GetType(dstr);
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
                    obj = (byte[])data;
                }
                else if (type.IsGenericType)
                {
                    if (SlimTypeInfo.ListTypes.Contains(type.Name))
                    {
                        obj = DeserializeList(type, data);
                    }
                    else if (SlimTypeInfo.DicTypes.Contains(type.Name))
                    {
                        obj = DeserializeDic(type, data);
                    }
                    else
                    {
                        obj = DeserializeClass(type, data);
                    }
                }
                else if (type.IsArray)
                {
                    obj = DeserializeArray(type, data);
                }
                else if (type.IsClass)
                {
                    obj = DeserializeClass(type, data);
                }
                else
                {
                    throw new NotImplementedException("未定义的类型：" + type.ToString());
                }

            }
            else
            {
                var tinfo = SlimTypeInfo.GetOrAddInstance(type);
                obj = tinfo.Instance;
            }

            return obj;
        }

        private static object DeserializeClass(Type type, byte[] datas)
        {
            var tinfo = SlimTypeInfo.GetOrAddInstance(type);

            var instance = tinfo.Instance;

            var ts = new List<Type>();

            //var ps = type.GetProperties();
            var fs = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            ApplyPropertySerializable(fs, type);
            foreach (var field in fs)
            {
                if (field != null)
                {
                    ts.Add(field.FieldType);
                }
            }

            var vas = Deserialize(ts.ToArray(), datas);

            var fsWithoutNull = fs.Where(f => f != null).ToArray();
            for (int j = 0; j < fsWithoutNull.Length; j++)
            {
                try
                {
                    if (!fsWithoutNull[j].FieldType.IsGenericType)
                    {
                        fsWithoutNull[j].SetValue(instance, vas[j]);
                    }
                    else
                    {
                        Type genericTypeDefinition = fsWithoutNull[j].FieldType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            fsWithoutNull[j].SetValue(instance, Convert.ChangeType(vas[j], Nullable.GetUnderlyingType(fsWithoutNull[j].FieldType)));
                        }
                        else
                        {
                            fsWithoutNull[j].SetValue(instance, vas[j]);
                        }
                    }
                }
                catch(Exception ex)
                {
                    throw new NotImplementedException("未定义的类型：" + type.ToString(), ex);
                }
            }

            return instance;
        }

        private static object DeserializeList(Type type, byte[] datas)
        {
            var info = SlimTypeInfo.GetOrAddInstance(type);

            var instance = info.Instance;

            if (info.ArgTypes[0].IsClass && info.ArgTypes[0] != typeof(string))
            {
                //子项内容
                var slen = 0;
                var soffset = 0;
                while (soffset < datas.Length)
                {
                    slen = BitConverter.ToInt32(datas, soffset);
                    if (slen > 0)
                    {
                        var sobj = Deserialize(info.ArgTypes[0], datas, ref soffset);
                        if (sobj != null)
                            info.MethodInfo.Invoke(instance, new object[] { sobj });

                    }
                    else
                    {
                        info.MethodInfo.Invoke(instance, null);
                    }
                }
                return instance;
            }
            else
            {
                //子项内容
                var slen = 0;
                var soffset = 0;
                while (soffset < datas.Length)
                {
                    var len = BitConverter.ToInt32(datas, soffset);
                    var data = new byte[len];
                    Buffer.BlockCopy(datas, soffset + 4, data, 0, len);
                    soffset += 4;
                    slen = BitConverter.ToInt32(datas, soffset);
                    if (slen > 0)
                    {
                        var sobj = Deserialize(info.ArgTypes[0], datas, ref soffset);
                        if (sobj != null)
                            info.MethodInfo.Invoke(instance, new object[] { sobj });
                    }
                    else
                    {
                        info.MethodInfo.Invoke(instance, null);
                    }
                }
                return instance;
            }

        }

        private static object DeserializeArray(Type type, byte[] datas)
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

        private static object DeserializeDic(Type type, byte[] datas)
        {
            var tinfo = SlimTypeInfo.GetOrAddInstance(type);

            var instance = tinfo.Instance;

            //子项内容
            var slen = 0;

            var soffset = 0;

            int m = 1;

            object key = null;
            object val = null;

            while (soffset < datas.Length)
            {
                slen = BitConverter.ToInt32(datas, soffset);
                var sdata = new byte[slen + 4];
                Buffer.BlockCopy(datas, soffset, sdata, 0, slen + 4);
                soffset += slen + 4;
                if (m % 2 == 1)
                {
                    object v = null;
                    if (slen > 0)
                    {
                        int lloffset = 0;
                        var sobj = Deserialize(tinfo.ArgTypes[0], sdata, ref lloffset);
                        if (sobj != null)
                            v = sobj;
                    }
                    key = v;
                }
                else
                {
                    object v = null;
                    if (slen > 0)
                    {
                        int lloffset = 0;
                        var sobj = Deserialize(tinfo.ArgTypes[1], sdata, ref lloffset);
                        if (sobj != null)
                            v = sobj;
                    }
                    val = v;
                    tinfo.MethodInfo.Invoke(instance, new object[] { key, val });
                }
                m++;
            }
            return instance;
        }

        #endregion
    }
}
