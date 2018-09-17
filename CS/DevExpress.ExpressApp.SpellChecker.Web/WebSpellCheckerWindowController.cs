using System;
using System.Linq;
using System.Web.UI;
using System.Globalization;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Templates;
using DevExpress.Web.ASPxSpellChecker;
using DevExpress.ExpressApp.Web.Controls;

namespace DevExpress.ExpressApp.SpellChecker.Web {
    public partial class WebSpellCheckerWindowController : SpellCheckerWindowController {
        private const string ActiveKeyDetailViewEditMode = "DetailView.ViewEditMode is Edit";
        protected static Dictionary<ISpellCheckerDictionary, WebDictionary> spellCheckerToWebDictionaryMap = new Dictionary<ISpellCheckerDictionary, WebDictionary>();
        public const string SpellCheckerClientInstanceName = "xaf_spellChecker";
        protected override Object CreateSpellCheckerComponent(object template) {
            DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker result = null;
            Page page = template as Page;
            if(page != null) {
                string controlId = SpellCheckerClientInstanceName;
                result = page.FindControl(controlId) as DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker;
                if(result == null) {
                    result = new DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker();
                    result.ID = result.ClientInstanceName = controlId;
                    result.ShowLoadingPanel = false;
                    result.Culture = CultureInfo.CurrentUICulture;
                    //Dennis: The Page.Form property is not yet initialized at this moment, while the ASPxSpellChecker must be added into the Form element only.
                    Control form = WebWindow.GetForm(page);
                    if(form != null) {
                        form.Controls.Add(result);
                    }
                }
            }
            return result;
        }
        public new DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker SpellCheckerComponent {
            get { return base.SpellCheckerComponent as DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker; }
            private set { base.SpellCheckerComponent = value; }
        }
        //TODO Dennis: Ask the team to make the ASPxSpellChecker.SpellChecker property public in the next version.
		protected override SpellCheckerBase SpellChecker {
            get { return SpellCheckerComponent.GetType().GetProperty("SpellChecker", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty).GetValue(SpellCheckerComponent, null) as SpellCheckerBase; }
        }
        public override void CheckSpelling() {
            IViewSiteTemplate page = Frame.Template as IViewSiteTemplate;
            if(page != null) {
                WebWindow.CurrentRequestWindow.RegisterStartupScript(GetType().Name,
                    string.Format("{0}.CheckElementsInContainer({1});",
                        SpellCheckerClientInstanceName,
                        page.ViewSiteControl is ViewSiteControl ?
                            string.Format("window.document.getElementById('{0}')", ((ViewSiteControl)page.ViewSiteControl).Control.ClientID) :
                            "window.document"
                    )
                );
            }
        }
        protected override void ActivateDictionaries() {
            SpellCheckerComponent.Dictionaries.Add(spellCheckerToWebDictionaryMap[DefaultDictionary]);
            SpellCheckerComponent.Dictionaries.Add(spellCheckerToWebDictionaryMap[CustomDictionary]);
        }
        protected override SpellCheckerISpellDictionary CreateDefaultDictionaryCore() {
            ASPxSpellCheckerISpellDictionary defaultWebDictionary = new ASPxSpellCheckerISpellDictionary();
            defaultWebDictionary.CacheKey = defaultWebDictionary.GetType().Name;
            spellCheckerToWebDictionaryMap.Add(defaultWebDictionary.Dictionary, defaultWebDictionary);
            return defaultWebDictionary.Dictionary;
        }
        protected override SpellCheckerCustomDictionary CreateCustomDictionaryCore() {
            ASPxSpellCheckerCustomDictionary customWebDictionary = new ASPxSpellCheckerCustomDictionary();
            customWebDictionary.CacheKey = customWebDictionary.GetType().Name;
            spellCheckerToWebDictionaryMap.Add(customWebDictionary.Dictionary, customWebDictionary);
            return customWebDictionary.Dictionary;
        }
        //protected override void OnWindowViewChanged(View view) {
        //    base.OnWindowViewChanged(view);
        //    DetailView dv = Window.View as DetailView;
        //    if((dv != null) && (dv.ViewEditMode == Editors.ViewEditMode.View)) {
        //        CheckSpellingAction.Active[ActiveKeyDetailViewEditMode] = false;
        //    }
        //}
    }
}