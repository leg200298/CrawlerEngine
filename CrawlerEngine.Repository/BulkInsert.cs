using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace CrawlerEngine.Repository
{
    public class BulkInsert<T>
    {
        /// <summary>
        ///  Ado.net BulkInset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt">資料來源</param>
        /// <param name="tableName">資料表名稱</param>
        /// <param name="connectString">連線對象字串</param>
        public void BulkInsertRecords(ref List<T> dt, string tableName, string connectString)
        {
            try
            {
                var bulkCopy = new SqlBulkCopy(connectString, SqlBulkCopyOptions.TableLock);
                bulkCopy.BulkCopyTimeout = 300;
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(ToDataTable(dt));
                dt.Clear();
            }
            catch (Exception)
            {
                throw new Exception("BCP出錯");
            }
            finally
            {
            }
        }

        private DataTable ToDataTable(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType); // 解決DataSet 不支援 System.Nullable<>
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }
}
