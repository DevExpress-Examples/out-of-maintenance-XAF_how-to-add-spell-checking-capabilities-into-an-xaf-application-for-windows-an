<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128587639/10.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E736)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [SpellCheckerInitializer.cs](./CS/Dennis.Spelling.Win/SpellCheckerInitializer.cs) (VB: [SpellCheckerInitializer.vb](./VB/Dennis.Spelling.Win/SpellCheckerInitializer.vb))
* [SpellCheckerViewController.cs](./CS/Dennis.Spelling.Win/SpellCheckerViewController.cs) (VB: [SpellCheckerViewController.vb](./VB/Dennis.Spelling.Win/SpellCheckerViewController.vb))
<!-- default file list end -->
# How to add spell checking capabilities into an XAF application for Windows and the Web


<p><strong>Scenario</strong><br>This example demonstrates how to integrate WinForms <a href="https://documentation.devexpress.com/WindowsForms/CustomDocument2635.aspx">SpellChecker</a> and ASP.NET WebForms <a href="https://documentation.devexpress.com/AspNet/CustomDocument3686.aspx">ASPxSpellChecker</a> components in XAF. These utilities provide a straightforward way in which to add Microsoft® Office® style spell checking capabilities into your Windows and Web applications and offer you built in suggestion forms that replicate corresponding forms found in Microsoft Outlook.<br><br><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-add-spell-checking-capabilities-into-an-xaf-application-for-windows-and-the-web-e736/10.1.4+/media/9bf1283e-0b86-11e6-80bf-00155d62480c.png"><br><strong>Note:</strong> this is not a complete solution by any means, but just an example that demonstrates certain integration scenarios to help you implement a spell checking functionality in your end application. There may be issues, so feel free to research, test and modify the source code of these modules to better meet your business needs. We look forward to any feedback on these modules if you find issues or have further suggestions.<br><br></p>
<p><strong>Steps to implement<br>1.</strong> <a href="https://www.devexpress.com/Support/Center/Attachment/GetAttachmentFile/3a8ba995-0b8c-11e6-80bf-00155d62480c">Here you can download</a> the improved implementation, which is recommended for use with the latest XAF versions (the old example versions for <= v12.2 are still available below). This attachment contains three custom <a href="https://documentation.devexpress.com/#Xaf/CustomDocument2569">XAF extra modules</a>  + a small demo project to help you check what is done and how this works.<br><strong>2.</strong> Copy and include the DevExpress.ExpressApp.SpellChecker, DevExpress.ExpressApp.SpellChecker.Win and DevExpress.ExpressApp.SpellChecker.Web projects into your own XAF solution and build it;<br><strong>3.</strong> Invoke the Application Designer for your executable WinForms project (YourSolutionName.Win) or the Module Designer for your WinForms module project (YourSolutionName.Module.Win) and open the Toolbox (Control+Alt+X) to drag the <em>SpellCheckerWindowsFormsModule </em>item from the Toolbox;<br><strong>4.</strong> Invoke the Application Designer for your executable ASP.NET WebForms project (YourSolutionName.Web) or the Module Designer for your ASP.NET WebForms module project (YourSolutionName.Module.Web) and open the Toolbox (Control+Alt+X) to drag the <em>SpellCheckerAspNetModule </em>item from the Toolbox;<br><strong>5.</strong> Copy the Dictionaries folder from the attachment (see under the Demo.Win folder) into your executable WinForms project (YourSolutionName.Win) and ASP.NET WebForms project (YourSolutionName.Web) projects.<br><strong>6.</strong> Run the Model Editor for YourSolutionName.Win/Model.XAFML file and adjust required settings under the Options | SpellChecker node.</p>
<p><br><strong>Important notes<br></strong><strong>1.</strong> If you are using RichEditControl, check out the <a href="https://www.devexpress.com/Support/Center/p/Q418588">Adding Richedit to Sample E736</a> thread for additional configuration.<br><strong>2.</strong> Our XtraSpellChecker Suite does not currently support the standard System.Windows.Forms.WebBrowser component (<a href="https://www.devexpress.com/Support/Center/p/Q142193">learn more...</a>) reused in the Design tab of our WinForms <a href="https://documentation.devexpress.com/WindowsForms/CustomDocument4874.aspx">HtmlPropertyEditor</a>. So, spell checking works fine only in the Html tab where the MemoEdit control is used.<br><strong>3.</strong> A VB.NET version is not planned due to the implementation and maintenance complexity, so you can either use <a href="http://stackoverflow.com/questions/862723/use-vb-net-and-c-sharp-in-the-same-application">this approach</a> OR convert this code from the attachment into your language using free tools found on the Web.</p>

<br/>


