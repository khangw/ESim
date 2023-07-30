using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media;

namespace ElectronicSimulator
{
    public partial class RunCircuit : Form
    {
        private readonly double v_out;
        private readonly double v_in;
        private ChartValues<double> chartValuesIn;
        private ChartValues<double> chartValuesOut;
        private double frequency = 5; // Tần số mặc định
        public RunCircuit(double v_out, double v_in)
        {
            this.v_out = v_out;
            this.v_in = v_in;
            InitializeComponent();
            // Khởi tạo giá trị cho biểu đồ
            chartValuesIn = new ChartValues<double>();
            chartValuesOut = new ChartValues<double>();
            chart.Series.Add(new LineSeries
            {
                Values = chartValuesIn,
                Fill = System.Windows.Media.Brushes.Transparent,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(107, 185, 79)),
                PointForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 22, 27)),
                PointGeometrySize = 5
            });
            chart.Series.Add(new LineSeries
            {
                Values = chartValuesOut,
                Fill = System.Windows.Media.Brushes.Transparent,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(26, 122, 129)),
                PointForeground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(34, 22, 27)),
                PointGeometrySize = 5
            });

            // Thêm sự kiện cho thanh trượt tần số
            trackBar1.ValueChanged += trackBarFrequency_ValueChanged;

            // Vẽ biểu đồ ban đầu
            drawChart();
        }

        private void trackBarFrequency_ValueChanged(object sender, EventArgs e)
        {
            // Cập nhật tần số từ giá trị của thanh trượt
            frequency = trackBar1.Value;

            // Vẽ lại biểu đồ khi tần số thay đổi
            drawChart();
        }

        private void drawChart()
        {
            // Xóa giá trị cũ
            chartValuesIn.Clear();
            chartValuesOut.Clear();

            // Tạo dữ liệu cho biểu đồ hình sin
            for (double t = 0; t <= 2 * Math.PI; t += 0.05)
            {
                double vo = v_out * Math.Sin(frequency * t);
                double vi = v_in * Math.Sin(frequency * t);
                chartValuesOut.Add(vo);
                chartValuesIn.Add(vi);
            }
        }
        
    }
}
