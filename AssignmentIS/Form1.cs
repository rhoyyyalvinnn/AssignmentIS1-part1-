﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AssignmentIS
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed, imageB, imageA, resultImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }


        private void pixelCopyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }



        private void greyscalingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave; 
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (pixel.R + pixel.G + pixel.B) / 3;
                    Color gray = Color.FromArgb(ave, ave, ave);
                    processed.SetPixel(x, y, gray);

                }
            }
            pictureBox2.Image = processed; 
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    Color inv = Color.FromArgb(255-pixel.R, 255-pixel.G, 255-pixel.B);
                    processed.SetPixel(x, y, inv);

                }
            }
            pictureBox2.Image = processed;
        }



        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }









        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);

                    // Calculate new sepia values based on current pixel color
                    int tr = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int tg = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int tb = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    // Ensure values are within the valid range [0, 255]
                    tr = tr > 255 ? 255 : tr;
                    tg = tg > 255 ? 255 : tg;
                    tb = tb > 255 ? 255 : tb;

                    Color sepiaColor = Color.FromArgb(tr, tg, tb);
                    processed.SetPixel(x, y, sepiaColor);
                }
            }
            pictureBox2.Image = processed;
        }



        //PART 2 of the assignment

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox3.Image = imageB;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox4.Image = imageA;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            resultImage = new Bitmap(imageB.Width, imageB.Height);
            Color mygreen = Color.FromArgb(0, 255, 0);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 60;

            for (int x = 0; x < imageB.Width; x++) 
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backPixel = imageA.GetPixel(x, y); 
                    int grey = (pixel.R + pixel.G + backPixel.B) / 2;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue > threshold)
                        resultImage.SetPixel(x, y, backPixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            }
            pictureBox5.Image = resultImage;
        }











    }
}
