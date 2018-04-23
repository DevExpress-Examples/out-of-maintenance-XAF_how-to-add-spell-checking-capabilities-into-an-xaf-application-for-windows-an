using System;
using System.Windows.Forms;
using System.Globalization;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.ExpressApp.Win.Editors;

namespace MySpellCheckerModule {
    public partial class SpellCheckerController : ViewController {
        private SpellChecker spellCheckerCore;
        private SimpleAction checkActionCore;
        public SpellCheckerController() {
            checkActionCore = new SimpleAction(this, "Check", PredefinedCategory.RecordEdit);
            checkActionCore.Caption = checkActionCore.ToolTip = "Check Spelling";
            checkActionCore.Category = "RecordEdit";
            checkActionCore.ImageName = "BO_Task";
            checkActionCore.Execute += checkActionCore_Execute;
        }
        protected virtual void SetupSpellChecker() {
            spellCheckerCore = new SpellChecker();
            spellCheckerCore.SpellCheckMode = SpellCheckMode.AsYouType;
            spellCheckerCore.CheckAsYouTypeOptions.ShowSpellCheckForm = false;
            spellCheckerCore.CheckAsYouTypeOptions.CheckControlsInParentContainer = true;
            spellCheckerCore.Culture = new CultureInfo("en-US");
            spellCheckerCore.Dictionaries.Add(SpellCheckerHelper.Dictionary);
            spellCheckerCore.Dictionaries.Add(SpellCheckerHelper.CustomDictionary);
            spellCheckerCore.BeforeCheck += spellCheckerCore_BeforeCheck;
            if (spellCheckerCore.ParentContainer == null && View.IsControlCreated) {
                Control control = (Control)View.Control;
                if (control.IsHandleCreated) {
                    SetupSpellCheckerCore(control);
                }
                else {
                    control.HandleCreated += delegate(object sender, EventArgs args) {
                        SetupSpellCheckerCore(control);
                    };
                }
            }
        }
        protected virtual void SetupSpellCheckerCore(Control control) {
            spellCheckerCore.ParentContainer = control.FindForm();
        }
        protected override void OnDeactivating() {
            base.OnDeactivating();
            spellCheckerCore.BeforeCheck -= spellCheckerCore_BeforeCheck;
        }
        private void spellCheckerCore_BeforeCheck(object sender, BeforeCheckEventArgs e) {
            e.Cancel = !(e.EditControl as Control).IsHandleCreated;
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            SetupSpellChecker();
            if (View is DevExpress.ExpressApp.ListView) {
                DevExpress.ExpressApp.ListView lv = (DevExpress.ExpressApp.ListView)View;
                GridListEditor gridListEditor = lv.Editor as GridListEditor;
                if (gridListEditor != null) {
                    gridListEditor.GridView.ShownEditor += GridView_ShownEditor;
                }
            }
        }
        private void GridView_ShownEditor(object sender, EventArgs e) {
            if (spellCheckerCore != null) {
                Control control = (sender as GridView).ActiveEditor;
                spellCheckerCore.SetShowSpellCheckMenu(control, true);
                spellCheckerCore.Check(control);
            }
        }
        private void checkActionCore_Execute(object sender, SimpleActionExecuteEventArgs e) {
            Check(e);
        }
        protected virtual void Check(SimpleActionExecuteEventArgs e) {
            spellCheckerCore.CheckContainer(View.Control as Control);
        }
        public SpellChecker SpellChecker {
            get { return spellCheckerCore; }
        }
        public SimpleAction CheckAction {
            get { return checkActionCore; }
        }
    }
}