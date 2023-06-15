using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace ibancheckerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public BigInteger Modulo ( BigInteger nummerein, BigInteger nummerzwei) 
        {
            return nummerein % nummerzwei;
        }

        private void btnIBAN_Click(object sender, EventArgs e)
        {

            string pattern = @"DE[0-9]{11,}";// Create a Regex  // picture box // textbox control das eigene eingaben key down event ! regex üverprüfen 
            string ibanEingabe = Regex.Replace(txtIBAN.Text.ToString(), "\\s+", ""); // falls es space enthält werden sie hier entfernt 
            bool ibanSizeBool = ibanEingabe.Length <= 22;
            int ibanLaengeAlsInt = ibanEingabe.Length; // Sollte 22 sein
            string pruefziffer = ibanEingabe.Substring(2, 2); //DE(45)
            bool boolpruefziffer = Convert.ToInt32(pruefziffer) > 1 && Convert.ToInt32(pruefziffer) < 99; // Ist nummer zwischen 2 und 98?
            if ((Regex.IsMatch(ibanEingabe,pattern)) && (ibanSizeBool) && (boolpruefziffer)) // Falls Iban >= 13 aber kleiner < 22  ist werden nullen hinzugefügt bis es 22 Ziffern lang ist
            {
                int aktuelleIbanLaengeUngleichNull = 22 - ibanLaengeAlsInt; // 0 heißt es sind 22 != 0 heißt es fehlen diese anzahl von Kontonummer.3 == die zahl die hinzugefügt werden muss 
                int aktuelleKontonummerLaengeUNgleichNull = 10 - aktuelleIbanLaengeUngleichNull; // KN sollte 10 sein deshsalb
                string startUndEndeVonkleineKontonummer = ibanEingabe.Substring(12, aktuelleKontonummerLaengeUNgleichNull); // KontonummerStart 12 bis (+) z.b 3 
                int kleineKontonummerLaengeInt = startUndEndeVonkleineKontonummer.Length;
                string kleineIban = ibanEingabe.Substring(0, ibanLaengeAlsInt); // kontonummer
                if (kleineKontonummerLaengeInt < 10)
                {
                    for (int g = ibanLaengeAlsInt; g < 22; g++)
                    {
                        string testnull = "0";
                        kleineIban = kleineIban + testnull;
                    }
                } //Iban wird zusammengesetz (DE00 Rückt nach vorne etc) .Bereitstellung für Rechnung
                string neueIban = kleineIban;
                string restDerIbanNachPruefziffer = neueIban.Substring(4, 18);
                string deKonvertiertUndNullen = "131400"; //DE50 1000 0000 1234 5678 89//DE50100000001234567889                     
                string neueIbanDurchInsertAmEndeDerKontonummer = restDerIbanNachPruefziffer.Insert(18, deKonvertiertUndNullen); // Komplett neue Iban Mit 131400 am Ende der Kontonummer
                // Rechnung und Kovertierung 
                int prueffzifferToInt = Convert.ToInt32(pruefziffer);
                BigInteger neuBigIntIban = BigInteger.Parse(neueIbanDurchInsertAmEndeDerKontonummer);
                BigInteger moduloZahl = new BigInteger(97);
                BigInteger bigIntRechnungIbanUndModuloZahl = Modulo(neuBigIntIban, moduloZahl);
                string insertFuehrendeNullErgebnis = "";
                BigInteger subtraktionVonModuloErgebnis = 98 - bigIntRechnungIbanUndModuloZahl;
                string convertSubtraktionsErgebnistoString = subtraktionVonModuloErgebnis.ToString(); // Convert Big INt To String
                insertFuehrendeNullErgebnis = convertSubtraktionsErgebnistoString;
                int convertingBackToInt = Convert.ToInt32(insertFuehrendeNullErgebnis);
                if (convertingBackToInt != prueffzifferToInt)
                {
                    MessageBox.Show(" NO!! Prüfziffer ist falsch. IBAN ist nicht valide");
                }
                else
                {
                    MessageBox.Show("YES !! Prüfziffer ist valide");
                }
            }
            else
            {
                errorProvider1.SetError(this.txtIBAN,$"Iban muss min. 13 bis max. 22 Ziffern sein.Ihre ist aktuell {ibanLaengeAlsInt}  Ziffern lang.DE Großschreiben + nach DE nur noch Zahlen bitte");
            }
            
            
        }
    }
}
