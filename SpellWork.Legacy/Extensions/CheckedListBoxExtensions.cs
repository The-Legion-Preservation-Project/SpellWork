using System;
using System.Windows.Forms;

namespace SpellWork.Extensions
{
    public static class CheckedListBoxExtensions
    {
        public static void SetCheckedItemFromFlag(this CheckedListBox name, uint value)
        {
            for (var i = 0; i < name.Items.Count; ++i)
                name.SetItemChecked(i, value / (1U << (i - 1)) % 2 != 0);
        }

        public static uint GetFlagsValue(this CheckedListBox name)
        {
            uint val = 0;
            for (var i = 0; i < name.CheckedIndices.Count; ++i)
                val |= 1U << name.CheckedIndices[i];

            return val;
        }

        public static void SetFlags<T>(this CheckedListBox clb)
        {
            clb.Items.Clear();

            foreach (var elem in Enum.GetValues(typeof(T)))
                clb.Items.Add(elem.ToString().NormalizeString());
        }

        public static void SetFlags<T>(this CheckedListBox clb, string remove)
        {
            clb.Items.Clear();

            foreach (var elem in Enum.GetValues(typeof(T)))
                clb.Items.Add(elem.ToString().NormalizeString(remove));
        }

        public static void SetFlags(this CheckedListBox clb, Type type, string remove)
        {
            clb.Items.Clear();

            foreach (var elem in Enum.GetValues(type))
                clb.Items.Add(elem.ToString().NormalizeString(remove));
        }
    }
}