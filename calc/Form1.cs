using System;
using System.Windows.Forms;

namespace calc
{
    public partial class Form1 : Form
    {
         public Form1()
        {
            InitializeComponent();
        }

        private void calc_btn_Click(object sender, EventArgs e)
        {
            int[] velocities_nv = new int[] { 100, 130, 160, 400 };
            string[] results_sv = new string[4];    //store possible angles for every velocity

            /*--user input--*/
            double distance_ns = Convert.ToDouble(distance_textbox.Text);
            double currentHeight_ns = Convert.ToDouble(current_height_textbox.Text);
            double targetHeight_ns = Convert.ToDouble(target_height_textbox.Text);

            /*--calculate--*/
            double heightDiff_ns = currentHeight_ns - targetHeight_ns;
            bool isTargetHigher_bs = heightDiff_ns < 0 ? true : false;
            heightDiff_ns = Math.Abs(heightDiff_ns);
            double g = 9.8;

            for (int i = 0; i < results_sv.Length; i++)
            {
            
                double[] anglesRad_vn = new double[2];
                double[] anglesDegree_vn = new double[2];

                /*--same level--*/
                if (heightDiff_ns == 0)
                {
                    //rad = (sin^-1(s*g/v²))/2          //if greater than 1, the target can't be reached no matter what the angle is
                    anglesRad_vn[0] = Math.Asin(distance_ns * g / Math.Pow(velocities_nv[i], 2)) / 2;
                    anglesRad_vn[1] = Math.PI / 2 - anglesRad_vn[0];
                }
                /*--lower level--*/
                else if (heightDiff_ns != 0 && isTargetHigher_bs == false)
                {
                    double ß = g * Math.Pow(distance_ns, 2) / Math.Pow(velocities_nv[i], 2);
                    double phi = Math.Atan(distance_ns / heightDiff_ns);
                    double angle = (Math.Acos((ß - heightDiff_ns) / Math.Sqrt(Math.Pow(heightDiff_ns, 2) + Math.Pow(distance_ns, 2))) + phi) / 2;

                    anglesRad_vn[0] = Math.Abs(angle);
                    anglesRad_vn[1] = Math.PI / 2 - anglesRad_vn[0];
                    //label15.Text = Convert.ToString(Math.Abs(angle));
                }
                /*--higher level--*/
                //0=distance*tan(theta)-(g/2*t^2)tan^2(theta)-(g/2*t^2)
                else if (heightDiff_ns != 0 && isTargetHigher_bs == true)
                { 
                    double t = distance_ns / velocities_nv[i];

                    double a = g / 2 * Math.Pow(t, 2);
                    double b = distance_ns * -1;
                    double c = g / 2 * Math.Pow(t, 2) + heightDiff_ns;

                    double sqrtPart_ns = Math.Pow(b, 2) - (4 * a * c);
                    double root1_ns = ((-1) * b + Math.Sqrt(sqrtPart_ns)) / (2 * a);
                    double root2_ns = ((-1) * b - Math.Sqrt(sqrtPart_ns)) / (2 * a);    //tan(theta) = root theta=?
                            //label16.Text = $"{root1_ns} és {root2_ns}";     

                    anglesRad_vn[0] = Math.Atan(root1_ns);
                    anglesRad_vn[1] = Math.Atan(root2_ns);

                            //label16.Text = $"{Math.Atan( root1_ns )*180/Math.PI} és {Math.Atan( root2_ns)*180/Math.PI}";
                }

                /*--output--*/
                if (isTargetHigher_bs == false)
                {
                    if (anglesRad_vn[0] >= 0 && anglesRad_vn[0] <= 1)   //sin és cos max -1 1 lehet
                    {
                        anglesDegree_vn[0] = radToDeg(anglesRad_vn[0]);
                        anglesDegree_vn[1] = radToDeg(anglesRad_vn[1]);

                        results_sv[i] = $"{ toDegMinSecFormat(anglesDegree_vn[0]) } or { toDegMinSecFormat(anglesDegree_vn[1]) }";
                    }
                    else
                    {
                        results_sv[i] = "target can't be reached at this speed";
                    }
                }
                else if (isTargetHigher_bs == true)
                {
                    anglesDegree_vn[0] = radToDeg(anglesRad_vn[0]);
                    anglesDegree_vn[1] = radToDeg(anglesRad_vn[1]);
                    
                    if(anglesDegree_vn[0] <=0 || Double.IsNaN(anglesDegree_vn[0]) ) results_sv[i] = "target can't be reached at this speed"; //ha minusz fok jön ki
                    else results_sv[i] = $"{ toDegMinSecFormat(anglesDegree_vn[0]) } or { toDegMinSecFormat(anglesDegree_vn[1]) }";
                }
            }

            label10.Text = results_sv[0];
            label11.Text = results_sv[1];
            label12.Text = results_sv[2];
            label13.Text = results_sv[3];
        }//event ends

        private string toDegMinSecFormat(double p_inDecimal_ns)
        {
            string result_ss;

            int sec_ns = (int)Math.Round(p_inDecimal_ns * 3600);
            int deg_ns = sec_ns / 3600;
            sec_ns = Math.Abs(sec_ns % 3600);
            int min_ns = sec_ns / 60;
            sec_ns %= 60;
            result_ss = $"{deg_ns}°{min_ns}'{sec_ns}''";
            return result_ss;
        }

        private double radToDeg(double p_rad_ns)
        {
            return p_rad_ns * 180 / Math.PI;
        }
    }
}
