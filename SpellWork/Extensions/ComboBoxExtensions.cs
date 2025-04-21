using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace SpellWork.Extensions
{
    public static class ComboBoxExtensions
    {
        public static void SetEnumValues<T>(this ComboBox cb, string noValue)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");

            dt.Rows.Add(-1, noValue);

            foreach (var str in Enum.GetValues(typeof(T)))
                dt.Rows.Add((int)str, "(" + ((int)str).ToString("000") + ") " + str);

            cb.DataSource = dt;
            cb.DisplayMember = "NAME";
            cb.ValueMember = "ID";
        }

        public static void SetEnumValuesDirect<T>(this ComboBox cb, bool setFirstValue)
        {
            cb.BeginUpdate();

            cb.Items.Clear();
            foreach (var value in Enum.GetValues(typeof(T)))
                cb.Items.Add(((Enum)value).GetFullName());

            if (setFirstValue && cb.Items.Count > 0)
                cb.SelectedIndex = 0;

            cb.EndUpdate();
        }

        public static void SetStructFields<T>(this ComboBox cb)
        {
            cb.Items.Clear();

            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(MemberInfo));
            dt.Columns.Add("NAME", typeof(string));

            var type = typeof(T).GetMembers();
            var i = 0;
            foreach (var str in type)
            {
                if (!(str is FieldInfo) && !(str is PropertyInfo))
                    continue;

                var dr = dt.NewRow();
                dr["ID"] = str;
                dr["NAME"] = $"({i:000}) {str.Name}";
                dt.Rows.Add(dr);
                ++i;
            }

            cb.DataSource    = dt;
            cb.DisplayMember = "NAME";
            cb.ValueMember   = "ID";
        }
    }
}