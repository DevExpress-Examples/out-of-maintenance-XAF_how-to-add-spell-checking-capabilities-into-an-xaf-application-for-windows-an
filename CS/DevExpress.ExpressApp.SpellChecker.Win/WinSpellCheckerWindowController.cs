using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraSpellChecker;

namespace DevExpress.ExpressApp.SpellChecker.Win {
    public partial class WinSpellCheckerWindowController : SpellCheckerWindowController {
        protected static SharedDictionaryStorage dictionariesStorage;
        protected override object CreateSpellCheckerComponent(object parentContainer) {
            var result = new DevExpress.XtraSpellChecker.SpellChecker();
			result.ParentContainer = (Form)parentContainer;
            result.SpellCheckMode = SpellCheckMode.AsYouType;
            result.CheckAsYouTypeOptions.ShowSpellCheckForm = true;
            result.CheckAsYouTypeOptions.CheckControlsInParentContainer = true;
            result.Culture = CultureInfo.CurrentUICulture;
            result.UseSharedDictionaries = true;
            return result;
        }
        public new DevExpress.XtraSpellChecker.SpellChecker SpellCheckerComponent {
            get { return base.SpellCheckerComponent as DevExpress.XtraSpellChecker.SpellChecker; }
            private set { base.SpellCheckerComponent = value; }
        }
		protected override SpellCheckerBase SpellChecker {
            get { return SpellCheckerComponent; }
        }
        public override void CheckSpelling() {
            if(SpellCheckerComponent != null) {
                SpellCheckerComponent.CheckContainer();
            }
        }
		protected override void UpdateCheckSpellingAction(Window window) {
			base.UpdateCheckSpellingAction(window);
			CheckSpellingAction.Active["IsMain"] = !window.IsMain;
		}
        protected override void ActivateDictionaries() {
            if(dictionariesStorage == null) {
                dictionariesStorage = new SharedDictionaryStorage();
                dictionariesStorage.Dictionaries.Add(DefaultDictionary);
                dictionariesStorage.Dictionaries.Add(CustomDictionary);
            }
            SpellCheckerComponent.Dictionaries.AddRange(dictionariesStorage.Dictionaries);
        }
        protected override SpellCheckerISpellDictionary CreateDefaultDictionaryCore() {
            return new SpellCheckerISpellDictionary();
        }
        protected override SpellCheckerCustomDictionary CreateCustomDictionaryCore() {
            return new SpellCheckerCustomDictionary();
        }
        //protected override void OnWindowViewChanged(View view) {
        //    base.OnWindowViewChanged(view);
        //    if(view is ListView) {
        //        CheckSpellingAction.Active[ActiveKeyWindowView] = false;
        //    }
        //}
        //protected override void OnQueryCanCheckSpelling(QueryCanCheckSpellingEventArgs args) {
        //    base.OnQueryCanCheckSpelling(args);
        //    Form form = args.Template as Form;
        //    if(form != null) {
        //        args.Cancel &= form.IsMdiChild;
        //    }
        //}
    }
}