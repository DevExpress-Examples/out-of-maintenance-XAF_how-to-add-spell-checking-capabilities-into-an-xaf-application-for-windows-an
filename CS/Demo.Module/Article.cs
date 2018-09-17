using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace Demo.Module {
    public class Article : BaseObject {
        public Article(Session session) : base(session) { }
        private string _subject;
        public string Subject {
            get { return _subject; }
            set { SetPropertyValue("Subject", ref _subject, value); }
        }
        private string _description;
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return _description; }
            set { SetPropertyValue("Description", ref _description, value); }
        }
        private string _body;
        [Size(SizeAttribute.Unlimited)]
        public string Body {
            get { return _body; }
            set { SetPropertyValue("Body", ref _body, value); }
        }
        private Document _document;
        [Association("Document-Articles")]
        public Document Document {
            get { return _document; }
            set { SetPropertyValue("Document", ref _document, value); }
        }
        private DateTime _createdOn;
        public DateTime CreatedOn {
            get { return _createdOn; }
            set { SetPropertyValue("CreatedOn", ref _createdOn, value); }
        }
    }
}