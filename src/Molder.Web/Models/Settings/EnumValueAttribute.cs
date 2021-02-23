namespace Molder.Web.Models.Settings
{
    public class EnumValue : System.Attribute
    {
        public EnumValue(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
