using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Actions;

namespace DevExpress.ExpressApp.SpellChecker {
    public abstract class SpellCheckerWindowController : WindowController {
        private static object lockObject = new object();
        private const string ActiveKeyCanCheckSpelling = "CanCheckSpelling";
        public SpellCheckerWindowController() {
            this.TargetWindowType = WindowType.Child;
            CheckSpellingAction = new SimpleAction(this, "CheckSpelling", PredefinedCategory.RecordEdit);
            CheckSpellingAction.Caption = "Check Spelling";
            CheckSpellingAction.ToolTip = "Check the spelling and grammar of text in the form.";
            CheckSpellingAction.Category = PredefinedCategory.RecordEdit.ToString();
            CheckSpellingAction.ImageName = "Action_SpellChecker";
            CheckSpellingAction.TargetViewType = ViewType.DetailView;
            CheckSpellingAction.Execute += checkSpellingAction_Execute;
        }
        private void checkSpellingAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            CheckSpelling();
        }
        protected override void OnActivated() {
            base.OnActivated();
            Window.TemplateChanged += Window_TemplateChanged;
        }
        protected override void OnDeactivated() {
            Window.TemplateChanged -= Window_TemplateChanged;
            ReleaseSpellChecker();
            base.OnDeactivated();
        }
        private void ReleaseSpellChecker() {
            if(SpellCheckerComponent is IDisposable) {
                ((IDisposable)SpellCheckerComponent).Dispose();
                SpellCheckerComponent = null;
            }
        }
        void Window_TemplateChanged(object sender, EventArgs e) {
            Window window = (Window)sender;
            if(window.Template == null) {
                ReleaseSpellChecker();
            }
            UpdateCheckSpellingAction(window);
        }
		protected virtual void UpdateCheckSpellingAction(Window window) {
			bool isInitialized = InitializeSpellChecker(window);
			CheckSpellingAction.Active[ActiveKeyCanCheckSpelling] = isInitialized;
		}
        /// <summary>
        /// Creates and initializes a spell checker component for the specified parent container.
        /// </summary>
        /// <param name="template"></param>
        private bool InitializeSpellChecker(Window window) {
            bool success = false;
            if((SpellCheckerComponent == null) && CanCheckSpelling(window.Context, window.Template)) {
                SpellCheckerComponent = CreateSpellCheckerComponent(window.Template);
                if(SpellCheckerComponent != null) {
                    EnsureDictionaries();
                    ActivateDictionaries();
                    OnSpellCheckerCreated(new SpellCheckerCreatedEventArgs(SpellCheckerComponent));
                    success = true;
                }
            }
			return success;
        }
        /// <summary>
        /// Determines whether the specified form template is supposed to be spell checked.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual bool CanCheckSpelling(TemplateContext context, object template) {
            QueryCanCheckSpellingEventArgs args = new QueryCanCheckSpellingEventArgs(context, template);
            bool isTargetTemplate = (template != null) && (context == TemplateContext.ApplicationWindow ||
                                                           context == TemplateContext.PopupWindow ||
                                                           context == TemplateContext.View);
            args.Cancel = !(SpellCheckerOptions.Enabled && isTargetTemplate);
            OnQueryCanCheckSpelling(args);
            return !args.Cancel;
        }
        /// <summary>
        /// Initializes a default dictionary depending on the platform.
        /// </summary>
        /// <returns>SpellCheckerISpellDictionary</returns>
        protected abstract SpellCheckerISpellDictionary CreateDefaultDictionaryCore();
        /// <summary>
        /// Initializes a custom dictionary depending on the platform.
        /// </summary>
        /// <returns>SpellCheckerCustomDictionary</returns>
        protected abstract SpellCheckerCustomDictionary CreateCustomDictionaryCore();
        /// <summary>
        /// Allows end-users to spell check the current form.
        /// </summary>
        public abstract void CheckSpelling();
        /// <summary>
        /// Links dictionaries to the created spell checker component.
        /// </summary>
        protected abstract void ActivateDictionaries();
        /// <summary>
        /// Creates and initializes a spell checker component for each platform for the specified parent container.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        protected abstract Object CreateSpellCheckerComponent(object template);
        /// <summary>
        /// Initializes static default and custom dictionaries for the first time they are accessed in the application.
        /// </summary>
        private void EnsureDictionaries() {
            if(DefaultDictionary == null) {
                lock(lockObject) {
                    if(DefaultDictionary == null) {
                        SpellCheckerISpellDictionary result = CreateDefaultDictionaryCore();
                        SetupDefaultDictionary(result);
                        DictionaryCreatedEventArgs args = new DictionaryCreatedEventArgs(result, false);
                        OnDictionaryCreated(args);
                        DefaultDictionary = args.Dictionary as SpellCheckerISpellDictionary ?? result;
                    }
                }
            }
            if(CustomDictionary == null) {
                lock(lockObject) {
                    if(CustomDictionary == null) {
                        SpellCheckerCustomDictionary result = CreateCustomDictionaryCore();
                        SetupCustomDictionary(result);
                        DictionaryCreatedEventArgs args = new DictionaryCreatedEventArgs(result, true);
                        OnDictionaryCreated(args);
                        CustomDictionary = args.Dictionary as SpellCheckerCustomDictionary ?? result;
                    }
                }
            }
        }
        /// <summary>
        /// Configures the default dictionary settings.
        /// </summary>
        /// <param name="dictionary"></param>
        protected virtual void SetupDefaultDictionary(SpellCheckerISpellDictionary dictionary) {
            dictionary.Culture = SpellChecker.Culture;
            dictionary.Encoding = System.Text.Encoding.UTF8;
            if(SpellCheckerOptions.PathResolutionMode == FilePathResolutionMode.None) {
                SpellCheckerDictionaryStreamInfo streamInfo = GetDictionaryStreamInfo(false);
                try {
                    dictionary.LoadFromStream(streamInfo.DictionaryStream, streamInfo.GrammarStream, streamInfo.AlphabetStream);
                }
                finally {
                    if(streamInfo.AlphabetStream != null) {
                        streamInfo.AlphabetStream.Dispose();
                        streamInfo.AlphabetStream = null;
                    }
                    if(streamInfo.DictionaryStream != null) {
                        streamInfo.DictionaryStream.Dispose();
                        streamInfo.DictionaryStream = null;
                    }
                    if(streamInfo.GrammarStream != null) {
                        streamInfo.GrammarStream.Dispose();
                        streamInfo.GrammarStream = null;
                    }
                }
            }
            else {
                SpellCheckerDictionaryFileInfo fileInfo = GetDictionaryFileInfo(false);
                dictionary.AlphabetPath = fileInfo.AlphabetPath;
                dictionary.DictionaryPath = fileInfo.DictionaryPath;
                dictionary.GrammarPath = fileInfo.GrammarPath;
            }
        }
        /// <summary>
        /// Configures the custom dictionary settings.
        /// </summary>
        /// <param name="dictionary"></param>
        protected virtual void SetupCustomDictionary(SpellCheckerCustomDictionary dictionary) {
            dictionary.Encoding = System.Text.Encoding.UTF8;
            dictionary.Culture = SpellChecker.Culture;
            if(SpellCheckerOptions.PathResolutionMode == FilePathResolutionMode.None) {
                SpellCheckerDictionaryStreamInfo streamInfo = GetDictionaryStreamInfo(true);
                try {
                    dictionary.Load(streamInfo.DictionaryStream, streamInfo.AlphabetStream);
                }
                finally {
                    if(streamInfo.AlphabetStream != null) {
                        streamInfo.AlphabetStream.Dispose();
                        streamInfo.AlphabetStream = null;
                    }
                    if(streamInfo.DictionaryStream != null) {
                        streamInfo.DictionaryStream.Dispose();
                        streamInfo.DictionaryStream = null;
                    }
                }
            }
            else {
                SpellCheckerDictionaryFileInfo fileInfo = GetDictionaryFileInfo(true);
                dictionary.AlphabetPath = fileInfo.AlphabetPath;
                dictionary.DictionaryPath = fileInfo.DictionaryPath;
            }
        }
        protected virtual void OnDictionaryCreated(DictionaryCreatedEventArgs args) {
            if(DictionaryCreated != null) {
                DictionaryCreated(this, args);
            }
        }
        protected virtual void OnQueryCanCheckSpelling(QueryCanCheckSpellingEventArgs args) {
            if(QueryCanCheckSpelling != null) {
                QueryCanCheckSpelling(this, args);
            }
        }
        protected virtual void OnSpellCheckerCreated(SpellCheckerCreatedEventArgs args) {
            if(SpellCheckerCreated != null) {
                SpellCheckerCreated(this, args);
            }
        }

        /// <summary>
        /// Provides a structure containing basic dictionary file settings depending on the Options | SpellChecker node values in the Application Model.
        /// </summary>
        /// <param name="isCustom"></param>
        /// <returns></returns>
        protected virtual SpellCheckerDictionaryFileInfo GetDictionaryFileInfo(bool isCustom) {
            string filePathPrefix = string.Empty;
            if(SpellCheckerOptions.PathResolutionMode == FilePathResolutionMode.RelativeToApplicationFolder) {
                filePathPrefix = PathHelper.GetApplicationFolder(); ;
            }
            return new SpellCheckerDictionaryFileInfo(
                    filePathPrefix + SpellCheckerOptions.AlphabetPath,
                    filePathPrefix + (isCustom ? SpellCheckerOptions.CustomDictionaryPath : SpellCheckerOptions.DefaultDictionaryPath),
                    isCustom ? string.Empty : (filePathPrefix + SpellCheckerOptions.GrammarPath)
                );
        }

        /// <summary>
        /// Provides a structure containing basic dictionary stream settings.
        /// </summary>
        /// <param name="isCustom"></param>
        /// <returns></returns>
        //TODO Dennis: Consider re-using the DictionaryHelper.CreateEnglishDictionary method instead.
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual SpellCheckerDictionaryStreamInfo GetDictionaryStreamInfo(bool isCustom) {
            Assembly resourceAssembly = typeof(SpellCheckerBase).Assembly;
            string resourcePrefix = resourceAssembly.GetName().Name;
            try {
                string alphabeth = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                Stream alphabethStream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(alphabeth));
                Stream dictionaryStream = resourceAssembly.GetManifestResourceStream("DevExpress.XtraSpellChecker.Core.Dictionary.american.xlg");
                Stream grammarStream = resourceAssembly.GetManifestResourceStream("DevExpress.XtraSpellChecker.Core.Dictionary.english.aff");
                return new SpellCheckerDictionaryStreamInfo(
                    alphabethStream,
                    isCustom ? new FileStream(PathHelper.GetApplicationFolder() + SpellCheckerOptions.CustomDictionaryPath, FileMode.OpenOrCreate, FileAccess.ReadWrite) : dictionaryStream,
                    isCustom ? null : grammarStream
                );
            }
            catch(Exception ex) {
                throw new ArgumentException(string.Format("Cannot load a dictionary from the {0} assembly by the specified name. Make sure that the dictionary file's BuildAction is set to EmbeddedResource and its name includes the file extension.",
                    resourceAssembly.GetName().Name), ex
                );
            }
        }
        /// <summary>
        /// Provides access to the default dictionary.
        /// </summary>
        public static SpellCheckerISpellDictionary DefaultDictionary { get; private set; }
        /// <summary>
        /// Provides access to the custom dictionary.
        /// </summary>
        public static SpellCheckerCustomDictionary CustomDictionary { get; private set; }

        /// <summary>
        /// Provides access to the spell checker component.
        /// </summary>
        public Object SpellCheckerComponent { get; protected set; }
        /// <summary>
        /// Provides access to the underlying SpellCheckerBase object, the core of the spell checker component.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		protected abstract SpellCheckerBase SpellChecker { get; }
        /// <summary>
        /// Provides access to the Options | SpellChecker node values in the Application Model.
        /// </summary>
        public IModelSpellChecker SpellCheckerOptions { get { return ((IModelOptionsSpellChecker)Application.Model.Options).SpellChecker; } }
        /// <summary>
        /// Provides access to the Action that allows end-users to spell check the current form.
        /// </summary>
        public SimpleAction CheckSpellingAction { get; private set; }
        /// <summary>
        /// Allows to choose whether spell checking should be available for the specified form template.
        /// </summary>
        public EventHandler<QueryCanCheckSpellingEventArgs> QueryCanCheckSpelling;
        /// <summary>
        /// Allows to customize the spell checker component once it is created and initialized.
        /// </summary>
        public EventHandler<SpellCheckerCreatedEventArgs> SpellCheckerCreated;
        /// <summary>
        /// Provides to customize a created dictionary or provide a fully custom one.
        /// </summary>
        public EventHandler<DictionaryCreatedEventArgs> DictionaryCreated;
    }
    public struct SpellCheckerDictionaryFileInfo {
        public SpellCheckerDictionaryFileInfo(string alphabetPath, string dictionaryPath, string grammarPath) {
            AlphabetPath = alphabetPath;
            DictionaryPath = dictionaryPath;
            GrammarPath = grammarPath;
        }
        public string AlphabetPath;
        public string DictionaryPath;
        public string GrammarPath;
    }
    public struct SpellCheckerDictionaryStreamInfo {
        public SpellCheckerDictionaryStreamInfo(Stream alphabetStream, Stream dictionaryStream, Stream grammarStream) {
            AlphabetStream = alphabetStream;
            DictionaryStream = dictionaryStream;
            GrammarStream = grammarStream;
        }
        public Stream AlphabetStream;
        public Stream DictionaryStream;
        public Stream GrammarStream;
    }
}