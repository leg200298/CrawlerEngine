using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CrawlerEngine.Common.Extansion
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 取得描述字串
        /// </summary>
        /// <param name="value">列舉</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                return null;
            }
            else if (double.TryParse(value.ToString(), out double number)) // 若是數字直接回傳結果
            {
                return value.ToString();
            }

            Type type = value.GetType();

            // Make sure the object is an enum.
            if (!type.IsEnum)
            {
                throw new ApplicationException("其值必須為列舉");
            }

            FieldInfo fieldInfo = type.GetField(value.ToString());
            object[] descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptionAttributes == null || descriptionAttributes.Length == 0)
            {
                object[] enforcementAttributes = type.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (enforcementAttributes != null && enforcementAttributes.Length == 1)
                {
                    DescriptionAttribute enforcementAttribute = (DescriptionAttribute)enforcementAttributes[0];

                    return value.ToString();
                }
                else // Just return the name of the enum.
                {
                    return value.ToString();
                }
            }
            else if (descriptionAttributes.Length > 1)
            {
                throw new ApplicationException("列舉類型「" + type.Name + "」有過多的Description屬性，相對應的列舉為「" + value.ToString() + "」");
            }

            // Return the value of the DescriptionAttribute.
            return (descriptionAttributes[0] as DescriptionAttribute).Description;
        }

        /// <summary>
        /// 取得列舉物件中指定的屬性
        /// </summary>
        /// <typeparam name="TAttribute">指定的屬性類型</typeparam>
        /// <param name="enumObj">列舉物件</param>
        /// <exception cref="NullReferenceException">型別轉換時發生錯誤</exception>
        /// <exception cref="Exception">Cannot find specified attribute type from enum object.</exception>
        /// <returns>指定的屬性物件</returns>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumObj) where TAttribute : Attribute
        {
            var objName = enumObj.ToString();
            var type = enumObj.GetType();
            var fieldInfo = type.GetField(objName);

            if (!(fieldInfo.GetCustomAttributes(typeof(TAttribute), false) is TAttribute[] attributes))
            {
                throw new NullReferenceException("型別轉換時發生錯誤");
            }

            if (attributes.Length == 0)
            {
                throw new Exception("Cannot find specified attribute type from enum object.");
            }

            return attributes[0];
        }

        /// <summary>
        /// 取得列舉值對應之列舉物件中指定的屬性
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <typeparam name="TAttribute">指定的屬性類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>指定的屬性物件</returns>
        public static TAttribute GetAttribute<TEnum, TAttribute>(this int enumValue) where TAttribute : Attribute
        {
            var e = Enum.Parse(typeof(TEnum), Convert.ToString(enumValue));

            return ((Enum)e).GetAttribute<TAttribute>();
        }

        /// <summary>
        /// 取得列舉物件的顯示名稱
        /// </summary>
        /// <param name="enumObj">列舉物件</param>
        /// <returns>
        /// 列舉顯示名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDisplayName(this Enum enumObj)
        {
            try
            {
                return GetAttribute<DisplayAttribute>(enumObj).Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉值所表示的列舉物件的顯示名稱
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>
        /// 列舉顯示名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDisplayName<TEnum>(this int enumValue)
        {
            try
            {
                return GetAttribute<TEnum, DisplayAttribute>(enumValue).Name;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉值所表示的列舉物件的描述
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>
        /// 列舉描述
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetDescription<TEnum>(this int enumValue)
        {
            try
            {
                return GetAttribute<TEnum, DescriptionAttribute>(enumValue).Description;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉物件的短名稱
        /// </summary>
        /// <param name="enumObj">列舉物件</param>
        /// <returns>
        /// 列舉短名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetShortNames(this Enum enumObj)
        {
            try
            {
                return GetAttribute<DisplayAttribute>(enumObj).ShortName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉值所表示的列舉物件的短名稱
        /// </summary>
        /// <typeparam name="TEnum">列舉類型</typeparam>
        /// <param name="enumValue">列舉值</param>
        /// <returns>
        /// 列舉短名稱
        /// <remarks>如果執行中發生例外，將回傳<see cref="string.Empty"/></remarks>
        /// </returns>
        public static string GetShortNames<TEnum>(this int enumValue)
        {
            try
            {
                return GetAttribute<TEnum, DisplayAttribute>(enumValue).ShortName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得列舉Property名稱
        /// </summary>
        /// <typeparam name="TEnum">列舉類別</typeparam>
        /// <param name="enumValue">列舉數字</param>
        /// <returns>列舉名稱</returns>
        public static string GetName<TEnum>(this int enumValue)
        {
            var property = Enum.Parse(typeof(TEnum), Convert.ToString(enumValue));

            return property.ToString();
        }

        /// <summary>
        /// 使用自訂方法，依序對所有列舉物件執行方法，並取得方法回傳值所構成的<![CDATA[IEnumerable<T>]]>物件
        /// </summary>
        /// <typeparam name="T">IEnumerable的裝載型別</typeparam>
        /// <typeparam name="TEnum">列舉型別</typeparam>
        /// <param name="getT">自訂方法，表示要對每個列舉物件執行的動作</param>
        /// <returns>自訂方法回傳值所構成的<![CDATA[IEnumerable<T>]]>物件</returns>
        public static IEnumerable<T> AsIEnumerable<T, TEnum>(Func<Enum, T> getT)
        {
            var result = new List<T>();
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                result.Add(getT((Enum)value));
            }

            return result;
        }
    }
}
