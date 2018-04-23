using System;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.XtraSpellChecker;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.ExpressApp.Win.Editors;

namespace Spelling.Win {
    public class SpellCheckerViewController : ViewController {
        private SpellChecker spellCheckerCore;
        private SimpleAction checkActionCore;
        public SpellCheckerViewController() {
            checkActionCore = new SimpleAction(this, "Check", PredefinedCategory.RecordEdit);
            checkActionCore.Caption = checkActionCore.ToolTip = "Check Spelling";
            checkActionCore.Category = "RecordEdit";
            checkActionCore.ImageName = "BO_Task";
            checkActionCore.TargetViewType = ViewType.DetailView;
            checkActionCore.Execute += checkActionCore_Execute;
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            SetupSpellChecker();
            SetupGridView();
        }
        protected override void OnDeactivated() {
            checkActionCore.Execute -= checkActionCore_Execute;
            spellCheckerCore = null;
            checkActionCore = null;
            base.OnDeactivated();
        }
        protected virtual void SetupSpellChecker() {
            spellCheckerCore = new SpellChecker();
            if (spellCheckerCore.ParentContainer == null) {
                spellCheckerCore.ParentContainer = (Control)View.Control;
            }
            SpellCheckerInitializer.Initialize(spellCheckerCore);
        }
        protected virtual void SetupGridView() {
            if (View is DevExpress.ExpressApp.ListView) {
                DevExpress.ExpressApp.ListView lv = (DevExpress.ExpressApp.ListView)View;
                GridListEditor gridListEditor = lv.Editor as GridListEditor;
                if (gridListEditor != null) {
                    gridListEditor.GridView.ShownEditor -= GridView_ShownEditor;
                    gridListEditor.GridView.ShownEditor += GridView_ShownEditor;
                    gridListEditor.GridView.Disposed += GridView_Disposed;
                }
            }
        }
        void GridView_Disposed(object sender, EventArgs e) {
            GridView gv = (GridView)sender;
            gv.ShownEditor -= GridView_ShownEditor;
            gv.Disposed -= GridView_Disposed;
        }
        private void GridView_ShownEditor(object sender, EventArgs e) {
            Control activeControl = ((GridView)sender).ActiveEditor;
            spellCheckerCore.SetShowSpellCheckMenu(activeControl, true);
            if (spellCheckerCore.SpellCheckMode == SpellCheckMode.AsYouType) {
                //spellCheckerCore.Check(activeControl);
            }
        }
        private void checkActionCore_Execute(object sender, SimpleActionExecuteEventArgs e) {
            spellCheckerCore.CheckContainer();
        }
        public SpellChecker SpellChecker {
            get { return spellCheckerCore; }
        }
        public SimpleAction CheckAction {
            get { return checkActionCore; }
        }
    }
}