Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.XtraSpellChecker

Namespace MySpellCheckerModule
	Public Class LargeStringEditTextBoxFinder
		Inherits DevExpress.XtraSpellChecker.Native.MemoEditTextBoxFinder
		Public Overrides Function GetTextBoxInstance(ByVal editControl As Control) As TextBoxBase
			If TypeOf editControl Is LargeStringEdit Then
				Return MyBase.GetTextBoxInstance(CType(editControl, MemoEdit))
			End If
			Return Nothing
		End Function
	End Class
	Public Class StringEditTextBoxFinder
		Inherits DevExpress.XtraSpellChecker.Native.TextEditTextBoxFinder
		Public Overrides Function GetTextBoxInstance(ByVal editControl As Control) As TextBoxBase
			If TypeOf editControl Is StringEdit Then
				Return MyBase.GetTextBoxInstance(CType(editControl, TextEdit))
			End If
			Return Nothing
		End Function
	End Class
	Public NotInheritable Class SpellCheckerHelper
		Private Shared dictionary_Renamed As SpellCheckerISpellDictionary = Nothing
		Private Shared customDictionary_Renamed As SpellCheckerCustomDictionary = Nothing
		Private Sub New()
		End Sub
		Shared Sub New()
			dictionary_Renamed = New SpellCheckerISpellDictionary()
			dictionary_Renamed.AlphabetPath = "Dictionaries\EnglishAlphabet.txt"
			dictionary_Renamed.DictionaryPath = "Dictionaries\American.xlg"
			dictionary_Renamed.GrammarPath = "Dictionaries\English.aff"
			dictionary_Renamed.CaseSensitive = False
			dictionary_Renamed.Culture = New System.Globalization.CultureInfo("en-US")
			dictionary_Renamed.Encoding = Encoding.GetEncoding(1252)
			dictionary_Renamed.Load()
			customDictionary_Renamed = New SpellCheckerCustomDictionary()
			customDictionary_Renamed.AlphabetPath = dictionary_Renamed.AlphabetPath
			customDictionary_Renamed.DictionaryPath = "Dictionaries\Custom.txt"
			customDictionary_Renamed.CaseSensitive = False
			customDictionary_Renamed.Culture = dictionary_Renamed.Culture
			customDictionary_Renamed.Encoding = dictionary_Renamed.Encoding
			customDictionary_Renamed.Load()
			DevExpress.XtraSpellChecker.Native.SpellCheckTextControllersManager.Default.RegisterClass(GetType(StringEdit), GetType(DevExpress.XtraSpellChecker.Native.SimpleTextEditTextController))
			DevExpress.XtraSpellChecker.Native.SpellCheckTextControllersManager.Default.RegisterClass(GetType(LargeStringEdit), GetType(DevExpress.XtraSpellChecker.Native.SimpleTextEditTextController))
			DevExpress.XtraSpellChecker.Native.SpellCheckTextControllersManager.Default.RegisterClass(GetType(MemoEdit), GetType(DevExpress.XtraSpellChecker.Native.SimpleTextEditTextController))
			DevExpress.XtraSpellChecker.Native.SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(GetType(StringEdit), GetType(StringEditTextBoxFinder))
			DevExpress.XtraSpellChecker.Native.SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(GetType(LargeStringEdit), GetType(LargeStringEditTextBoxFinder))
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
End Namespace
