using System;
using System.ComponentModel;
using DevExpress.XtraSpellChecker;

namespace DevExpress.ExpressApp.SpellChecker {
    /// <summary>
    /// Event arguments for the SpellCheckerWindowController.QueryCanCheckSpelling event.
    /// </summary>
    public class QueryCanCheckSpellingEventArgs : CancelEventArgs {
        public QueryCanCheckSpellingEventArgs(TemplateContext context, object template) {
            this.Context = context;
            this.Template = template;
        }
        public TemplateContext Context { get; private set; }
        public object Template { get; private set; }
    }
    /// <summary>
    /// Event arguments for the SpellCheckerWindowController.SpellCheckerCreated event.
    /// </summary>
    public class SpellCheckerCreatedEventArgs : EventArgs {
        public SpellCheckerCreatedEventArgs(Object spellChecker) {
            this.SpellChecker = spellChecker;
        }
        public Object SpellChecker { get; private set; }
    }
    /// <summary>
    /// Event arguments for the SpellCheckerWindowController.DictionaryCreated event.
    /// </summary>
    public class DictionaryCreatedEventArgs : EventArgs {
        public DictionaryCreatedEventArgs(ISpellCheckerDictionary dictionary, bool isCustom) {
            this.Dictionary = dictionary;
            this.IsCustom = isCustom;
        }
        public ISpellCheckerDictionary Dictionary { get; set; }
        public bool IsCustom { get; private set; }
    }
 
}
