using System.ComponentModel;
using DevExpress.ExpressApp.Model;

namespace DevExpress.ExpressApp.SpellChecker {
    public sealed partial class SpellCheckerModule : ModuleBase {
        public SpellCheckerModule() {
            InitializeComponent();
        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelOptions, IModelOptionsSpellChecker>();
        }
    }
    public interface IModelOptionsSpellChecker : IModelNode {
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelOptionsSpellChecker,SpellChecker")]
        IModelSpellChecker SpellChecker { get; }
    }
    public interface IModelSpellChecker : IModelNode {
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,Enabled")]
        [Category("Behavior")]
        [DefaultValue(true)]
        bool Enabled { get; set; }
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,AlphabetPath")]
        [Category("Data")]
        [Localizable(true)]
        [DefaultValue("Dictionaries\\EnglishAlphabet.txt")]
        string AlphabetPath { get; set; }
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,GrammarPath")]
        [Category("Data")]
        [DefaultValue("Dictionaries\\English.aff")]
        [Localizable(true)]
        string GrammarPath { get; set; }
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,DefaultDictionaryPath")]
        [Category("Data")]
        [DefaultValue("Dictionaries\\American.xlg")]
        [Localizable(true)]
        string DefaultDictionaryPath { get; set; }
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,CustomDictionaryPath")]
        [Category("Data")]
        [DefaultValue("Dictionaries\\Custom.txt")]
        [Localizable(true)]
        string CustomDictionaryPath { get; set; }
        [DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,DefaultDictionaryPathResolution")]
        [Category("Behavior")]
        [DefaultValue(FilePathResolutionMode.RelativeToApplicationFolder)]
        FilePathResolutionMode PathResolutionMode { get; set; }
    }
    public enum FilePathResolutionMode {
        //TODO Dennis: Consider introducing an option allowing English spell checking without doing anything by developers.
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        None,
        Absolute,
        RelativeToApplicationFolder,
    }
}
