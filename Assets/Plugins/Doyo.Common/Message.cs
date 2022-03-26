using System;
using System.Collections.Generic;

namespace Doyo.Common
{
    public class Message
    {
        public TypeCode TypeCode { get; } = TypeCode.Object;

        private int m_intData;
        private long m_longData;
        private float m_floatData;
        private double m_doubleData;
        private string m_stringData;
        private char m_charData;
        private bool m_boolData;
        private DateTime m_dateTimeData;
        private object m_objectData;

        public int Count
        {
            get
            {
                if (m_dicData == null)
                {
                    return 1;
                }
                else
                {
                    return m_dicData.Count;
                }
            }
        }

        private Dictionary<string, Message> m_dicData;

        public Message this[string key]
        {
            get
            {
                if (!CheckDataExistInDictionary(key))
                {
                    return null;
                }
                return m_dicData[key];
            }
            set
            {
                if (m_dicData == null)
                {
                    m_dicData = new Dictionary<string, Message>();
                }
                m_dicData[key] = value;
            }
        }

        private static readonly string InvalidCastExceptionFormatBase = "StandardMsg doesn't hold an {0}";

        private static readonly Dictionary<TypeCode, Type> BaseTypeDic = new Dictionary<TypeCode, Type>()
        {
            {TypeCode.Int32, typeof(int)},
            {TypeCode.Int64, typeof(long)},
            {TypeCode.Single, typeof(float)},
            {TypeCode.Double, typeof(double)},
            {TypeCode.String, typeof(string)},
            {TypeCode.Char, typeof(char)},
            {TypeCode.Boolean, typeof(bool)},
            {TypeCode.DateTime, typeof(DateTime)},
        };

        #region 构造函数

        public Message()
        {
            TypeCode = TypeCode.Object;
            m_objectData = m_dicData = new Dictionary<string, Message>();
        }

        public Message(int data)
        {
            m_intData = data;
            TypeCode = TypeCode.Int32;
        }

        public Message(long data)
        {
            m_longData = data;
            TypeCode = TypeCode.Int64;
        }

        public Message(float data)
        {
            m_floatData = data;
            TypeCode = TypeCode.Single;
        }

        public Message(double data)
        {
            m_doubleData = data;
            TypeCode = TypeCode.Double;
        }

        public Message(string data)
        {
            m_stringData = data;
            TypeCode = TypeCode.String;
        }

        public Message(char data)
        {
            m_charData = data;
            TypeCode = TypeCode.Char;
        }

        public Message(bool data)
        {
            m_boolData = data;
            TypeCode = TypeCode.Boolean;
        }

        public Message(DateTime data)
        {
            m_dateTimeData = data;
            TypeCode = TypeCode.DateTime;
        }

        public Message(object data)
        {
            m_objectData = data;
            TypeCode = TypeCode.Object;
        }

        #endregion

        public T As<T>() where T : class
        {
            if (TypeCode != TypeCode.Object)
            {
                return null;
            }
            if (!(m_objectData is T))
            {
                return null;
            }
            return m_objectData as T;
        }

        public bool ContainsKey(string key)
        {
            if (m_dicData == null)
            {
                return false;
            }
            return m_dicData.ContainsKey(key);
        }

        public bool Add(string key, Message msg)
        {
            if (TypeCode != TypeCode.Object)
            {
                throw new Exception("StandardMsg is not type of dic");
            }
            if (m_dicData.ContainsKey(key))
            {
                return false;
            }
            m_dicData.Add(key, msg);
            return true;
        }

        #region 类型验证

        public Type GetDataType()
        {
            Type type;
            if (TypeCode == TypeCode.Object)
            {
                type = m_objectData.GetType();
            }
            else
            {
                if (BaseTypeDic.ContainsKey(TypeCode))
                {
                    type = BaseTypeDic[TypeCode];
                }
                else
                {
                    type = null;
                }
            }
            return type;
        }

        public bool IsDestinationType(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            bool isDestType;
            if (typeCode == TypeCode.Object)
            {
                if (m_objectData == null)
                {
                    isDestType = false;
                }
                else
                {
                    isDestType = type == m_objectData.GetType();
                }
            }
            else
            {
                isDestType = typeCode == TypeCode;
            }
            return isDestType;
        }

        public bool IsDestinationType<T>()
        {
            return IsDestinationType(typeof(T));
        }

        private bool CheckDataExistInDictionary(string key)
        {
            if (m_dicData == null)
            {
                throw new NullReferenceException("StandarMsg doesn't have a dictionary data");
            }
            if (!m_dicData.ContainsKey(key))
            {
                throw new KeyNotFoundException(string.Format("the key named \"{0}\" is not present in StandarMsg", key));
            }
            return true;
        }

        #endregion

        #region 隐式转换

        public static implicit operator Message(int data)
        {
            return new Message(data);
        }

        public static implicit operator Message(long data)
        {
            return new Message(data);
        }

        public static implicit operator Message(float data)
        {
            return new Message(data);
        }

        public static implicit operator Message(double data)
        {
            return new Message(data);
        }

        public static implicit operator Message(string data)
        {
            return new Message(data);
        }

        public static implicit operator Message(char data)
        {
            return new Message(data);
        }

        public static implicit operator Message(bool data)
        {
            return new Message(data);
        }

        public static implicit operator Message(DateTime data)
        {
            return new Message(data);
        }

        #endregion

        #region 显示转换

        public static explicit operator int(Message msg)
        {
            if (msg.TypeCode != TypeCode.Int32 && msg.TypeCode != TypeCode.Int64)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("int"));
            }
            return msg.m_intData;
        }

        public static explicit operator long(Message msg)
        {
            if (msg.TypeCode != TypeCode.Int32 && msg.TypeCode != TypeCode.Int64)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("long"));
            }
            return msg.m_longData;
        }

        public static explicit operator float(Message msg)
        {
            if (msg.TypeCode != TypeCode.Single && msg.TypeCode != TypeCode.Double)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("float"));
            }
            return msg.m_floatData;
        }

        public static explicit operator double(Message msg)
        {
            if (msg.TypeCode != TypeCode.Single && msg.TypeCode != TypeCode.Double)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("double"));
            }
            return msg.m_doubleData;
        }

        public static explicit operator string(Message msg)
        {
            if (msg == null)
            {
                return null;
            }
            if (msg.TypeCode != TypeCode.String)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("string"));
            }
            return msg.m_stringData;
        }

        public static explicit operator char(Message msg)
        {
            if (msg.TypeCode != TypeCode.Char)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("char"));
            }
            return msg.m_charData;
        }

        public static explicit operator bool(Message msg)
        {
            if (msg.TypeCode != TypeCode.Boolean)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("bool"));
            }
            return msg.m_boolData;
        }

        public static explicit operator DateTime(Message msg)
        {
            if (msg.TypeCode != TypeCode.DateTime)
            {
                throw new InvalidCastException(GenerateInvalidCastExceptionDesc("DateTime"));
            }
            return msg.m_dateTimeData;
        }

        private static string GenerateInvalidCastExceptionDesc(string type)
        {
            return string.Format(InvalidCastExceptionFormatBase, type);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            switch (TypeCode)
            {
                case TypeCode.Int32:
                    return m_intData.ToString();
                case TypeCode.Int64:
                    return m_longData.ToString();
                case TypeCode.Single:
                    return m_floatData.ToString();
                case TypeCode.Double:
                    return m_doubleData.ToString();
                case TypeCode.String:
                    return m_stringData;
                case TypeCode.Char:
                    return m_charData.ToString();
                case TypeCode.Boolean:
                    return m_boolData.ToString();
                case TypeCode.DateTime:
                    return m_dateTimeData.ToString();
                case TypeCode.Object:
                    return m_objectData.ToString();
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
