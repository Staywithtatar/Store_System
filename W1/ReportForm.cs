using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace W1
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();


        }
        public void SetResult(string result)
        {
            label4.Text = result;
        }
        public void AddDataToGridView(string data)
        {
            // เพิ่มข้อมูลใน DataGridView
            dataGridView1.Rows.Add(new object[] { data });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // สร้างไฟล์ PDF
            PdfDocument pdf = new PdfDocument();
            pdf.Info.Title = "ใบเสร็จ"; // กำหนดชื่อใบเสร็จ

            // เพิ่มหน้าใหม่ในไฟล์ PDF
            PdfPage page = pdf.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Tahoma", 12);

            // กำหนดตำแหน่งเริ่มต้นของข้อความ
            XRect rect = new XRect(40, 40, page.Width.Point - 80, page.Height.Point - 80);

            // เริ่มเขียนข้อความลงในไฟล์ PDF
            gfx.DrawString("ข้อมูลใบสั่งซื้อ", font, XBrushes.Black, rect, XStringFormats.TopLeft);

            // กำหนดตำแหน่งเริ่มต้นของข้อมูล
            int yPos = 80;

            // เพิ่มข้อมูลจาก Label1-4 ลงในไฟล์ PDF
            string[] labelData = { label1.Text, label2.Text, label3.Text, label4.Text };
            foreach (string data in labelData)
            {
                gfx.DrawString(data, font, XBrushes.Black, new XRect(40, yPos, page.Width.Point - 80, page.Height.Point - 80), XStringFormats.TopLeft);
                yPos += 20; // ขยับข้อความลงไปทีละบรรทัด
            }

            // เพิ่มข้อมูลจาก DataGridView ลงในไฟล์ PDF
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string rowData = row.Cells[0].Value.ToString();
                    gfx.DrawString(rowData, font, XBrushes.Black, new XRect(40, yPos, page.Width.Point - 80, page.Height.Point - 80), XStringFormats.TopLeft);
                    yPos += 20; // ขยับข้อความลงไปทีละบรรทัด
                }
            }

            // กำหนดตำแหน่งของไฟล์ PDF ที่ต้องการบันทึก
            string pdfFilename = "ใบเสร็จ.pdf";

            // บันทึกไฟล์ PDF
            pdf.Save(pdfFilename);

            // เปิดไฟล์ PDF หลังจากบันทึก
            System.Diagnostics.Process.Start(pdfFilename);
        }
    }
}
