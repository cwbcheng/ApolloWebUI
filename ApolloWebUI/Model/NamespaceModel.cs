using System;
using System.Collections.Generic;

namespace ApolloWebUI.Model
{
    public class NamespaceModel
    {
        public string AppId { get; set; }
        public string ClusterName { get; set; }
        public string NamespaceName { get; set; }
        public string Comment { get; set; }
        public string Format { get; set; }
        public bool IsPublic { get; set; }
        public List<NamespaceItem> Items { get; set; }
        public string DataChangeCreatedBy { get; set; }
        public string DataChangeLastModifiedBy { get; set; }
        public DateTime DataChangeCreatedTime { get; set; }
        public DateTime DataChangeLastModifiedTime { get; set; }
    }

    public class NamespaceItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public string DataChangeCreatedBy { get; set; }
        public string DataChangeLastModifiedBy { get; set; }
        public DateTime DataChangeCreatedTime { get; set; }
        public DateTime DataChangeLastModifiedTime { get; set; }
    }
}
