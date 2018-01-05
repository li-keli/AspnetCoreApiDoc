using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AspnetCoreApiDoc.Extensions
{
    internal static class XmlExctensions
    {
        internal static string ToXml(this Type t, object obj)
        {
            // 序列化这个对象  
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true; // 不生成声明头  

            MemoryStream stream = new MemoryStream();
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(xmlWriter, obj, namespaces);
                xmlWriter.Dispose();
            }

            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            string str = sr.ReadToEnd();

            stream.Dispose();
            sr.Dispose();

            return str;
        }
    }
}