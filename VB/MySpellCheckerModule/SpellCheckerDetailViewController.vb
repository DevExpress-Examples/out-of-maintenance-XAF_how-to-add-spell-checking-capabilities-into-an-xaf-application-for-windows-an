Imports System
Imports System.Windows.Forms
Imports System.Globalization
Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.XtraSpellChecker
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.ExpressApp.Win.Editors

Namespace MySpellCheckerModule
    Partial Public Class SpellCheckerController
        Inherits ViewController
        Private spellCheckerCore As SpellChecker
        Private checkActionCore As SimpleAction
        Public Sub New()
            checkActionCore = New SimpleAction(Me, "Check", PredefinedCategory.RecordEdit)
            checkActionCore.ToolTip = "Check Spelling"
            checkActionCore.Caption = checkActionCore.ToolTip
            checkActionCore.Category = "RecordEdit"
            checkActionCore.ImageName = "BO_Task"
            AddHandler checkActionCore.Execute, AddressOf checkActionCore_Execute
        End Sub
        Protected Overridable Sub SetupSpellChecker()
            spellCheckerCore = New SpellChecker()
            spellCheckerCore.SpellCheckMode = SpellCheckMode.AsYouType
            spellCheckerCore.CheckAsYouTypeOptions.ShowSpellCheckForm = False
            spellCheckerCore.CheckAsYouTypeOptions.CheckControlsInParentContainer = True
            spellCheckerCore.Culture = New CultureInfo("en-US")
            spellCheckerCore.Dictionaries.Add(SpellCheckerHelper.Dictionary)
            spellCheckerCore.Dictionaries.Add(SpellCheckerHelper.CustomDictionary)
            AddHandler spellCheckerCore.BeforeCheck, AddressOf spellCheckerCore_BeforeCheck
            If spellCheckerCore.ParentContainer Is Nothing AndAlso View.IsControlCreated Then
                Dim control As Control = CType(View.Control, Control)
                If control.IsHandleCreated Then
                    SetupSpellCheckerCore(control)
                Else
                    AddHandler control.HandleCreated, AddressOf control_HandleCreated
                End If
            End If
        End Sub
        Private Sub control_HandleCreated(ByVal sender As Object, ByVal args As EventArgs)
            Dim control As Control = CType(sender, Control)
            RemoveHandler control.HandleCreated, AddressOf control_HandleCreated
            SetupSpellCheckerCore(control)
        End Sub
        Protected Overridable Sub SetupSpellCheckerCore(ByVal control As Control)
            spellCheckerCore.ParentContainer = control.FindForm()
        End Sub
        Protected Overrides Sub OnDeactivating()
            MyBase.OnDeactivating()
            RemoveHandler spellCheckerCore.BeforeCheck, AddressOf spellCheckerCore_BeforeCheck
        End Sub
        Private Sub spellCheckerCore_BeforeCheck(ByVal sender As Object, ByVal e As BeforeCheckEventArgs)
            e.Cancel = Not (TryCast(e.EditControl, Control)).IsHandleCreated
        End Sub
        Protected Overrides Sub OnViewControlsCreated()
            MyBase.OnViewControlsCreated()
            SetupSpellChecker()
            If TypeOf View Is DevExpress.ExpressApp.ListView Then
                Dim lv As DevExpress.ExpressApp.ListView = CType(View, DevExpress.ExpressApp.ListView)
                Dim gridListEditor As GridListEditor = TryCast(lv.Editor, GridListEditor)
                If gridListEditor IsNot Nothing Then
                    AddHandler gridListEditor.GridView.ShownEditor, AddressOf GridView_ShownEditor
                End If
            End If
        End Sub
        Private Sub GridView_ShownEditor(ByVal sender As Object, ByVal e As EventArgs)
            If spellCheckerCore IsNot Nothing Then
                Dim control As Control = (TryCast(sender, GridView)).ActiveEditor
                spellCheckerCore.SetShowSpellCheckMenu(control, True)
                spellCheckerCore.Check(control)
            End If
        End Sub
        Private Sub checkActionCore_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
            Check(e)
        End Sub
        Protected Overridable Sub Check(ByVal e As SimpleActionExecuteEventArgs)
            spellCheckerCore.CheckContainer(TryCast(View.Control, Control))
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