using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicSimulator
{
    public partial class ChangeValue : Form
    {
        public string TextBoxValue
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public ChangeValue()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra giá trị nhập có hợp lệ hay không
            double value;
            if (double.TryParse(textBox1.Text, out value))
            {
                // Giá trị hợp lệ, trả về DialogResult.OK để xác nhận
                DialogResult = DialogResult.OK;
            }
            else
            {
                // Giá trị không hợp lệ, hiển thị thông báo lỗi
                MessageBox.Show("Giá trị không hợp lệ. Vui lòng nhập một số thực.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Trả về DialogResult.Cancel để hủy bỏ
            DialogResult = DialogResult.Cancel;
        }
    }
}
