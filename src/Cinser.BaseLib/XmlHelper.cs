using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Cinser.BaseLib
{
    public class XmlHelper
    {
        XmlDocument xmlDocument;

        public XmlDocument XmlDocument
        {
            get
            {
                if (xmlDocument == null)
                    xmlDocument = new XmlDocument();
                return xmlDocument;
            }
            set { xmlDocument = value; }
        }
        
        public XmlHelper(string xmlFilePath)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFilePath);
        }

        public XmlNode FirstNode
        {
            get
            {
                XmlNode node = this.XmlDocument.FirstChild;
                if(node.Name.ToLower()=="xml")
                {
                    node = this.XmlDocument.ChildNodes[1];
                }
                return node;
            }
        }
    }

    public static class XmlUtils
    {
        /// <summary>
        /// 取某节点的属性值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAttributeValue(this XmlNode node, string attributeName)
        {
            string value = null;
            if (node.HasAttribute(attributeName))
            {
                value = node.Attributes[attributeName].Value;
            }
            return value;
        }

        /// <summary>
        /// 判断某节点是否包含某属性
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool HasAttribute(this XmlNode node, string attributeName)
        {
            bool bReturn = false;
            foreach (XmlAttribute item in node.Attributes)
            {
                if(item.Name == attributeName)
                {
                    bReturn = true;
                    break;
                }
            }
            return bReturn;
        }

        public static Dictionary<string,string>GetAttributes(this XmlNode node)
        {
            Dictionary<string, string> value = new Dictionary<string, string>();
            foreach (XmlAttribute item in node.Attributes)
            {
                value.Add(item.Name, item.Value);
            }
            return value;
        }

        public static Dictionary<string, string> GetChildNodes(this XmlNode node)
        {
            Dictionary<string, string> value = new Dictionary<string, string>();
            foreach (XmlNode item in node.ChildNodes)
            {
                value.Add(item.Name, item.Value);
            }
            return value;
        }

        public static XmlNode FindNode(this XmlNode node, string nodeName, string attributeName, string attributeValue, bool bFindAllTreeNode)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            Dictionary<string, string> attrs = new Dictionary<string, string>();
            attrs.Add(attributeName, attributeValue);
            nodes = node.FindNode(nodeName, new List<string> { attributeName }, new List<string> { attributeValue }, bFindAllTreeNode);
            if (nodes != null && nodes.Count > 0)
                return nodes[0];
            else
                return null;
        }

        public static List<XmlNode> FindNode(this XmlNode node, string nodeName, List<string> attrNames, List<string> attrValues, bool bFindAllTreeNode)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == nodeName)
                {
                    //名字匹配成功后，匹配属性
                    if (attrNames != null && attrNames.Count > 0)
                    {
                        int okCount = 0;
                        for (int i = 0; i < attrNames.Count; i++)
                        {
                            if (item.HasAttribute(attrNames[i]) == false)
                                break;
                            if (item.GetAttributeValue(attrNames[i]) != attrValues[i])
                                break;
                            okCount++;
                        }
                        if (okCount == attrNames.Count)
                            nodes.Add(item);
                    }
                    else
                    {
                        nodes.Add(item);//只匹配节点名字
                    }
                }

                //搜寻子节点
                if (bFindAllTreeNode == true && item.ChildNodes.Count > 0)
                {
                    nodes.AddRange(item.FindNode(nodeName, attrNames, attrValues, bFindAllTreeNode));
                }
            }
            return nodes;
        }

        public static List<XmlNode> FindNode(this XmlNode node, string nodeName, bool bFindAllTreeNode)
        {
            return node.FindNode(nodeName, new List<string> { }, new List<string> { }, bFindAllTreeNode);
        }
        
        public static void SetNodeAttribute(this XmlNode node, string attributeName, string attributeValue)
        {
            if(node.HasAttribute(attributeName))
            {
                node.Attributes[attributeName].Value = attributeValue;
            }
            else
            {
                XmlElement e = node as XmlElement;
                e.SetAttribute(attributeName, attributeValue);
            }
        }

        public  delegate void XmlNodeHandler(XmlNode node);

        public static void UpdateNode(this XmlNode node, string nodeName, List<string> attrNames, List<string> attrValues, bool bFindAllTreeNode,XmlNodeHandler updateFunc)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == nodeName)
                {
                    //名字匹配成功后，匹配属性
                    if (attrNames != null && attrNames.Count > 0)
                    {
                        int okCount = 0;
                        for (int i = 0; i < attrNames.Count; i++)
                        {
                            if (item.HasAttribute(attrNames[i]) == false)
                                break;
                            if (item.GetAttributeValue(attrNames[i]) != attrValues[i])
                                break;
                            okCount++;
                        }
                        if (okCount == attrValues.Count)
                        {
                            if (updateFunc != null)
                            {
                                updateFunc(item);
                            }
                        }
                    }
                    else
                    {
                        if (updateFunc != null)
                        {
                            updateFunc(item);
                        }
                    }
                }
                //搜寻子节点
                if (bFindAllTreeNode ==true &&  item.ChildNodes.Count > 0)
                {
                    item.UpdateNode(nodeName, attrNames, attrValues, bFindAllTreeNode,updateFunc);
                }
            }
        }
    }
}
