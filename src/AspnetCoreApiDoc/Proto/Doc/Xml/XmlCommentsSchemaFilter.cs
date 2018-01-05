using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using AspnetCoreApiDoc.Filter;
using Microsoft.Extensions.PlatformAbstractions;

namespace AspnetCoreApiDoc.Proto.Doc.Xml
{
    public class XmlCommentsSchemaFilter : IXmlCommentsSchemaFilter
    {
        private const string MemberXPath = "/doc/members/member[@name='{0}']";
        private const string SummaryTag = "summary";

        private readonly List<XPathNavigator> _xmlList = new List<XPathNavigator>();

        public XmlCommentsSchemaFilter()
        {
            var files = Directory.GetFiles(PlatformServices.Default.Application.ApplicationBasePath, "*.xml");
            foreach (var file in files)
            {
                _xmlList.Add(new XPathDocument(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file))
                    .CreateNavigator());
            }
        }

        public string GetTypeDec(Type systemType)
        {
            foreach (var xPathNavigator in _xmlList)
            {
                var dec = "";
                var commentId = XmlCommentsIdHelper.GetCommentIdForType(systemType);
                var typeNode = xPathNavigator.SelectSingleNode(string.Format(MemberXPath, commentId));

                var summaryNode = typeNode?.SelectSingleNode(SummaryTag);
                if (summaryNode != null)
                    dec = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
                
                if (!string.IsNullOrWhiteSpace(dec))
                    return dec;
            }
            return string.Empty;
        }

        public string GetMethodDec(PropertyInfo systemType)
        {
            foreach (var xPathNavigator in _xmlList)
            {
                var dec = "";
                var commentId = XmlCommentsIdHelper.GetCommentIdForProperty(systemType);
                var typeNode = xPathNavigator.SelectSingleNode(string.Format(MemberXPath, commentId));

                var summaryNode = typeNode?.SelectSingleNode(SummaryTag);
                if (summaryNode != null)
                    dec = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
                
                if (!string.IsNullOrWhiteSpace(dec))
                    return dec;
            }
            return string.Empty;
        }

        public string GetFuncDec(MethodInfo methodInfo)
        {
            foreach (var xPathNavigator in _xmlList)
            {
                var dec = "";
                var commentId = XmlCommentsIdHelper.GetCommentIdForMethod(methodInfo);
                var typeNode = xPathNavigator.SelectSingleNode(string.Format(MemberXPath, commentId));

                var summaryNode = typeNode?.SelectSingleNode(SummaryTag);
                if (summaryNode != null)
                    dec = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);

                if (!string.IsNullOrWhiteSpace(dec))
                    return dec;
            }
            return string.Empty;
        }

        public string GetEnumDesc(Type enumType, string enumName)
        {
            foreach (var xPathNavigator in _xmlList)
            {
                var dec = "";
                var commentId = XmlCommentsIdHelper.GetCommentIdForEnum(enumType,enumName);
                var typeNode = xPathNavigator.SelectSingleNode(string.Format(MemberXPath, commentId));

                var summaryNode = typeNode?.SelectSingleNode(SummaryTag);
                if (summaryNode != null)
                    dec = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
                
                if (!string.IsNullOrWhiteSpace(dec))
                    return dec;
            }
            return string.Empty;
        }

    }
}
