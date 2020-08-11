using System.Net.Http;
using System.Net.Http.Headers;
using EvidentInstruction.Helpers;


namespace EvidentInstruction.Service.Helpers
{
    public static class ServiceHelpers
    {
        public static object GetObjectFromString(string str)
        {
            var xDoc = Converter.CreateXDoc(str);
            if (xDoc == null)
            {
                var xmlDoc = Converter.CreateXmlDoc(str);
                if (xmlDoc == null)
                {
                    var jsonDoc = Converter.CreateJson(str);
                    if (jsonDoc == null)
                    {
                        return str;
                    }
                    else
                    {
                        return jsonDoc;
                    }
                }
                else
                {
                    return xmlDoc;
                }
            }
            else
            {
                return xDoc;
            }
        }

    }
}
