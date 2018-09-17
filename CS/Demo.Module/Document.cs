using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace Demo.Module {
    [DefaultClassOptions]
    public class Document : BaseObject {
        public Document(Session session) : base(session) { }
        private string _name;
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }
        [Association("Document-Articles"), Aggregated]
        public XPCollection<Article> Articles {
            get {
                return GetCollection<Article>("Articles");
            }
        }
    }
}
