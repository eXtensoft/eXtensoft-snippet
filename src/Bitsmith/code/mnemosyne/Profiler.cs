using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsmith.code.mnemosyne
{
    public class Profiler
    {
        public int Count { get; set; }
        public ProfileFields Fields { get; set; } = new ProfileFields();
        public void Profile<T>(IEnumerable<T> items, Action<T,Profiler> extract)
        {
            foreach (var item in items)
            {
                Count++;
                extract(item, this);
            }
        }

        public void Profile(string fieldName, string value)
        {
            Fields.Profile(fieldName, value);
        }

        public DataSet Report(StringBuilder sb)
        {
            DataSet ds = new DataSet() { DataSetName = "Profile" };
            foreach (ProfileField field in Fields)
            {
                DataTable dt = field.Report(sb);
                ds.Tables.Add(dt);
            }
            return ds;
        }

    }

    public class ProfileFields : KeyedCollection<string, ProfileField>
    {
        protected override string GetKeyForItem(ProfileField item)
        {
            return item.Name;
        }
        public void Profile(string fieldName, string value)
        {
            if (!Contains(fieldName))
            {
                Add(new ProfileField(fieldName, value));
            }
            else
            {
                this[fieldName].Profile(fieldName, value);
            }
        }
        public override string ToString()
        {
            return $"{this.Count()}";
        }
    }

    public class ProfileField : KeyedCollection<string, ProfileItem>
    {
        public string Name { get { return this[0].FieldName; } }

        public ProfileField(string fieldName, string value)
        {
            Add(new ProfileItem() { FieldName = fieldName, Value = value, Count = 1 });
        }
        protected override string GetKeyForItem(ProfileItem item)
        {
            return item.Value;
        }
        public void Profile(string fieldName, string value)
        {
            if (!Contains(value))
            {
                Add(new ProfileItem() { FieldName = fieldName, Value = value, Count = 1 });
            }
            else
            {
                this[value].Count++;
            }
        }
        public override string ToString()
        {
            return $"{Name}: {Count}";
        }

        internal DataTable Report(StringBuilder sb)
        {
            DataTable dt = new DataTable() { TableName = this[0].FieldName };
            dt.Columns.Add(new DataColumn("FieldValue", typeof(string)));
            dt.Columns.Add(new DataColumn("Count", typeof(Int32)));
            foreach (ProfileItem item in this)
            {
                DataRow row = dt.NewRow();
                row["FieldValue"] = item.Value;
                row["Count"] = item.Count;
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    public class ProfileItem
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public override string ToString()
        {
            return $"{Value}: {Count}";
        }
    }
}