using System;
using System.IO;
using System.Reflection;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.XtraSpellChecker;
using DevExpress.XtraSpellChecker.Native;
using DevExpress.Persistent.Base;
using System.Threading.Tasks;
using DevExpress.Web.ASPxSpellChecker;

namespace DevExpress.ExpressApp.SpellChecker.Web {
    public sealed partial class SpellCheckerAspNetModule: ModuleBase {
        public SpellCheckerAspNetModule() {
            InitializeComponent();
        }
    }
}
