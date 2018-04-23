using System;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Win.Editors;

namespace MySpellCheckerModule {
    public class LargeStringEditTextBoxFinder : DevExpress.XtraSpellChecker.Native.MemoEditTextBoxFinder {
        public override TextBoxBase GetTextBoxInstance(Control editControl) {
            if (editControl is LargeStringEdit)
                return base.GetTextBoxInstance((MemoEdit)editControl);
            return null;
        }
    }
    public class StringEditTextBoxFinder : DevExpress.XtraSpellChecker.Native.TextEditTextBoxFinder {
        public override TextBoxBase GetTextBoxInstance(Control editControl) {
            if (editControl is StringEdit)
                return base.GetTextBoxInstance((TextEdit)editControl);
            return null;
        }
    }
    public static class SpellCheckerHelper {
        private static SpellCheckerISpellDictionary dictionary;
        private static SpellCheckerCustomDictionary customDictionary;
        static SpellCheckerHelper() {
            dictionary = new SpellCheckerISpellDictionary();
            dictionary.AlphabetPath = "Dictionaries\\EnglishAlphabet.txt";
            dictionary.DictionaryPath = "Dictionaries\\American.xlg";
            dictionary.GrammarPath = "Dictionaries\\English.aff";
            dictionary.CaseSensitive = false;
            dictionary.Culture = new System.Globalization.CultureInfo("en-US");
            dictionary.Encoding = Encoding.GetEncoding(1252);
            dictionary.Load();
            customDictionary = new SpellCheckerCustomDictionary();
            customDictionary.AlphabetPath = dictionary.AlphabetPath;
            customDictionary.DictionaryPath = "Dictionaries\\Custom.txt";
            customDictionary.CaseSensitive = false;
            customDictionary.Culture = dictionary.Culture;
            customDictionary.Encoding = dictionary.Encoding;
            customDictionary.Load();
            DevExpress.XtraSpellChecker.Native.SpellCheckTextControllersManager.Default.RegisterClass(
                typeof(StringEdit), typeof(DevExpress.XtraSpellChecker.Native.SimpleTextEditTextController));
            DevExpress.XtraSpellChecker.Native.SpellCheckTextControllersManager.Default.RegisterClass(
                typeof(LargeStringEdit), typeof(DevExpress.XtraSpellChecker.Native.SimpleTextEditTextController));
            DevExpress.XtraSpellChecker.Native.SpellCheckTextControllersManager.Default.RegisterClass(
                typeof(MemoEdit), typeof(DevExpress.XtraSpellChecker.Native.SimpleTextEditTextController));
            DevExpress.XtraSpellChecker.Native.SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(
                typeof(StringEdit), typeof(StringEditTextBoxFinder));
            DevExpress.XtraSpellChecker.Native.SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(
                typeof(LargeStringEdit), typeof(LargeStringEditTextBoxFinder));
        }
        public static SpellCheckerISpellDictionary Dictionary {
            get { return dictionary; }
        }
        public static SpellCheckerCustomDictionary CustomDictionary {
            get { return customDictionary; }
        }
    }
}
