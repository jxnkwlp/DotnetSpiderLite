//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Globalization;
//using System.Text;

//namespace DotnetSpiderLite.Redis
//{
//    public class RedisStore
//    {
//        IDatabase Database { get; }

//        public RedisStore(string connectString)
//        {
//            var connection = ConnectionMultiplexer.Connect(connectString);

//            this.Database = connection.GetDatabase();
//        }

//        #region String

//        public bool StringSet<T>(string key, T value, TimeSpan? time = null)
//        {
//            return Database.StringSet(key, ConventToRedisValue(value), time);
//        }

//        //public bool StringSet(string key, string value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value, time);
//        //}

//        //public bool StringSet(string key, bool value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value, time);
//        //}

//        //public bool StringSet(string key, int value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value, time);
//        //}

//        //public bool StringSet(string key, long value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value, time);
//        //}

//        //public bool StringSet(string key, double value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value, time);
//        //}

//        //public bool StringSet(string key, byte[] value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value, time);
//        //}

//        //public bool StringSet(string key, DateTime value, TimeSpan? time = null)
//        //{
//        //    return Database.StringSet(key, value.ToString(), time);
//        //}

//        public string StringGet(string key)
//        {
//            return Database.StringGet(key);
//        }

//        public TResult StringGet<TResult>(string key)
//        {
//            var value = Database.StringGet(key);

//            return ConventFromRedisValue<TResult>(value);
//        }

//        public long StringIncrement(string key, long value = 1)
//        {
//            return Database.StringIncrement(key, value);
//        }

//        public double StringIncrement(string key, double value = 1)
//        {
//            return Database.StringIncrement(key, value);
//        }

//        public long StringDecrement(string key, long value = 1)
//        {
//            return Database.StringDecrement(key, value);
//        }

//        public double StringDecrement(string key, double value = 1)
//        {
//            return Database.StringDecrement(key, value);
//        }

//        #endregion

//        #region Hash

//        public bool HashSet<TKey, TValue>(string key, TKey hashKey, TValue hashValue)
//        {
//            return Database.HashSet(key, ConventToRedisValue(hashKey), ConventToRedisValue(hashValue));
//        }

//        public TValue HashGet<TKey, TValue>(string key, TKey hashKey)
//        {
//            var value = Database.HashGet(key, ConventToRedisValue(hashKey));
//            return ConventFromRedisValue<TValue>(value);
//        }

//        public bool HashExists<TKey>(string key, TKey hashKey)
//        {
//            return Database.HashExists(key, ConventToRedisValue(hashKey));
//        }

//        public bool HashDelete<TKey>(string key, TKey hashKey)
//        {
//            return Database.HashDelete(key, ConventToRedisValue(hashKey));
//        }

//        public string[] HashKeys(string key)
//        {
//            return Database.HashKeys(key).ToStringArray();
//        }

//        public long HashLength(string key)
//        {
//            return Database.HashLength(key);
//        }


//        public long HashDecrement(string key, string hashKey, long value = 1)
//        {
//            return Database.HashDecrement(key, hashKey, value);
//        }

//        public double HashDecrement(string key, string hashKey, double value = 1)
//        {
//            return Database.HashDecrement(key, hashKey, value);
//        }

//        public long HashIncrement(string key, string hashKey, long value = 1)
//        {
//            return Database.HashIncrement(key, hashKey, value);
//        }

//        public double HashIncrement(string key, string hashKey, double value = 1)
//        {
//            return Database.HashIncrement(key, hashKey, value);
//        }

//        #endregion

//        #region Set

//        public bool SetAdd<T>(string key, T value)
//        {
//            return Database.SetAdd(key, ConventToRedisValue(value)); 
//        }

//        public bool SetAdd<T>(string key, T value)
//        {
//            return Database.set(key, ConventToRedisValue(value));
//        }

//        public bool SetAdd<T>(string key, T value)
//        {
//            return Database.SetAdd(key, ConventToRedisValue(value));
//        }

//        public bool SetAdd<T>(string key, T value)
//        {
//            return Database.SetAdd(key, ConventToRedisValue(value));
//        }

//        public bool SetAdd<T>(string key, T value)
//        {
//            return Database.SetAdd(key, ConventToRedisValue(value));
//        }

//        #endregion



//        #region Helper

//        static T ConventFromRedisValue<T>(RedisValue redisValue)
//        {
//            // 是类
//            if (typeof(T).IsClass)
//            {
//                // TODO 解析 JSON 为 类
//                throw new NotImplementedException();
//            }
//            else if (typeof(T).IsValueType)
//            {
//                if (redisValue.IsInteger)
//                {
//                    return (T)ConventTo((int)redisValue, typeof(T), CultureInfo.CurrentCulture);
//                }
//                else if (redisValue.IsNull || redisValue.IsNullOrEmpty)
//                {
//                    return default(T);
//                }
//                else
//                {
//                    return (T)ConventTo(redisValue.ToString(), typeof(T), CultureInfo.CurrentCulture);
//                }
//            }
//            else
//            {
//                throw new NotSupportedException();
//            }
//        }

//        static RedisValue ConventToRedisValue<T>(T value)
//        {
//            // 是类
//            if (typeof(T).IsClass)
//            {
//                // TODO 保存为JSON字符串
//                throw new NotImplementedException();
//            }
//            else if (typeof(T).IsValueType)
//            {
//                return value.ToString();
//            }
//            else
//            {
//                throw new NotSupportedException();
//            }
//        }

//        static object ConventTo(object value, Type destinationType, CultureInfo culture)
//        {
//            if (value != null)
//            {
//                var sourceType = value.GetType();

//                TypeConverter destinationConverter = TypeDescriptor.GetConverter(destinationType);
//                TypeConverter sourceConverter = TypeDescriptor.GetConverter(sourceType);
//                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
//                    return destinationConverter.ConvertFrom(null, culture, value);
//                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
//                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
//                if (destinationType.IsEnum && (value is Int32 || value is Int64))
//                    return Enum.ToObject(destinationType, Convert.ToInt32(value));
//                if (!destinationType.IsInstanceOfType(value))
//                    return Convert.ChangeType(value, destinationType, culture);
//            }
//            return value;
//        }

//        #endregion

//    }
//}
