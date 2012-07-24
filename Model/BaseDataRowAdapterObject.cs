using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using System.Reflection;

namespace Apollo.Model
{
    /// <summary>
    /// Base class for functionality shared between different data adapter objects.  Mostly just typed pointers to the
    /// DataTable and DataSet the object is associated with.
    /// </summary>
    /// <typeparam name="T">Type of DataRow this adapter represents.</typeparam>
    /// <typeparam name="U">Type of DataTable the underlying DataRow exists in.</typeparam>
    /// <typeparam name="V">Type of DataSet the underlying DataRow exists in.</typeparam>
    public abstract class BaseDataRowAdapterObject<T, U, V>
        where T : DataRow
        where U : DataTable
        where V : DataSet
    {
        readonly protected T _Item;

        public U DataTable
        {
            get { return (U)_Item.Table; }
        }

        public V DataSet
        {
            get { return (V)DataTable.DataSet; }
        }

        internal BaseDataRowAdapterObject(T item)
        {
            if (null == item)
                throw new ArgumentNullException("item", "DataRow object may not be null");
            _Item = item;
        }

        // bms_todo: See if there is a way to auto-generate properties that are linked to columns on the DataRow.
    }
}
