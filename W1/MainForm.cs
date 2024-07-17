using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace W1
{
    
    public partial class MainForm : Form
    {
        
        // ประกาศและสร้างอ็อบเจ็กต์ของหน้าต่าง VegetableForm และ MeatForm
        VegetableForm VeForm = new VegetableForm();
        MeatForm MeForm = new MeatForm();
        // สร้างตัวแปรสำหรับเก็บรายละเอียดของผักและเนื้อ
        private string vegetableDetails = string.Empty, meatDetails = string.Empty;
        // สร้างตัวแปรสำหรับเก็บรวมราคาของผักและเนื้อ
        private int Vegetableresult, Meatresult;

        public MainForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ControlBox = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
        // เมทอดเรียกเมื่อคลิกที่เมนู ToolStripItem
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // แปลงอ็อบเจ็กต์จาก sender เป็น ToolStripMenuItem
            ToolStripMenuItem Tstrip = (ToolStripMenuItem)sender;
            // ตรวจสอบว่าคลิกที่เมนูผักหรือเนื้อ
            if (Tstrip.Name == "ผกสดToolStripMenuItem")
            {
                // แสดงหน้าต่าง VeForm เพื่อกรอกข้อมูลผัก
                vegetableDetails = string.Empty;
                if (VeForm.ShowDialog() == DialogResult.OK)
                {
                    // คำนวณราคาและบันทึกรายละเอียดผัก
                    Vegetableresult = VegetableCost(
                        (NumericUpDown)VeForm.Controls["groupBox1"].Controls["numericUpDown4"],
                        (NumericUpDown)VeForm.Controls["groupBox1"].Controls["numericUpDown3"],
                        (NumericUpDown)VeForm.Controls["groupBox1"].Controls["numericUpDown2"],
                        (NumericUpDown)VeForm.Controls["groupBox1"].Controls["numericUpDown1"]
                    );
                }
            }
            else if (Tstrip.Name == "เนอสดToolStripMenuItem")
            {
                // แสดงหน้าต่าง MeForm เพื่อกรอกข้อมูลเนื้อ
                meatDetails = string.Empty;
                if (MeForm.ShowDialog() == DialogResult.OK)
                {
                    // คำนวณราคาและบันทึกรายละเอียดเนื้อ
                    Meatresult = MeatCost(
                        (NumericUpDown)MeForm.Controls["groupBox1"].Controls["numericUpDown1"],
                        (NumericUpDown)MeForm.Controls["groupBox1"].Controls["numericUpDown2"],
                        (NumericUpDown)MeForm.Controls["groupBox1"].Controls["numericUpDown3"],
                        (NumericUpDown)MeForm.Controls["groupBox1"].Controls["numericUpDown4"]
                    );
                }
            }
        }
        // เมทอดเรียกเมื่อคลิกที่ปุ่ม
        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            try
            {
                // คำนวณราคารวมและแสดงข้อมูลใบสั่งซื้อ
                int result = Vegetableresult + Meatresult;
                switch (btn.Name)
                {
                    case "button1":
                        // ตรวจสอบข้อมูลที่ป้อน
                        ValidateInput();

                        // เรียกใช้เมธอดเพื่อแสดง ReportForm และนำข้อมูลไปแสดงใน DataGridView

                        ShowReportForm(result);
                        // ล้างข้อมูลในฟอร์มหลังจากแสดง ReportForm เสร็จสิ้น
                        ResetFormControls(VeForm);
                        ResetFormControls(MeForm);
                        break;

                    case "button2":
                        // ล้างข้อมูลในฟอร์ม
                        ResetFormControls(VeForm);
                        ResetFormControls(MeForm);
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                // แสดงข้อผิดพลาดถ้ามีข้อมูลไม่ถูกต้อง
                MessageBox.Show(ex.Message, "ผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // เมทอดตรวจสอบข้อมูลที่ป้อน
        private void ValidateInput()
        {
            // ตรวจสอบว่าข้อมูลที่ป้อนครบถ้วน
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                throw new ArgumentException("กรุณากรอกข้อมูลให้ครบทุกช่อง");
            }
            // ตรวจสอบว่ามีการเลือกสินค้าอย่างน้อย 1 ชิ้น
            if (!HasNonZeroValue(VeForm) || !HasNonZeroValue(MeForm))
            {
                throw new ArgumentException("กรุณาเลือกสินค้าอย่างน้อยชนิดละ 1 ชิ้น");
            }
        }
        // เมธอดตรวจสอบว่ามีค่าของ NumericUpDown ที่มีค่ามากกว่า 0 หรือไม่
        private bool HasNonZeroValue(Form form)
        {
            foreach (Control groupBoxControl in form.Controls["groupBox1"].Controls)
            {
                if (groupBoxControl is NumericUpDown numericUpDown && numericUpDown.Value > 0)
                {
                    return true;
                }
            }
            return false;
        }
        // เมทอดล้างข้อมูลในฟอร์ม
        private void ResetFormControls(Form form)
        {
            // รีเซ็ตค่าของ TextBox, DateTimePicker, และตัวแปรที่เก็บรายละเอียดการสั่งซื้อ
            textBox1.Text = textBox2.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            vegetableDetails = string.Empty;
            meatDetails = string.Empty;

            // รีเซ็ตค่าของ NumericUpDown ใน groupBox1 ทั้งหมดให้เป็น 0
            foreach (Control groupBoxControl in form.Controls["groupBox1"].Controls)
            {
                if (groupBoxControl is NumericUpDown numericUpDown)
                {
                    numericUpDown.Value = 0;
                }
            }
        }
        // เมทอดคำนวณราคาทั้งหมดของผักตามจำนวนเลือก
        private int VegetableCost(params NumericUpDown[] numericUpDowns)
        {
            int totalCost = 0;
            // วนลูปคำนวณราคาของผักตามปริมาณที่เลือก
            for (int i = 0; i < numericUpDowns.Length; i++)
            {
                int quantity = (int)numericUpDowns[i].Value;
                if (quantity > 0)
                {
                    // คำนวณราคาและเก็บรายละเอียดการสั่งซื้อของผัก
                    int cost = quantity * 25;
                    totalCost += cost;

                    Label label = (Label)VeForm.Controls["groupBox1"].Controls.Find($"label{i + 1}", true)[0];
                    vegetableDetails += $"{label.Text} {numericUpDowns[i].Tag} {quantity} แพ็ค ราคา {cost} บาท\n";
                }
            }
            return totalCost;
        }
        // เมทอดคำนวณราคาทั้งหมดของเนื้อตามจำนวนเลือก
        private int MeatCost(params NumericUpDown[] numericUpDowns)
        {
            int totalCost = 0;
            // วนลูปคำนวณราคาของเนื้อตามปริมาณที่เลือก
            for (int i = 0; i < numericUpDowns.Length; i++)
            {
                // คำนวณราคาและเก็บรายละเอียดการสั่งซื้อของเนื้อ
                int quantity = (int)numericUpDowns[i].Value;
                if (quantity > 0)
                {
                    int cost = quantity * 60;
                    totalCost += cost;

                    Label label = (Label)MeForm.Controls["groupBox1"].Controls.Find($"label{i + 1}", true)[0];
                    meatDetails += $"{label.Text} {numericUpDowns[i].Tag} {quantity} แพ็ค ราคา {cost} บาท\n";
                }
            }
            return totalCost;
        }
        private void ShowReportForm(int result)
        {
            // สร้าง ReportForm
            ReportForm reportForm = new ReportForm();

            reportForm.label1.Text = textBox1.Text;
            reportForm.label2.Text = textBox2.Text;
            reportForm.label3.Text = dateTimePicker1.Value.ToShortDateString();
            reportForm.SetResult(result.ToString());

            // แยกรายการอาหารผักและเนื้อให้แสดงในแถวต่าง ๆ
            string[] vegetableLines = vegetableDetails.Split('\n');
            string[] meatLines = meatDetails.Split('\n');

            // เพิ่มแถวใน DataGridView สำหรับรายการอาหารผัก
            foreach (string line in vegetableLines)
            {
                reportForm.dataGridView1.Rows.Add(new object[] { line });
            }

            // เพิ่มแถวใน DataGridView สำหรับรายการอาหารเนื้อ
            foreach (string line in meatLines)
            {
                reportForm.dataGridView1.Rows.Add(new object[] { line });
            }
            

            // แสดง ReportForm
            reportForm.Show();
        }
    }
}
