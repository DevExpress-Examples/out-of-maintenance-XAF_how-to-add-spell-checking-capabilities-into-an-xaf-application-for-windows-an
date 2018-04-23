using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Updating;

namespace WinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(Session session, Version currentDBVersion) : base(session, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            Document d = Session.FindObject<Document>(CriteriaOperator.Parse("Name='How to usse XtraSpellChecker in XAF'"));
            if (d == null) {
                d = new Document(Session);
                d.Name = "How to usse XtraSpellChecker in XAF";
                d.Save();
            }
            Article a = Session.FindObject<Article>(CriteriaOperator.Parse("Subject='Artiicle 1'"));
            if (a == null) {
                a = new Article(Session);
                a.Subject = "Artiicle 1";
                a.Document = d;
                a.Description = "Descriptionn 1";
                a.Body = @"It is with difficulty that I persuade myself, that it is I who am sitting and writing to you from this great city of the East. Whether I look upon the face of nature, or the works of man, I see every thing different from what the West presents; so widely different, that it seems to me, at times, as if I were subject to the power of a dream. But I rouse myself, and find that I am awake, and that it is really I, your old friend and neighbor, Piso, late a dweller upon the Coelian hill, who am now basking in the warm skies of Palmyra, and, notwithstanding all the splendor and luxury by which I am surrounded, longing to be once more in Rome, by the side of my Curtius, and with him discoursing, as we have been wont to do, of the acts and policy of the magnificent Aurelian.

But to the purpose of this letter, which is, in agreement with my promise, to tell you of my fortunes since I parted from you, and of my good or ill success, as it may be, in the prosecution of that affair which has driven me so far from my beloved Rome. O, Humanity! why art thou so afflicted? Why have the immortal gods made the cup of life so bitter? And why am I singled out to partake of one that seems all bitter? My feelings sometimes overmaster my philosophy. You can forgive this, who know my sorrows. Still I am delaying to inform you concerning my journey and my arrival. Now I will begin.

This text have been written in the bygone millenium by William Ware, 1797-1852. This is an opening of his novel, ""Zenobia or the Fall of Palmyra"", the full text of which you can definately find using the http://www.google.com or mailing me at someone@somewhere.";
                a.Save();
            }
        }
    }
}
