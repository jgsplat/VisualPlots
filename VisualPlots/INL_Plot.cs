using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;       // Needed for chart

namespace VisualPlots
{
    

    public partial class INL_Plot : Form
    {
        List<int> codes = new List<int>();
        List<double> inls = new List<double>();

        public INL_Plot(bool trim_btn, List<int> codes, List<double> inls)
        {
            InitializeComponent();
            this.codes = codes;
            this.inls = inls;

            btnTrim.Visible = trim_btn;

        }

        private void INL_Plot_Load(object sender, EventArgs e)
        {
            BubbleSort(ref codes, ref inls);
            plot_INL.Series[0].Points.Clear();

            for (int i = 0; i < codes.Count; i++)
            {
                plot_INL.Series[0].Points.AddXY(codes[i], inls[i]);
            }
            plot_INL.Series[0].Points.AddXY(32768, 0);
        }

        public void BubbleSort(ref List<short> codes, ref List<double> inls)
        {

            for (int i = 0; i < codes.Count; i++)
            {
                for (int j = i; j < codes.Count; j++)
                {
                    if (codes[j] < codes[i])
                    {
                        short temp_codes = codes[i];
                        double temp_inls = inls[i];

                        codes[i] = codes[j];
                        inls[i] = inls[j];

                        codes[j] = temp_codes;
                        inls[j] = temp_inls;
                    }
                }
            }

            return;
        }
        public void BubbleSort(ref List<int> codes, ref List<double> inls)
        {

            for (int i = 0; i < codes.Count; i++)
            {
                for (int j = i; j < codes.Count; j++)
                {
                    if (codes[j] < codes[i])
                    {
                        int temp_codes = codes[i];
                        double temp_inls = inls[i];

                        codes[i] = codes[j];
                        inls[i] = inls[j];

                        codes[j] = temp_codes;
                        inls[j] = temp_inls;
                    }
                }
            }

            return;
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintChart);
            pd.DefaultPageSettings.Landscape = true;

            PrintDialog pdi = new PrintDialog();
            pdi.Document = pd;
            if (pdi.ShowDialog() == DialogResult.OK)
            {
                pdi.Document.Print();
            }

        }

        void PrintChart(object sender, PrintPageEventArgs ev)
        {
            using (var f = new System.Drawing.Font("Arial", 10))
            {
                var size = ev.Graphics.MeasureString(Text, f);
                ev.Graphics.DrawString("INL Plot", f, Brushes.Black, ev.PageBounds.X + (ev.PageBounds.Width - size.Width) / 2, ev.PageBounds.Y);
            }

            //Note, the chart printing code wants to print in pixels.
            Rectangle marginBounds = ev.MarginBounds;
            if (ev.Graphics.PageUnit != GraphicsUnit.Pixel)
            {
                ev.Graphics.PageUnit = GraphicsUnit.Pixel;
                marginBounds.X = (int)(marginBounds.X * (ev.Graphics.DpiX / 100f));
                marginBounds.Y = (int)(marginBounds.Y * (ev.Graphics.DpiY / 100f));
                marginBounds.Width = (int)(marginBounds.Width * (ev.Graphics.DpiX / 100f));
                marginBounds.Height = (int)(marginBounds.Height * (ev.Graphics.DpiY / 100f));
            }

            plot_INL.Printing.PrintPaint(ev.Graphics, marginBounds);
        }

        private void btnTrim_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }


    }
}
