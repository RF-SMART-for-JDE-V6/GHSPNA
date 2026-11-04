using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ICS.Stoneridge.Wireless.JDE.Utilities.Extensions
{
    /// <summary>
    /// Contains a collection of Extensions in relation to DataTables (as well as DataRows and DataColumns)
    /// </summary>
    public static class DataTableExtensions
    {
        #region ColumnToEnumerable

        /// <summary>
        /// Returns all the values for a Column in the DataTable as an Enumerable
        /// </summary>
        /// <typeparam name="T">Type of the Enumerable to Return</typeparam>
        /// <param name="value">DataTable to retireve the value from</param>
        /// <param name="ColumnName">The name of the Column to retrieve values from</param>
        /// <returns>Returns an Enumerable of values (or null, should an error occur)</returns>
        public static IEnumerable<T> ColumnToEnumerable<T>(this DataTable value, string ColumnName)
        {
            try
            {
                return value.Rows.Cast<DataRow>().Select(row => (T) Convert.ChangeType(row[ColumnName].ToString(), typeof(T)));
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}