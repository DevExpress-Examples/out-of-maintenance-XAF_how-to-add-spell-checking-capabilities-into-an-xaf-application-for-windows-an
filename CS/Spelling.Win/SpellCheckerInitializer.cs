using System;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraSpellChecker.Native;

namespace Spelling.Win {
    public static class SpellCheckerInitializer {
        private static SpellCheckerISpellDictionary dictionary;
        private static SpellCheckerCustomDictionary customDictionary;
        public static void Initialize(SpellChecker spellCheckerCore) {
            dictionary = new SpellCheckerISpellDictionary();
            spellCheckerCore.SpellCheckMode = SpellCheckMode.AsYouType;
            spellCheckerCore.CheckAsYouTypeOptions.ShowSpellCheckForm = true;
            spellCheckerCore.CheckAsYouTypeOptions.CheckControlsInParentContainer = true;
            spellCheckerCore.Culture = new CultureInfo("en-US");
            spellCheckerCore.Dictionaries.Add(SpellCheckerInitializer.Dictionary);
            spellCheckerCore.Dictionaries.Add(SpellCheckerInitializer.CustomDictionary);
            //dictionary.AlphabetPath = "Dictionaries\\EnglishAlphabet.txt";
            //dictionary.DictionaryPath = "Dictionaries\\American.xlg";
            //dictionary.GrammarPath = "Dictionaries\\English.aff";
            Assembly workAssembly = typeof(SpellCheckerInitializer).Assembly;
            Stream affStream = workAssembly.GetManifestResourceStream("Spelling.Win.Dictionaries.English.aff");
            Stream dicStream = workAssembly.GetManifestResourceStream("Spelling.Win.Dictionaries.American.xlg");
            Stream alphStream = workAssembly.GetManifestResourceStream("Spelling.Win.Dictionaries.EnglishAlphabet.txt");
            dictionary.LoadFromStream(dicStream, affStream, alphStream);
            dictionary.CaseSensitive = false;
            dictionary.Culture = new CultureInfo("en-US");
            //dictionary.Encoding = Encoding.GetEncoding(1252);
            //dictionary.Load();

            customDictionary = new SpellCheckerCustomDictionary();
            //customDictionary.AlphabetPath = dictionary.AlphabetPath;
            customDictionary.FillAlphabetFromStream(workAssembly.GetManifestResourceStream("Spelling.Win.Dictionaries.English.aff"));
            //customDictionary.DictionaryPath = "Dictionaries\\Custom.txt";
            customDictionary.LoadFromStream(workAssembly.GetManifestResourceStream("Spelling.Win.Dictionaries.Custom.txt"));
            customDictionary.CaseSensitive = false;
            customDictionary.Culture = dictionary.Culture;
            //customDictionary.Encoding = dictionary.Encoding;
            //customDictionary.Load();

            SpellCheckTextControllersManager.Default.RegisterClass(
                typeof(StringEdit), typeof(SimpleTextEditTextController));
            SpellCheckTextControllersManager.Default.RegisterClass(
                typeof(LargeStringEdit), typeof(SimpleTextEditTextController));
            SpellCheckTextControllersManager.Default.RegisterClass(
                typeof(MemoEdit), typeof(SimpleTextEditTextController));

            SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(
                typeof(StringEdit), typeof(StringEditTextBoxFinder));
            SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(
                typeof(LargeStringEdit), typeof(LargeStringEditTextBoxFinder));
        }
        public static SpellCheckerISpellDictionary Dictionary {
            get { return dictionary; }
        }
        public static SpellCheckerCustomDictionary CustomDictionary {
            get { return customDictionary; }
        }
    }
    public class LargeStringEditTextBoxFinder : MemoEditTextBoxFinder {
        public override TextBoxBase GetTextBoxInstance(Control editControl) {
            if (editControl is LargeStringEdit)
                return base.GetTextBoxInstance((MemoEdit)editControl);
            return null;
        }
    }
    public class StringEditTextBoxFinder : TextEditTextBoxFinder {
        public override TextBoxBase GetTextBoxInstance(Control editControl) {
            if (editControl is StringEdit)
                return base.GetTextBoxInstance((TextEdit)editControl);
            return null;
        }
    }
}