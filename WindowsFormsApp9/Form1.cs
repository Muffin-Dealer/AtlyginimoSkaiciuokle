using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        ComboBox capability;
        ComboBox pencion;
        Dictionary<string, double> pen;
        Dictionary<string, int> cap;
        TextBox sum;
        Dictionary<string, string> ok;
        public static int minimumas = 555;
        public static double minimumasIrankas = 395.77;


        public Form1()
        {
            InitializeComponent();
            pen = new Dictionary<string, double>();
            cap = new Dictionary<string, int>();
            pen.Add("Pencijos kaupimas", 0);
            pen.Add("0%", 0);
            pen.Add("1.8%", 1.8);
            pen.Add("3%", 3);
            cap.Add("Darbingumo grupe", 300);
            cap.Add("100%", 300);
            cap.Add("30-55%", 308);
            cap.Add("0-25%", 353);
            Loadingas();


        }
        public void sum_changed(object sender, EventArgs e)
        {
            try
            {
                double aaaa;
                if (sum.Text == "")
                    aaaa = 0;
                else
                    aaaa = Convert.ToDouble(sum.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veskite tiktai skaicius");
            }
            foreach (var item in Tabai.SelectedTab.Controls.OfType<Panel>())
            {
                Tabai.SelectedTab.Controls.Remove(item);
            }
            ResultPanel();


        }
        public void Loadingas()
        {

            Label lab = new Label();
            lab.Location = new Point(20, 20);
            lab.Text = "Gauta :";
            lab.Size = new Size(45, 15);
            sum = new TextBox();
            sum.Location = new Point(lab.Location.X + lab.Width + 10, 17);
            //sum.Location = new Point(Tabai.SelectedTab.Width, Tabai.SelectedTab.Height);
            capability = new ComboBox();
            pencion = new ComboBox();
            capability.Location = new Point(sum.Location.X + sum.Width + 100, 17);

            capability.DataSource = new BindingSource(cap, null);
            capability.DisplayMember = "Key";
            capability.ValueMember = "Value";
            //pencion.Location = new Point(capability.Location.X + capability.Width + 25, 17);
            pencion.Location = new Point(sum.Location.X + sum.Width + 100, 47);

            pencion.DataSource = new BindingSource(pen, null);
            pencion.DisplayMember = "Key";
            pencion.ValueMember = "Value";


            Tabai.SelectedTab.Controls.Add(lab);
            Tabai.SelectedTab.Controls.Add(sum);
            Tabai.SelectedTab.Controls.Add(capability);
            Tabai.SelectedTab.Controls.Add(pencion);
            sum.TextChanged += new EventHandler(sum_changed);

            capability.SelectedValueChanged += new EventHandler(sum_changed);
            pencion.SelectedValueChanged += new EventHandler(sum_changed);
            ResultPanel();

        }

        public void ResultPanel()
        {

            ok = new Dictionary<string, string>();

            if (Tabai.SelectedTab == tabPage1 && sum.Text != "")
            {
                ok.Add("NPD", (TaikomaNPD(Convert.ToDouble(sum.Text), (int)capability.SelectedValue)).ToString("F2"));
                ok.Add("GPM", GPM(Convert.ToDouble(sum.Text), (int)capability.SelectedValue).ToString("F2"));
                ok.Add("Sodra", Sodra(Convert.ToDouble(sum.Text), (double)pencion.SelectedValue).ToString("F2"));
                ok.Add("I Rankas", I_rankas(Convert.ToDouble(sum.Text), (int)capability.SelectedValue, (double)pencion.SelectedValue).ToString("F2"));
                ok.Add("Ant Popieriaus", sum.Text);
            }
            else if (Tabai.SelectedTab == tabPage2 && sum.Text != "")
            {
                ok.Add("NPD", (TaikomaNPD(Ant_popieriaus(Convert.ToDouble(sum.Text), (int)capability.SelectedValue, (double)pencion.SelectedValue), (int)capability.SelectedValue)).ToString("F2"));
                ok.Add("GPM", GPM(Ant_popieriaus(Convert.ToDouble(sum.Text), (int)capability.SelectedValue, (double)pencion.SelectedValue), (int)capability.SelectedValue).ToString("F2"));
                ok.Add("Sodra", Sodra(Ant_popieriaus(Convert.ToDouble(sum.Text), (int)capability.SelectedValue, (double)pencion.SelectedValue), (double)pencion.SelectedValue).ToString("F2"));
                ok.Add("I Rankas", sum.Text);
                ok.Add("Ant Popieriaus", Ant_popieriaus(Convert.ToDouble(sum.Text), (int)capability.SelectedValue, (double)pencion.SelectedValue).ToString("F2"));
            }
            else
            {
                ok.Add("NPD", "-");
                ok.Add("GPM", "-");
                ok.Add("Sodra", "-");
                ok.Add("I Rankas", "-");
                ok.Add("Ant Popieriaus", "-");
            }

            Panel panel = new Panel();
            panel.Location = new Point(10, 87);
            panel.BackColor = Color.Black;
            panel.Size = new Size(Tabai.Width - 35, Tabai.Height - 117);
            int k = 0;
            foreach (var item in ok)
            {
                Panel pan = new Panel();
                pan.Location = new Point(10, 10 + 40 * k);
                pan.Size = new Size(500, 40);
                if (k % 2 == 0)
                    pan.BackColor = Color.Yellow;
                else
                    pan.BackColor = Color.Green;
                Label label1 = new Label();
                Label label2 = new Label();
                label1.Text = item.Key;
                label2.Text = item.Value;
                label1.Location = new Point(10, 20);
                label2.Location = new Point(pan.Width - 100, 20);
                pan.Controls.Add(label1);
                pan.Controls.Add(label2);
                panel.Controls.Add(pan);
                k++;
            }

            Tabai.SelectedTab.Controls.Add(panel);
        } 
        






        private static double Ant_popieriaus(double rankos, int neigalus, double kaupimas)
        {
            double suma;
            if (rankos <= minimumasIrankas)
            {
                //suma = poperio - (poperio * (19.5 + kaupimas) / 100) - ((poperio - neigalus) * 20 / 100);
                //100 * suma = 100 * poperio - (poperio * (19.5 + kaupimas)) - ((poperio - neigalus) * 20);
                //100 * suma / poperio = 100 - ((19.5 + kaupimas)) - ((20 - (neigalus * 20 / poperio)));
                //100 * suma / poperio = 100 - 19.5 - kaupimas - 20 + (neigalus * 20 / poperio);
                //100 * suma = (100 - 19.5 - kaupimas - 20) * poperio + (neigalus * 20);
                //100 * suma = (100 - 19.5 - kaupimas - 20) * poperio + (neigalus * 20);
                suma = (100 * rankos - 20 * neigalus) / (100 - 19.5 - 20 - kaupimas);
                if (suma < 0)
                    suma = (100 * rankos) / (100 - 19.5 + kaupimas);

            }
            else
            {// suma = poperio - (poperio * (19.5 + kaupimas) / 100) - (poperio - (neigalus - (0.15 * (poperio - minimumas))));
                //suma = poperio - (poperio * (19.5 + kaupimas) / 100) - (poperio - (neigalus - (0.15 * (poperio - minimumas)))) * 20 / 100;
                //100 * suma = 100 * poperio - (poperio * (19.5 + kaupimas)) - (poperio - (neigalus - (0.15 * (poperio - minimumas)))) * 20;
                //100 * suma = 100 * poperio - (poperio * (19.5 + kaupimas)) - poperio * 20 + neigalus * 20 - 0.15 * poperio * 20 + 0.15 * minimumas * 20;
                //100 * suma / poperio = 100 - (19.5 + kaupimas) - 20 + neigalus * 20 / poperio - 0.15 * 20 + 0.15 * minimumas * 20 / poperio;
                //100 * suma / poperio = 100 - 19.5 - kaupimas - 20 + neigalus * 20 / poperio - 0.15 * 20 + 0.15 * minimumas * 20 / poperio;
                //100 * suma / poperio = (100 - 19.5 - kaupimas - 20 - 0.15 * 20) + neigalus * 20 / poperio + 0.15 * minimumas * 20 / poperio;
                //100 * suma = (100 - 19.5 - kaupimas - 20 - 0.15 * 20)*poperio + neigalus * 20 + 0.15 * minimumas * 20;
                //poperio = (100 * suma - neigalus * 20 - 0.15 * minimumas * 20) / (100 - 19.5 - kaupimas - 20 - 0.15 * 20);
                suma = (100 * rankos - neigalus * 20 - 0.15 * minimumas * 20) / (100 - 19.5 - kaupimas - 20 - 0.15 * 20);
                if (rankos >= 1545.77)
                    suma = (100 * rankos) / (100 - 19.5 - kaupimas-20);
                //suma = poperio - (poperio * (19.5 + kaupimas) / 100) - poperio * 20 / 100;
                //100*suma = 100*poperio - (poperio * (19.5 + kaupimas)) - poperio * 20;
                //100*suma = (100 - 19.5 - kaupimas - 20)*pop;

            }
            Console.WriteLine(suma);
            return suma;
        }
        private static double I_rankas(double poperio, int neigalus, double kaupimas)
        {
            double suma = poperio - Sodra(poperio, kaupimas) - GPM(poperio, neigalus);
            return suma;
        }

        private static double GPM(double poperio, int neigalus)
        {
            double suma = ((poperio - TaikomaNPD(poperio, neigalus)) * 20) / 100;
            if (suma < 0)
                return 0;
            else
                return suma;
        }

        private static double Sodra(double poperio, double kaupimas)
        {
            double SodraDarbuotojo = (poperio * (19.5 + kaupimas)) / 100;
            return SodraDarbuotojo;
        }

        private static void SodraDarbdaviui(double poperio, double pavojus)
        {
            double SodraDarbdaviui = (poperio * pavojus) / 100;
        }
        private static double TaikomaNPD(double poperio, int neigalus)
        {
            double suma;
            if (poperio <= minimumas)
            {
                suma = neigalus;
            }
            else
            {
                suma = (neigalus - (0.15 * (poperio - minimumas)));
            }
            if (suma < 0)
                suma = 0;
            return suma;
        }

       

        private void Tabai_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabPage1.Controls.Clear();
            tabPage2.Controls.Clear();

            Loadingas();
        }
    }
}
