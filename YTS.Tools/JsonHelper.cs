using System;
using System.IO;
using Newtonsoft.Json;

namespace YTS.Tools
{
    /// <summary>
    /// JSON帮助类
    /// </summary>
    public static class JsonHelper
    {
        #region ====== Newtonsoft.Json BLL Json Serialize Deserialize ======
        /// <summary>
        /// 序列化: 对象 转为 JSON格式
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json字符串</returns>
        public static String ToString(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化: JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型 可以是数据</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T ToObject<T>(String json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 反序列化: JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T ToAnonymousType<T>(String json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;

            /*
                DeserializeAnonymousType 使用方法:
                //匿名对象解析
                var tempEntity = new { ID = 0, Name = String.Empty };
                String json5 = JsonHelper.SerializeObject(tempEntity);
                //json5 : {"ID":0,"Name":""}
                tempEntity = JsonHelper.DeserializeAnonymousType("{\"ID\":\"112\",\"Name\":\"石子儿\"}", tempEntity);
                Console.WriteLine(tempEntity.ID + ":" + tempEntity.Name);
            */
        }
        #endregion
    }
}
