Imports System
Imports System.IO
Imports System.Reflection
Imports System.Globalization
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraSpellChecker
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.XtraSpellChecker.Native

Namespace Spelling.Win
    Public NotInheritable Class SpellCheckerInitializer

        Private Sub New()
        End Sub


        Private Shared dictionary_Renamed As SpellCheckerISpellDictionary

        Private Shared customDictionary_Renamed As SpellCheckerCustomDictionary
        Public Shared Sub Initialize(ByVal spellCheckerCore As SpellChecker)
            dictionary_Renamed = New SpellCheckerISpellDictionary()
            spellCheckerCore.SpellCheckMode = SpellCheckMode.AsYouType
            spellCheckerCore.CheckAsYouTypeOptions.ShowSpellCheckForm = True
            spellCheckerCore.CheckAsYouTypeOptions.CheckControlsInParentContainer = True
            spellCheckerCore.Culture = New CultureInfo("en-US")
            spellCheckerCore.Dictionaries.Add(SpellCheckerInitializer.Dictionary)
            spellCheckerCore.Dictionaries.Add(SpellCheckerInitializer.CustomDictionary)
            'dictionary.AlphabetPath = "Dictionaries\\EnglishAlphabet.txt";
            'dictionary.DictionaryPath = "Dictionaries\\American.xlg";
            'dictionary.GrammarPath = "Dictionaries\\English.aff";
            Dim workAssembly As System.Reflection.Assembly = GetType(SpellCheckerInitializer).Assembly
            Dim affStream As Stream = workAssembly.GetManifestResourceStream("Dictionaries.English.aff")
            Dim dicStream As Stream = workAssembly.GetManifestResourceStream("Dictionaries.American.xlg")
            Dim alphStream As Stream = workAssembly.GetManifestResourceStream("Dictionaries.EnglishAlphabet.txt")
            dictionary_Renamed.LoadFromStream(dicStream, affStream, alphStream)
            dictionary_Renamed.CaseSensitive = False
            dictionary_Renamed.Culture = New CultureInfo("en-US")
            'dictionary.Encoding = Encoding.GetEncoding(1252);
            'dictionary.Load();

            customDictionary_Renamed = New SpellCheckerCustomDictionary()
            'customDictionary.AlphabetPath = dictionary.AlphabetPath;
            customDictionary_Renamed.FillAlphabetFromStream(workAssembly.GetManifestResourceStream("Dictionaries.English.aff"))
            'customDictionary.DictionaryPath = "Dictionaries\\Custom.txt";
            customDictionary_Renamed.LoadFromStream(workAssembly.GetManifestResourceStream("Dictionaries.Custom.txt"))
            customDictionary_Renamed.CaseSensitive = False
            customDictionary_Renamed.Culture = dictionary_Renamed.Culture
            'customDictionary.Encoding = dictionary.Encoding;
            'customDictionary.Load();

            SpellCheckTextControllersManager.Default.RegisterClass(GetType(StringEdit), GetType(SimpleTextEditTextController))
            SpellCheckTextControllersManager.Default.RegisterClass(GetType(LargeStringEdit), GetType(SimpleTextEditTextController))
            SpellCheckTextControllersManager.Default.RegisterClass(GetType(MemoEdit), GetType(SimpleTextEditTextController))

            SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(GetType(StringEdit), GetType(StringEditTextBoxFinder))
            SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(GetType(LargeStringEdit), GetType(LargeStringEditTextBoxFinder))
        End Sub
        Public Shared ReadOnly Property Dictionary() As SpellCheckerISpellDictionary
            Get
                Return dictionary_Renamed
            End Get
        End Property
        Public Shared ReadOnly Property CustomDictionary() As SpellCheckerCustomDictionary
            Get
                Return customDictionary_Renamed
            End Get
        End Property
    End Class
    Public Class LargeStringEditTextBoxFinder
        Inherits MemoEditTextBoxFinder

        Public Overrides Function GetTextBoxInstance(ByVal editControl As Control) As TextBoxBase
            If TypeOf editControl Is LargeStringEdit Then
                Return MyBase.GetTextBoxInstance(CType(editControl, MemoEdit))
            End If
            Return Nothing
        End Function
    End Class
    Public Class StringEditTextBoxFinder
        Inherits TextEditTextBoxFinder

        Public Overrides Function GetTextBoxInstance(ByVal editControl As Control) As TextBoxBase
            If TypeOf editControl Is StringEdit Then
                Return MyBase.GetTextBoxInstance(CType(editControl, TextEdit))
            End If
            Return Nothing
        End Function
    End Class
End Namespace