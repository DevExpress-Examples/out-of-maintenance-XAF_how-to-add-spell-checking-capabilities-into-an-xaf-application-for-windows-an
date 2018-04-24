Imports System
Imports System.Windows.Forms
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.XtraSpellChecker
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.ExpressApp.Win.Editors

Namespace Spelling.Win
    Public Class SpellCheckerViewController
        Inherits ViewController

        Private spellCheckerCore As SpellChecker
        Private checkActionCore As SimpleAction
        Public Sub New()
            checkActionCore = New SimpleAction(Me, "Check", PredefinedCategory.RecordEdit)
            checkActionCore.ToolTip = "Check Spelling"
            checkActionCore.Caption = checkActionCore.ToolTip
            checkActionCore.Category = "RecordEdit"
            checkActionCore.ImageName = "BO_Task"
            checkActionCore.TargetViewType = ViewType.DetailView
            AddHandler checkActionCore.Execute, AddressOf checkActionCore_Execute
        End Sub
        Protected Overrides Sub OnViewControlsCreated()
            MyBase.OnViewControlsCreated()
            SetupSpellChecker()
            SetupGridView()
        End Sub
        Protected Overrides Sub OnDeactivated()
            RemoveHandler checkActionCore.Execute, AddressOf checkActionCore_Execute
            spellCheckerCore = Nothing
            checkActionCore = Nothing
            MyBase.OnDeactivated()
        End Sub
        Protected Overridable Sub SetupSpellChecker()
            spellCheckerCore = New SpellChecker()
            If spellCheckerCore.ParentContainer Is Nothing Then
                spellCheckerCore.ParentContainer = CType(View.Control, Control)
            End If
            SpellCheckerInitializer.Initialize(spellCheckerCore)
        End Sub
        Protected Overridable Sub SetupGridView()
            If TypeOf View Is DevExpress.ExpressApp.ListView Then
                Dim lv As DevExpress.ExpressApp.ListView = CType(View, DevExpress.ExpressApp.ListView)
                Dim gridListEditor As GridListEditor = TryCast(lv.Editor, GridListEditor)
                If gridListEditor IsNot Nothing Then
                    RemoveHandler gridListEditor.GridView.ShownEditor, AddressOf GridView_ShownEditor
                    AddHandler gridListEditor.GridView.ShownEditor, AddressOf GridView_ShownEditor
                    AddHandler gridListEditor.GridView.Disposed, AddressOf GridView_Disposed
                End If
            End If
        End Sub
        Private Sub GridView_Disposed(ByVal sender As Object, ByVal e As EventArgs)
            Dim gv As GridView = DirectCast(sender, GridView)
            RemoveHandler gv.ShownEditor, AddressOf GridView_ShownEditor
            RemoveHandler gv.Disposed, AddressOf GridView_Disposed
        End Sub
        Private Sub GridView_ShownEditor(ByVal sender As Object, ByVal e As EventArgs)
            Dim activeControl As Control = DirectCast(sender, GridView).ActiveEditor
            spellCheckerCore.SetShowSpellCheckMenu(activeControl, True)
            If spellCheckerCore.SpellCheckMode = SpellCheckMode.AsYouType Then
                'spellCheckerCore.Check(activeControl);
            End If
        End Sub
        Private Sub checkActionCore_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
            spellCheckerCore.CheckContainer()
        End Sub
        Public ReadOnly Property SpellChecker() As SpellChecker
            Get
                Return spellCheckerCore
            End Get
        End Property
        Public ReadOnly Property CheckAction() As SimpleAction
            Get
                Return checkActionCore
            End Get
        End Property
    End Class
End Namespace