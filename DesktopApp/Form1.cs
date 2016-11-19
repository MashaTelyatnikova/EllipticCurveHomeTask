using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class Form1 : Form
    {
        private readonly EllipticCurveParameters ellipticCurveParameters = new EllipticCurveParameters();

        public Form1()
        {
            InitializeComponent();
            aTextBox.TextChanged += TextBoxOnTextChanged;
            bTextBox.TextChanged += TextBoxOnTextChanged;
            pTextBox.TextChanged += TextBoxOnTextChanged;
            x1.TextChanged += TextBoxOnTextChanged;
            x2.TextChanged += TextBoxOnTextChanged;
            y1.TextChanged += TextBoxOnTextChanged;
            y2.TextChanged += TextBoxOnTextChanged;
        }

        private void TextBoxOnTextChanged(object sender, EventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(aTextBox.Text) && !string.IsNullOrEmpty(bTextBox.Text) &&
                !string.IsNullOrEmpty(pTextBox.Text) &&
                !string.IsNullOrEmpty(x1.Text) && !string.IsNullOrEmpty(x2.Text) && !string.IsNullOrEmpty(y1.Text) &&
                !string.IsNullOrEmpty(y2.Text)
            )
            {
                var curve = new EllipticCurve(aTextBox.Text.ToInt(), bTextBox.Text.ToInt(), pTextBox.Text.ToInt());

                try
                {
                    var res = curve.Add(new Point(x1.Text.ToInt(), y1.Text.ToInt()),
                        new Point(x2.Text.ToInt(), y2.Text.ToInt()));
                    result.Text = $"{res.X}, {res.Y}";
                }
                catch (Exception ex)
                {
                    result.Text = "";
                }
            }
        }
    }

    public static class StringExtensions
    {
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }
    }
}