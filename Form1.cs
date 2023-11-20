using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class Form1 : Form
    {
        Bitmap openImg, openImg_8_1, openImg_8_2;
        Point[] tarimg_point = new Point[4];
        Point[] inimg_point = new Point[4];
        int tarimg_index = 0;
        int inimg_index = 0;
        public Bitmap Img { get; private set; }
        public Color Color { get; private set; }
        Stack sk = new Stack();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //load
        {
            openFileDialog1.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openImg = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = new Bitmap (openImg);
                sk.Push(openImg);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)  //save
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox2.Image.Save(sfd.FileName);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)  //R channel
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            for(int y = 0; y < Img.Height; y++)
            {
                for(int x = 0; x < Img.Width; x++)
                {
                    Color RGB = Img.GetPixel(x, y);
                    Img.SetPixel(x, y, Color.FromArgb(RGB.R, RGB.R, RGB.R));
                }
            }
            pictureBox2.Image = Img;
            sk.Push(Img);
        }

        private void button4_Click(object sender, EventArgs e)  //G channel
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            for (int y = 0; y < Img.Height; y++)
            {
                for (int x = 0; x < Img.Width; x++)
                {
                    Color RGB = Img.GetPixel(x, y);
                    Img.SetPixel(x, y, Color.FromArgb(RGB.G, RGB.G, RGB.G));
                }
            }
            pictureBox2.Image = Img;
            sk.Push(Img);
        }

        private void button5_Click(object sender, EventArgs e)  //B channel
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            for (int y = 0; y < Img.Height; y++)
            {
                for (int x = 0; x < Img.Width; x++)
                {
                    Color RGB = Img.GetPixel(x, y);
                    Img.SetPixel(x, y, Color.FromArgb(RGB.B, RGB.B, RGB.B));
                }
            }
            pictureBox2.Image = Img;
            sk.Push(Img);
        }

        private void button8_Click(object sender, EventArgs e)  //Grayscale
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap grayimg = get_gray_img(Img);
            pictureBox2.Image = grayimg;
            sk.Push(grayimg);
        }

        private Bitmap get_gray_img(Bitmap Img)
        {
            Bitmap newImg = (Bitmap)Img.Clone();
            for(int y = 0; y < Img.Height; y++)
            {
                for(int x = 0; x < Img.Width; x++)
                {
                    Color RGB = Img.GetPixel(x, y);
                    int R = Convert.ToInt32(RGB.R) * 299 / 1000;
                    int G = Convert.ToInt32(RGB.G) * 587 / 1000;
                    int B = Convert.ToInt32(RGB.B) * 114 / 1000;
                    int ret = R + G + B;
                    newImg.SetPixel(x, y, Color.FromArgb(ret, ret, ret));
                }
            }
            return newImg;
        }


        private void button7_Click(object sender, EventArgs e)
        {
            if ((int)sk.Count > 1)
            {
                Bitmap img = (Bitmap)sk.Pop();
                pictureBox1.Image = (Bitmap)sk.Peek();
                pictureBox2.Image = null;
            }
            else
            {
                MessageBox.Show("cannot undo");
            }
        }

        private void button6_Click(object sender, EventArgs e)  //mean
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap blur = Mean_blur(Img);
            pictureBox2.Image = blur;
            sk.Push(blur);
        }
        private Bitmap Mean_blur(Bitmap Img)
        {
            Bitmap newImg = (Bitmap)Img.Clone();
            newImg = get_gray_img(newImg);
            double a = ((double)1 / (double)9);
            double[] kernel = new double[9];
            for (int i = 0; i < 9; i++){
                kernel[i] = a;
            }
            newImg = Conv(Img, kernel);
            return newImg;
        }
        private Bitmap Conv(Bitmap Img, double[] kernel)
        {
            Color[] current_3X3 = new Color[3 * 3];
            Bitmap new_Img = (Bitmap)Img.Clone();



            new_Img = zeropad(new_Img);
            double p_r = 0;
            int p = 0;
            for(int y = 1; y <Img.Height-1; y++)
            {
                for(int 
                    
                    x = 1; x <Img.Width-1; x++)
                {
                    Color RGB = Img.GetPixel(x, y);
                    current_3X3[0] = Img.GetPixel(x - 1, y - 1);
                    current_3X3[1] = Img.GetPixel(x, y - 1);
                    current_3X3[2] = Img.GetPixel(x + 1, y - 1);
                    current_3X3[3] = Img.GetPixel(x - 1, y);
                    current_3X3[4] = Img.GetPixel(x, y);
                    current_3X3[5] = Img.GetPixel(x + 1, y);
                    current_3X3[6] = Img.GetPixel(x - 1, y + 1);
                    current_3X3[7] = Img.GetPixel(x, y + 1);
                    current_3X3[8] = Img.GetPixel(x + 1, y + 1);
                    for(int i = 0; i < 9; i++)
                    {
                        p_r += (double)(current_3X3[i].R) * kernel[i];
                    }
                    p = (int)p_r;
                    if(p > 255)
                    {
                        p = 255;
                    }
                    else if (p < 0)
                    {
                        p = 0;
                    }
                    new_Img.SetPixel(x, y, Color.FromArgb(p, p, p));
                    p_r = 0;
                }
            }
            return new_Img;
        }
        private Bitmap zeropad(Bitmap Img)
        {
            Bitmap out_Img = (Bitmap)Img.Clone();
            for(int y = 0; y < Img.Height; y++)
            {
                out_Img.SetPixel(0, y, Color.FromArgb(0, 0, 0));
                out_Img.SetPixel(Img.Width - 1, y, Color.FromArgb(0, 0, 0));
            }
            for(int x = 0;x < Img.Width; x++)
            {
                out_Img.SetPixel(x, 0, Color.FromArgb(0, 0, 0));
                out_Img.SetPixel(x, Img.Height - 1, Color.FromArgb(0, 0, 0));
            }
            return out_Img;
        }

        private void button9_Click(object sender, EventArgs e)  //median
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap med_blur = Median_blur(Img);
            pictureBox2.Image = med_blur;
            sk.Push(med_blur);
        }
        private Bitmap Median_blur(Bitmap Img)
        {
            Bitmap newImg = (Bitmap)Img.Clone();
            newImg = get_gray_img(newImg);
            double[] current_3X3 = new double[9];
            //double[] current_3X3_sort = new double[9];
            //int p = 0;
            double median ;
            for(int y = 1;y < Img.Height - 1; y++)
            {
                for(int x = 1;x < Img.Width - 1; x++)
                {
                    //Color RGB = Img.GetPixel(x, y);
                    current_3X3[0] = Img.GetPixel(x - 1, y - 1).R;
                    current_3X3[1] = Img.GetPixel(x, y - 1).R;
                    current_3X3[2] = Img.GetPixel(x + 1, y - 1).R;
                    current_3X3[3] = Img.GetPixel(x - 1, y).R;
                    current_3X3[4] = Img.GetPixel(x, y).R;
                    current_3X3[5] = Img.GetPixel(x + 1, y).R;
                    current_3X3[6] = Img.GetPixel(x - 1, y + 1).R;
                    current_3X3[7] = Img.GetPixel(x, y + 1).R;
                    current_3X3[8] = Img.GetPixel(x + 1, y + 1).R;
                    double [] current_3X3_sort = (double[])current_3X3.Clone();
                    Array.Sort(current_3X3_sort);
                    median = current_3X3_sort[4];
                    int p = (int)median;
                    newImg.SetPixel(x, y, Color.FromArgb(p, p, p));
                }
            }
            return newImg;



        }

        private void button10_Click(object sender, EventArgs e) //Histogram Equalization
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap he = Histogram(Img);
            Bitmap grayimg = get_gray_img(Img);
            pictureBox1.Image = grayimg;
            pictureBox2.Image = he;
            sk.Push(he);
        }

        private Bitmap Histogram(Bitmap bitmap)
        {
            Bitmap grayimg = get_gray_img(bitmap);
            Bitmap newImg = (Bitmap)grayimg.Clone();
            double[] gscale_list = new double[256];
            double[] prob_gscale = new double[256];
            for(int i = 0; i < 256; i++)
            {
                gscale_list[i] = 0;
                prob_gscale[i] = 0;
            }
            for(int y = 0; y < grayimg.Height; y++)
            {
                for(int x = 0; x < grayimg.Width; x++)
                {
                    Color color = grayimg.GetPixel(x, y);
                    gscale_list[color.R] += 1;
                }
            }
            for(int k = 0; k < 256; k++)
            {
                chart1.Series[0].Points.AddXY(k + 1, gscale_list[k]);
                prob_gscale[k] = gscale_list[k] / (grayimg.Height * grayimg.Width);
            }

            double prob_sum = 0;
            for(int y = 0; y < grayimg.Height; y++)
            {
                for(int x = 0; x < grayimg.Width; x++)
                {
                    for(int i = 0; i < grayimg.GetPixel(x, y).R; i++)
                    {
                        prob_sum += prob_gscale[i];
                    }
                    int new_g = Convert.ToInt32(Math.Round(prob_sum * 255, 0, MidpointRounding.AwayFromZero));
                    newImg.SetPixel(x, y, Color.FromArgb(new_g, new_g, new_g));
                    prob_sum = 0;
                }
            }
            double[] gscale_list2 = new double[256];
            for (int y = 0; y < newImg.Height; y++)
            {
                for (int x = 0; x < newImg.Width; x++)
                {
                    Color color = newImg.GetPixel(x, y);
                    gscale_list2[color.R] += 1;
                }
            }
            for (int k = 0; k < 256; k++)
            {
                chart2.Series[0].Points.AddXY(k + 1, gscale_list2[k]);
                //prob_gscale[k] = gscale_list2[k] / (grayimg.Height * grayimg.Width);
            }
            return newImg;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int value = trackBar1.Value;
            textBox1.Text = "" + trackBar1.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //int i = 0;
            //int value = trackBar1.Value;
            // textBox1.Text = ""+ trackBar1.Value;
        }

        private void button11_Click(object sender, EventArgs e) //thresholding
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            int threshold_value = trackBar1.Value;
            Bitmap thresimg = thresholding(Img, threshold_value);
            pictureBox2.Image = thresimg;
            sk.Push(thresimg);
        }
        
        private Bitmap thresholding(Bitmap Img, int value)
        {
            Bitmap grayimg = get_gray_img(Img);
            Bitmap newImg = (Bitmap)grayimg.Clone();
            for(int y = 0; y < grayimg.Height; y++)
            {
                for(int x = 0; x < grayimg.Width; x++)
                {
                    Color color = grayimg.GetPixel(x, y);
                    if(Convert.ToInt32(color.R) < value)
                    {
                        newImg.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        newImg.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                }
            }
            return newImg;
        }

        private void button12_Click(object sender, EventArgs e) //vertical
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap verimg = vertical(Img);
            pictureBox2.Image = verimg;
            sk.Push(verimg);
        }
        private Bitmap vertical(Bitmap bitmap)
        {
            double[] array = new double[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            Bitmap median = Median_blur(bitmap);
            median = Conv(median, array);
            for(int y = 0; y < median.Height; y++)
            {
                for(int x = 0; x < median.Width; x++)
                {
                    int c = Convert.ToInt32(median.GetPixel(x, y).R);
                    if(c < 0)
                    {
                        median.SetPixel(x, y, Color.FromArgb(-1 * c , -1 * c , -1 * c ));
                    }
                }
            }
            return median;
        }

        private void button13_Click(object sender, EventArgs e) //Horizontal
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap horimg = horizontal(Img);
            pictureBox2.Image = horimg;
            sk.Push(horimg);
        }

        private Bitmap horizontal(Bitmap bitmap)
        {
            double[] array = new double[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
            Bitmap median = Median_blur(bitmap);
            median = Conv(median, array);
            for (int y = 0; y < median.Height; y++)
            {
                for (int x = 0; x < median.Width; x++)
                {
                    int c = Convert.ToInt32(median.GetPixel(x, y).R);
                    if (c < 0)
                    {
                        median.SetPixel(x, y, Color.FromArgb(-1 * c, -1 * c, -1 * c));
                    }
                }
            }
            return median;
        }

        private void button14_Click(object sender, EventArgs e) //combined
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap combimg = combined(Img);
            pictureBox2.Image = combimg;
            sk.Push(combimg);
        }

        private Bitmap combined(Bitmap bitmap)
        {
            Bitmap newImg = (Bitmap)bitmap.Clone();
            newImg = zeropad(newImg);
            Bitmap img_V = vertical(bitmap);
            Bitmap img_H = horizontal(bitmap);
            for(int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    int v_c = Convert.ToInt32(img_V.GetPixel(x, y).R);
                    int h_c = Convert.ToInt32(img_H.GetPixel(x, y).R);
                    double res_c = Math.Sqrt(v_c * v_c + h_c * h_c);
                    int c = Convert.ToInt32(res_c);
                    if (c > 255)
                    {
                        c = 255;
                    }
                    newImg.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }
            return newImg;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            int value = trackBar3.Value;
            textBox3.Text = "" + trackBar3.Value;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e) //edge overlap
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap edge = edgeoverlapping(Img);
            pictureBox2.Image = edge;
            sk.Push(edge);
        }

        private Bitmap edgeoverlapping(Bitmap bitmap)
        {
            Bitmap newImg = (Bitmap)bitmap.Clone();
            Bitmap comb = combined(bitmap);
            //Bitmap median = Median_blur(bitmap);
            Bitmap thres = thresholding(comb, Convert.ToInt32(trackBar3.Value));
            for(int y = 0; y < bitmap.Height; y++)
            {
                for(int x = 0; x < bitmap.Width; x++)
                {
                    if(thres.GetPixel(x, y).R == 255)
                    {
                        newImg.SetPixel(x, y, Color.FromArgb(0, 255, 0));
                    }
                }
            }
            return newImg;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e) //connected component
        {
            Bitmap in_img = (Bitmap)sk.Peek();
            pictureBox1.Image = in_img;
            Bitmap Img = (Bitmap)in_img.Clone();
            Bitmap connect = new Bitmap(Img.Width, Img.Height);
            for(int y = 0; y < openImg.Height; y++)
            {
                for(int x = 0; x < openImg.Width; x++)
                {
                    Color c = Img.GetPixel(x, y);
                    connect.SetPixel(x, y, c);
                }
            }
            Color[] con = new Color[9];
            con[0] = Color.Gray;
            con[1] = Color.Blue;
            con[2] = Color.Red;
            con[3] = Color.Green;
            con[4] = Color.Yellow;
            con[5] = Color.Orange;
            con[6] = Color.Purple;
            con[7] = Color.Pink;
            con[8] = Color.Salmon;

            Queue<Point> q = new Queue<Point>();
            int regioncolor = 0;
            int rcolor = 0;
            Color pixel1 = new Color();
            Color pixel2 = new Color();
            Point p = new Point(0, 0);
            Point rcheck = new Point(0, 0);
            Point result = new Point(0, 0);

            for(int x = 1; x < (openImg.Width); x++)
            {
                for(int y = 1; y < (openImg.Height); y++)
                {
                    pixel1 = connect.GetPixel(x, y);
                    if(pixel1 == Color.FromArgb(0, 0, 0))
                    {
                        regioncolor++;
                        rcolor = regioncolor % 9;
                        p.X = x;
                        p.Y = y;
                        q.Enqueue(p);
                        while(q.Count != 0)
                        {
                            rcheck = q.Dequeue();
                            Console.WriteLine(rcheck.X);
                            Console.WriteLine(rcheck.Y);
                            pixel2 = connect.GetPixel(rcheck.X, rcheck.Y);
                            for(int i = -1; i <= 1; i++)
                            {
                                for(int j = -1; j <= 1; j++)
                                {
                                    if((rcheck.X + i) < 0 || (rcheck.X + i) > Img.Width - 1 || (rcheck.Y + j) < 0 || (rcheck.Y + j) > Img.Height - 1)
                                        break;
                                    if(pixel2 == Color.FromArgb(0, 0, 0))
                                    {
                                        result.X = rcheck.X + i;
                                        result.Y = rcheck.Y + j;
                                        q.Enqueue(result);
                                        connect.SetPixel(rcheck.X, rcheck.Y, con[rcolor]);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            sk.Push(connect);
            pictureBox2.Image = connect;

            textBox4.Text = Convert.ToString(regioncolor);
        }

        private void groupBox14_Enter(object sender, EventArgs e)
        {

        }
        int pic3_flag = 0;
        private void pictureBox3_Click(object sender, EventArgs e) //target
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            Point coordinates = mouse.Location;
            tarimg_point[tarimg_index] = coordinates;
            tarimg_index++;
            if (tarimg_index >= 4)
            {
                tarimg_index = 0;
                MessageBox.Show("Target image already samples 4 points");
                pic3_flag = 1;
            }
        }

        int pic4_flag = 0;
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            Point coordinates = mouse.Location;
            inimg_point[inimg_index] = coordinates;
            inimg_index++;
            if (inimg_index >= 4)
            {
                inimg_index = 0;
                MessageBox.Show("Input image already samples 4 points");
                pic4_flag = 1;
            }
        }

        private void button20_Click(object sender, EventArgs e) //image registration
        {
            if((pic3_flag + pic4_flag) != 2)
            {
                MessageBox.Show("8 points not complete");
                return;
            }
            double scale, theta, diff;
            theta = get_theta();
            scale = get_scale(0, 2);


            Bitmap outimg = backward_rotation(openImg_8_2, theta, 107, -61);
            Bitmap outimg_2 = backward_scale(outimg, scale, scale);
            pictureBox2.Image = outimg_2;
            sk.Push(openImg_8_2);
            sk.Push(outimg_2);
            diff = difference(openImg_8_1, outimg_2);
            textBox5.Text = Convert.ToString(scale);
            textBox6.Text = Convert.ToString(theta);
            textBox7.Text = Convert.ToString(diff);


        }
        private double get_scale(int a, int b)
        {
            double scale;
            int x, y;
            double d1, d2;
            x = Math.Abs(tarimg_point[a].X - tarimg_point[b].X);
            y = Math.Abs(tarimg_point[a].Y - tarimg_point[b].Y);
            d1 = Math.Sqrt(x * x + y * y);
            x = Math.Abs(inimg_point[a].X - inimg_point[b].X);
            y = Math.Abs(inimg_point[a].Y - inimg_point[b].Y);
            d2 = Math.Sqrt(x * x + y * y);

            scale = d2 / d1;
            return scale;
        }

        private double get_theta()
        {
            int x, y;
            x = Math.Abs(tarimg_point[1].X - tarimg_point[2].X);
            y = Math.Abs(tarimg_point[1].Y - tarimg_point[2].Y);
            double theta_1 = Math.Atan2((double)y, (double)x);
            theta_1 = theta_1 * (180 / Math.PI);
            x = Math.Abs(inimg_point[1].X - inimg_point[2].X);
            y = Math.Abs(inimg_point[1].Y - inimg_point[2].Y);
            double theta_2 = Math.Atan2((double)y, (double)x);
            theta_2 = theta_2 * (180 / Math.PI);
            double theta = theta_2 - theta_1;
            return theta;
        }

        private Bitmap backward_rotation(Bitmap bitmap, double theta, int a, int b)
        {
            theta = theta * (Math.PI / (double)180);
            Bitmap newImg = new Bitmap(openImg_8_1.Width, openImg_8_1.Height);
            int old_x = 0;
            int old_y = 0;
            Color old_color;
            for (int new_x = 0; new_x < newImg.Width; new_x++)
            {
                for (int new_y = 0; new_y < newImg.Height; new_y++)
                {
                    old_x = Convert.ToInt32(((double)new_x + a) * Math.Cos(theta) - ((double)new_y + b) * Math.Sin(theta));

                    old_y = Convert.ToInt32(((double)new_x + a) * Math.Sin(theta) + ((double)new_y + b) * Math.Cos(theta));

                    if (inside_border(bitmap, old_x, old_y))
                    {
                        old_color = bitmap.GetPixel(old_x, old_y);
                        newImg.SetPixel(new_x, new_y, Color.FromArgb(old_color.R, old_color.G, old_color.B));
                    }
                    else
                    {
                        newImg.SetPixel(new_x, new_y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return newImg;
        }

        private Bitmap backward_scale(Bitmap bitmap, double scale_x, double scale_y)
        {
            Bitmap newImg = new Bitmap(openImg_8_1.Width, openImg_8_1.Height);
            Color old_c;
            int old_x = 0;
            int old_y = 0;
            for (int new_x = 0; new_x < newImg.Width; new_x++)
            {
                for (int new_y = 0; new_y < newImg.Height; new_y++)
                {
                    old_x = Convert.ToInt32((double)new_x * scale_x);
                    old_y = Convert.ToInt32((double)new_y * scale_y);
                    if (inside_border(bitmap, old_x, old_y))
                    {
                        old_c = bitmap.GetPixel(old_x, old_y);
                        newImg.SetPixel(new_x, new_y, Color.FromArgb(old_c.R, old_c.G, old_c.B));
                    }
                    else
                    {
                        newImg.SetPixel(new_x, new_y, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return newImg;
        }

        private double difference(Bitmap bitmap_1, Bitmap bitmap_2)
        {
            double d = 0;
            double diff;
            int w = (bitmap_1.Width > bitmap_2.Width) ? bitmap_2.Width : bitmap_1.Width;
            int h = (bitmap_1.Height > bitmap_2.Height) ? bitmap_2.Height : bitmap_1.Height;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    d += Math.Abs(bitmap_1.GetPixel(x, y).R - bitmap_2.GetPixel(x, y).R);
                }
            }
            diff = d / (bitmap_1.Width * bitmap_1.Height);
            return diff;
        }

        private bool inside_border(Bitmap bitmap, int x, int y)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            if ((x > 0 && x < w) && (y > 0 && y < h))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)     //load input image
        {
            openFileDialog1.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openImg_8_2 = new Bitmap(openFileDialog1.FileName);
                pictureBox4.Image = openImg_8_2;
                MessageBox.Show("Please sample for sequence from top left through counterclockwise");
            }
        }

        private void button18_Click(object sender, EventArgs e) //load target image
        {
            openFileDialog1.Filter = "All Files|*.*|Bitmap Files (.bmp)|*.bmp|Jpeg File(.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openImg_8_1 = new Bitmap(openFileDialog1.FileName);
                pictureBox3.Image = openImg_8_1;
                MessageBox.Show("Please sample for sequence from top left through counterclockwise");
            }
        }
    }
}
