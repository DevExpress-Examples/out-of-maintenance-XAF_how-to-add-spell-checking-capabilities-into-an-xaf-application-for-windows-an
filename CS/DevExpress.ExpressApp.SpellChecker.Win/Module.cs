using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.XtraEditors;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraSpellChecker.Native;
using DevExpress.Persistent.Base;
using System.Threading.Tasks;

namespace DevExpress.ExpressApp.SpellChecker.Win {
    public sealed partial class SpellCheckerWindowsFormsModule : ModuleBase {
        public SpellCheckerWindowsFormsModule() {
            InitializeComponent();
            RegisterSpellCheckerControlFinders();
        }
        private static void RegisterSpellCheckerControlFinders() {
            SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(typeof(StringEdit), typeof(StringEditTextBoxFinder));
            SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(typeof(LargeStringEdit), typeof(LargeStringEditTextBoxFinder));
 
            SpellCheckTextControllersManager.Default.RegisterClass(typeof(StringEdit), typeof(SimpleTextEditTextController));
            SpellCheckTextControllersManager.Default.RegisterClass(typeof(LargeStringEdit), typeof(SimpleTextEditTextController));
            SpellCheckTextControllersManager.Default.RegisterClass(typeof(MemoEdit), typeof(SimpleTextEditTextController));
            //Q418588
            //SpellCheckTextControllersManager.Default.RegisterClass(typeof(RichEditUserControl), typeof(RichEditSpellCheckController));
            //SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(typeof(RichEditUserControl), typeof(RichTextBoxFinder));
        }
    }
    public class LargeStringEditTextBoxFinder : MemoEditTextBoxFinder {
        public override TextBoxBase GetTextBoxInstance(Control editControl) {
            if(editControl is LargeStringEdit)
                return base.GetTextBoxInstance((MemoEdit)editControl);
            return null;
        }
    }
    public class StringEditTextBoxFinder : TextEditTextBoxFinder {
        public override TextBoxBase GetTextBoxInstance(Control editControl) {
            if(editControl is StringEdit)
                return base.GetTextBoxInstance((TextEdit)editControl);
            return null;
        }
    }
}
