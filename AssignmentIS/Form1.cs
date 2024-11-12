using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;


namespace AssignmentIS
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed,   imageB, imageA, resultImage;
        private FilterInfoCollection videoDevices; // List of available webcams
        private VideoCaptureDevice videoSource;    // The selected webcam


        public Form1()
        {
            InitializeComponent();

            // Populate ComboBox with filter options
            comboBoxFilters.Items.AddRange(new string[]
            {
        "Edge Detection", "Sharpen", "Gaussian Blur", "Mean Removal", "Embossing"
            });
            comboBoxFilters.SelectedIndex = 0; // Set default selection
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
        // hide atm to try to use webcam to get imageB
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    openFileDialog2.ShowDialog();
        //}



        private void button1_Click(object sender, EventArgs e)
        {
            // Check for available video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No webcam found.");
                return;
            }

            // Use the first available webcam
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);

            // Set the NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
            captureButton.Visible = true;

            // Start capturing video
            videoSource.Start();
        }

        private void captureButton_Click(object sender, EventArgs e)
        {
            if (pictureBox3.Image != null)
            {
                // Capture the current frame displayed in the PictureBox
                imageB = new Bitmap(pictureBox3.Image);

                // Stop the video source to "freeze" the image
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }

                // Display the captured image in pictureBox3
                pictureBox3.Image = imageB;

                // Optionally hide the capture button after capturing
                captureButton.Visible = false;

                // Optionally, display a message or log to confirm the capture
                MessageBox.Show("Image captured and assigned to imageB!");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ensure the video source is properly stopped when the form is closed
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }



        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Display the current frame in the PictureBox
            imageB = (Bitmap)eventArgs.Frame.Clone();
            pictureBox3.Image = imageB;
        }


        //hidden atm 
        //private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        //{
        //    imageB = new Bitmap(openFileDialog2.FileName);
        //    pictureBox3.Image = imageB;
        //}


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
            // Ensure that both images are non-null and have dimensions
            if (imageA == null || imageB == null)
            {
                MessageBox.Show("Both images must be loaded.");
                return;
            }

            // Use the minimum width and height to avoid out-of-bounds issues
            int minWidth = Math.Min(imageA.Width, imageB.Width);
            int minHeight = Math.Min(imageA.Height, imageB.Height);
            resultImage = new Bitmap(minWidth, minHeight);

            Color mygreen = Color.FromArgb(0, 255, 0); // Chroma key green color
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 60;

            for (int x = 0; x < minWidth; x++)
            {
                for (int y = 0; y < minHeight; y++)
                {
                    // Retrieve the pixel colors from both images
                    Color pixel = imageB.GetPixel(x, y);
                    Color backPixel = imageA.GetPixel(x, y);

                    // Calculate grayscale value for chroma keying
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;  // Only use `pixel`, not `backPixel.B`
                    int subtractvalue = Math.Abs(grey - greygreen);

                    // If the difference is greater than the threshold, use the background pixel
                    if (subtractvalue > threshold)
                        resultImage.SetPixel(x, y, backPixel);
                    else
                        resultImage.SetPixel(x, y, pixel);
                }
            }

            // Display the result image
            pictureBox5.Image = resultImage;
        }

        private void convolutionFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyConvolutionFilter(comboBoxFilters.SelectedItem.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ApplyConvolutionFilter(comboBoxFilters.SelectedItem.ToString());
        }



        private double[,] GetKernel(string kernelName)
        {
            switch (kernelName)
            {
                case "Edge Detection":
                    return new double[,]
                    {
                { -1, -1, -1 },
                { -1,  8, -1 },
                { -1, -1, -1 }
                    };

                case "Sharpen":
                    return new double[,]
                    {
                { 0, -1,  0 },
                { -1,  5, -1 },
                { 0, -1,  0 }
                    };

                case "Gaussian Blur":
                    return new double[,]
                    {
                { 0.0625, 0.125, 0.0625 },
                { 0.125,  0.25,  0.125 },
                { 0.0625, 0.125, 0.0625 }
                    };

                case "Mean Removal":
                    return new double[,]
                    {
                { -1, -1, -1 },
                { -1,  9, -1 },
                { -1, -1, -1 }
                    };

                case "Embossing":
                    return new double[,]
                    {
                { -2, -1,  0 },
                { -1,  1,  1 },
                {  0,  1,  2 }
                    };

                default:
                    // Return an identity kernel if the filter name doesn't match
                    return new double[,]
                    {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 }
                    };
            }
        }


        private void ApplyConvolutionFilter(string kernelName)
        {
            if (loaded == null)
            {
                MessageBox.Show("No image loaded.");
                return;
            }

            double[,] kernel = GetKernel(kernelName);
            int kernelSize = kernel.GetLength(0);
            int offset = kernelSize / 2;
            processed = new Bitmap(loaded.Width, loaded.Height);

            for (int x = offset; x < loaded.Width - offset; x++)
            {
                for (int y = offset; y < loaded.Height - offset; y++)
                {
                    double red = 0, green = 0, blue = 0;

                    for (int i = 0; i < kernelSize; i++)
                    {
                        for (int j = 0; j < kernelSize; j++)
                        {
                            int pixelX = x + i - offset;
                            int pixelY = y + j - offset;
                            Color pixel = loaded.GetPixel(pixelX, pixelY);

                            red += pixel.R * kernel[i, j];
                            green += pixel.G * kernel[i, j];
                            blue += pixel.B * kernel[i, j];
                        }
                    }

                    int r = Math.Min(Math.Max((int)red, 0), 255);
                    int g = Math.Min(Math.Max((int)green, 0), 255);
                    int b = Math.Min(Math.Max((int)blue, 0), 255);

                    processed.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            pictureBox2.Image = processed;
        }





    }
}
