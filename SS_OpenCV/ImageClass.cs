using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Diagnostics.Contracts;

namespace CG_OpenCV
{
    class ImageClass
    {

        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        /// 

        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            (dataPtr + nChan * x + step * y)[0] = (byte)(255 - (dataPtr + nChan * x + step * y)[0]);
                            (dataPtr + nChan * x + step * y)[1] = (byte)(255 - (dataPtr + nChan * x + step * y)[1]);
                            (dataPtr + nChan * x + step * y)[2] = (byte)(255 - (dataPtr + nChan * x + step * y)[2]);

                        }
                    }
                }
            }
        }

       
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //retrive 3 colour components
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];

                            // convert to gray
                            gray = (byte)Math.Round(((int)blue + green + red) / 3.0);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }


        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int valor;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            //BLUE
                            valor = (int)Math.Round((dataPtr + nChan * x + step * y)[0] * contrast + bright);

                            if (valor < 0)
                            {
                                valor = 0;
                            }
                            else if (valor > 255)
                            {
                                valor = 255;
                            }

                            (dataPtr + nChan * x + step * y)[0] = (byte)valor;


                            //GREEN
                            valor = (int)Math.Round((dataPtr + nChan * x + step * y)[1] * contrast + bright);

                            if (valor < 0)
                            {
                                valor = 0;
                            }
                            else if (valor > 255)
                            {
                                valor = 255;
                            }

                            (dataPtr + nChan * x + step * y)[1] = (byte)valor;


                            //RED
                            valor = (int)Math.Round((dataPtr + nChan * x + step * y)[2] * contrast + bright);

                            if (valor < 0)
                            {
                                valor = 0;
                            }
                            else if (valor > 255)
                            {
                                valor = 255;
                            }

                            (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                        }
                    }
                }
            }
        }


        public static void RedChannel(Image<Bgr, byte> img)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int valor;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            (dataPtr + nChan * x + step * y)[0] = (dataPtr + nChan * x + step * y)[2];
                            (dataPtr + nChan * x + step * y)[1] = (dataPtr + nChan * x + step * y)[2];
                        }
                    }
                }
            }
        }


        public static void Mean(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                MIplImage copy = imgCopy.MIplImage;
                byte* copyPtr = (byte*)copy.imageData.ToPointer();

                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int valor;

                if (nChan == 3) // image in RGB
                {

                    //Mean of Core
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[0] + (copyPtr + nChan * (x) + step * (y - 1))[0] + (copyPtr + nChan * (x + 1) + step * (y - 1))[0] + (copyPtr + nChan * (x - 1) + step * (y))[0] + (copyPtr + nChan * (x) + step * (y))[0] + (copyPtr + nChan * (x + 1) + step * (y))[0] + (copyPtr + nChan * (x - 1) + step * (y + 1))[0] + (copyPtr + nChan * (x) + step * (y + 1))[0] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0]) / 9.0);
                            (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[1] + (copyPtr + nChan * (x) + step * (y - 1))[1] + (copyPtr + nChan * (x + 1) + step * (y - 1))[1] + (copyPtr + nChan * (x - 1) + step * (y))[1] + (copyPtr + nChan * (x) + step * (y))[1] + (copyPtr + nChan * (x + 1) + step * (y))[1] + (copyPtr + nChan * (x - 1) + step * (y + 1))[1] + (copyPtr + nChan * (x) + step * (y + 1))[1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1]) / 9.0);
                            (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[2] + (copyPtr + nChan * (x) + step * (y - 1))[2] + (copyPtr + nChan * (x + 1) + step * (y - 1))[2] + (copyPtr + nChan * (x - 1) + step * (y))[2] + (copyPtr + nChan * (x) + step * (y))[2] + (copyPtr + nChan * (x + 1) + step * (y))[2] + (copyPtr + nChan * (x - 1) + step * (y + 1))[2] + (copyPtr + nChan * (x) + step * (y + 1))[2] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2]) / 9.0);
                        }
                    }


                    //Mean of Top and Bottom Border
                    for (x = 1; x < width - 1; x++)
                    {
                        y = 0;
                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round((((copyPtr + nChan * (x - 1) + step * (y))[0] * 2 + (copyPtr + nChan * (x) + step * (y))[0] * 2 + (copyPtr + nChan * (x + 1) + step * (y))[0] * 2 + (copyPtr + nChan * (x - 1) + step * (y + 1))[0] + (copyPtr + nChan * (x) + step * (y + 1))[0] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0]) / 9.0));
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round((((copyPtr + nChan * (x - 1) + step * (y))[1] * 2 + (copyPtr + nChan * (x) + step * (y))[1] * 2 + (copyPtr + nChan * (x + 1) + step * (y))[1] * 2 + (copyPtr + nChan * (x - 1) + step * (y + 1))[1] + (copyPtr + nChan * (x) + step * (y + 1))[1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1]) / 9.0));
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round((((copyPtr + nChan * (x - 1) + step * (y))[2] * 2 + (copyPtr + nChan * (x) + step * (y))[2] * 2 + (copyPtr + nChan * (x + 1) + step * (y))[2] * 2 + (copyPtr + nChan * (x - 1) + step * (y + 1))[2] + (copyPtr + nChan * (x) + step * (y + 1))[2] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2]) / 9.0));

                        y = height - 1;
                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[0] + (copyPtr + nChan * (x) + step * (y - 1))[0] + (copyPtr + nChan * (x + 1) + step * (y - 1))[0] + ((copyPtr + nChan * (x - 1) + step * (y))[0] + (copyPtr + nChan * (x) + step * (y))[0] + (copyPtr + nChan * (x + 1) + step * (y))[0]) * 2 / 9.0));
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[1] + (copyPtr + nChan * (x) + step * (y - 1))[1] + (copyPtr + nChan * (x + 1) + step * (y - 1))[1] + ((copyPtr + nChan * (x - 1) + step * (y))[1] + (copyPtr + nChan * (x) + step * (y))[1] + (copyPtr + nChan * (x + 1) + step * (y))[1]) * 2 / 9.0));
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[2] + (copyPtr + nChan * (x) + step * (y - 1))[2] + (copyPtr + nChan * (x + 1) + step * (y - 1))[2] + ((copyPtr + nChan * (x - 1) + step * (y))[2] + (copyPtr + nChan * (x) + step * (y))[2] + (copyPtr + nChan * (x + 1) + step * (y))[2]) * 2 / 9.0));


                    }



                    //Mean of Left and Right Border
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;
                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(((copyPtr + nChan * (x) + step * (y - 1))[0] * 2 + (copyPtr + nChan * (x + 1) + step * (y - 1))[0] + (copyPtr + nChan * (x) + step * (y))[0] * 2 + (copyPtr + nChan * (x + 1) + step * (y))[0] + (copyPtr + nChan * (x) + step * (y + 1))[0] * 2 + (copyPtr + nChan * (x + 1) + step * (y + 1))[0]) / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(((copyPtr + nChan * (x) + step * (y - 1))[1] * 2 + (copyPtr + nChan * (x + 1) + step * (y - 1))[1] + (copyPtr + nChan * (x) + step * (y))[1] * 2 + (copyPtr + nChan * (x + 1) + step * (y))[1] + (copyPtr + nChan * (x) + step * (y + 1))[1] * 2 + (copyPtr + nChan * (x + 1) + step * (y + 1))[1]) / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(((copyPtr + nChan * (x) + step * (y - 1))[2] * 2 + (copyPtr + nChan * (x + 1) + step * (y - 1))[2] + (copyPtr + nChan * (x) + step * (y))[2] * 2 + (copyPtr + nChan * (x + 1) + step * (y))[2] + (copyPtr + nChan * (x) + step * (y + 1))[2] * 2 + (copyPtr + nChan * (x + 1) + step * (y + 1))[2]) / 9.0);

                        x = width - 1;
                        (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[0] + (copyPtr + nChan * (x) + step * (y - 1))[0] * 2 + (copyPtr + nChan * (x - 1) + step * (y))[0] + (copyPtr + nChan * (x) + step * (y))[0] * 2 + (copyPtr + nChan * (x - 1) + step * (y + 1))[0] + (copyPtr + nChan * (x) + step * (y + 1))[0] * 2) / 9.0);
                        (dataPtr + nChan * x + step * y)[1] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[1] + (copyPtr + nChan * (x) + step * (y - 1))[1] * 2 + (copyPtr + nChan * (x - 1) + step * (y))[1] + (copyPtr + nChan * (x) + step * (y))[1] * 2 + (copyPtr + nChan * (x - 1) + step * (y + 1))[1] + (copyPtr + nChan * (x) + step * (y + 1))[1] * 2) / 9.0);
                        (dataPtr + nChan * x + step * y)[2] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[2] + (copyPtr + nChan * (x) + step * (y - 1))[2] * 2 + (copyPtr + nChan * (x - 1) + step * (y))[2] + (copyPtr + nChan * (x) + step * (y))[2] * 2 + (copyPtr + nChan * (x - 1) + step * (y + 1))[2] + (copyPtr + nChan * (x) + step * (y + 1))[2] * 2) / 9.0);
                    }

                    //faltam cantos

                    x = 0;
                    y = 0;

                    (dataPtr + nChan * x + step * y)[0] = (byte)Math.Round(((copyPtr + nChan * (x - 1) + step * (y - 1))[0] + (copyPtr + nChan * (x) + step * (y - 1))[0] + (copyPtr + nChan * (x + 1) + step * (y - 1))[0] + (copyPtr + nChan * (x - 1) + step * (y))[0] + (copyPtr + nChan * (x) + step * (y))[0] + (copyPtr + nChan * (x + 1) + step * (y))[0] + (copyPtr + nChan * (x - 1) + step * (y + 1))[0] + (copyPtr + nChan * (x) + step * (y + 1))[0] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0]) / 9.0);




                }
            }

        }


        public static void NonUniform(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float[,] matrix, float matrixWeight)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image
                byte blue, green, red, gray;

                MIplImage copy = imgCopy.MIplImage;
                byte* copyPtr = (byte*)copy.imageData.ToPointer();

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int valueB, valueG, valueR;

                int step = m.widthStep;

                if (nChan == 3) // image in RGB
                {
                    //Core
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            //Blue Color
                            valueB = (int)(Math.Round((((copyPtr + nChan * (x - 1) + step * (y - 1))[0] * matrix[0, 0] + (copyPtr + nChan * (x) + step * (y - 1))[0] * matrix[0, 1] + (copyPtr + nChan * (x + 1) + step * (y - 1))[0] * matrix[0, 2]) + ((copyPtr + nChan * (x - 1) + step * (y))[0] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[0] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[0] * matrix[1, 2]) + ((copyPtr + nChan * (x - 1) + step * (y + 1))[0] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[0] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0] * matrix[2, 2])) / matrixWeight));

                            if (valueB > 255)
                            {
                                valueB = 255;
                            }
                            else
                            {
                                if (valueB < 0)
                                    valueB = 0;
                            }

                            (dataPtr + nChan * x + step * y)[0] = (byte)valueB;

                            //Green Color
                            valueG = (int)(Math.Round((((copyPtr + nChan * (x - 1) + step * (y - 1))[1] * matrix[0, 0] + (copyPtr + nChan * (x) + step * (y - 1))[1] * matrix[0, 1] + (copyPtr + nChan * (x + 1) + step * (y - 1))[1] * matrix[0, 2]) + ((copyPtr + nChan * (x - 1) + step * (y))[1] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[1] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[1] * matrix[1, 2]) + ((copyPtr + nChan * (x - 1) + step * (y + 1))[1] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[1] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1] * matrix[2, 2])) / matrixWeight));

                            if (valueG > 255)
                            {
                                valueG = 255;
                            }
                            else
                            {
                                if (valueG < 0)
                                    valueG = 0;
                            }

                            (dataPtr + nChan * x + step * y)[1] = (byte)valueG;

                            //Red Color
                            valueR = (int)(Math.Round((((copyPtr + nChan * (x - 1) + step * (y - 1))[2] * matrix[0, 0] + (copyPtr + nChan * (x) + step * (y - 1))[2] * matrix[0, 1] + (copyPtr + nChan * (x + 1) + step * (y - 1))[2] * matrix[0, 2]) + ((copyPtr + nChan * (x - 1) + step * (y))[2] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[2] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[2] * matrix[1, 2]) + ((copyPtr + nChan * (x - 1) + step * (y + 1))[2] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[2] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2] * matrix[2, 2])) / matrixWeight));

                            if (valueR > 255)
                            {
                                valueR = 255;
                            }
                            else
                            {
                                if (valueR < 0)
                                    valueR = 0;
                            }

                            (dataPtr + nChan * x + step * y)[2] = (byte)valueR;
                        }
                    }

                    //Top and Bottom Border
                    for (x = 1; x < width - 1; x++)
                    {
                        //Top
                        y = 0;

                        //Blue Color
                        valueB = (int)(Math.Round(((copyPtr + nChan * (x - 1) + step * (y))[0] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[0] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[0] * matrix[1, 2]) * 2 + ((copyPtr + nChan * (x - 1) + step * (y + 1))[0] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[0] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0] * matrix[2, 2]) / matrixWeight));

                        if (valueB > 255)
                        {
                            valueB = 255;
                        }
                        else
                        {
                            if (valueB < 0)
                                valueB = 0;
                        }

                        (dataPtr + nChan * x + step * y)[0] = (byte)valueB;

                        //Green Color
                        valueG = (int)(Math.Round(((copyPtr + nChan * (x - 1) + step * (y))[1] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[1] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[1] * matrix[1, 2]) * 2 + ((copyPtr + nChan * (x - 1) + step * (y + 1))[1] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[1] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1] * matrix[2, 2]) / matrixWeight));

                        if (valueG > 255)
                        {
                            valueG = 255;
                        }
                        else
                        {
                            if (valueG < 0)
                                valueG = 0;
                        }

                        (dataPtr + nChan * x + step * y)[1] = (byte)valueG;

                        //Red Color
                        valueR = (int)(Math.Round(((copyPtr + nChan * (x - 1) + step * (y))[2] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[2] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[2] * matrix[1, 2]) * 2 + ((copyPtr + nChan * (x - 1) + step * (y + 1))[2] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[2] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2] * matrix[2, 2]) / matrixWeight));

                        if (valueR > 255)
                        {
                            valueR = 255;
                        }
                        else
                        {
                            if (valueR < 0)
                                valueR = 0;
                        }

                        (dataPtr + nChan * x + step * y)[2] = (byte)valueR;

                        //Bottom
                        y = height - 1;

                        //Blue Color
                        valueB = (int)(Math.Round(((copyPtr + nChan * (x - 1) + step * (y))[0] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[0] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[0] * matrix[1, 2]) * 2 + ((copyPtr + nChan * (x - 1) + step * (y + 1))[0] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[0] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0] * matrix[2, 2]) / matrixWeight));

                        if (valueB > 255)
                        {
                            valueB = 255;
                        }
                        else
                        {
                            if (valueB < 0)
                                valueB = 0;
                        }

                        (dataPtr + nChan * x + step * y)[0] = (byte)valueB;

                        //Green Color
                        valueG = (int)(Math.Round(((copyPtr + nChan * (x - 1) + step * (y))[1] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[1] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[1] * matrix[1, 2]) * 2 + ((copyPtr + nChan * (x - 1) + step * (y + 1))[1] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[1] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1] * matrix[2, 2]) / matrixWeight));

                        if (valueG > 255)
                        {
                            valueG = 255;
                        }
                        else
                        {
                            if (valueG < 0)
                                valueG = 0;
                        }

                        (dataPtr + nChan * x + step * y)[1] = (byte)valueG;

                        //Red Color
                        valueR = (int)(Math.Round(((copyPtr + nChan * (x - 1) + step * (y))[2] * matrix[1, 0] + (copyPtr + nChan * (x) + step * (y))[2] * matrix[1, 1] + (copyPtr + nChan * (x + 1) + step * (y))[2] * matrix[1, 2]) * 2 + ((copyPtr + nChan * (x - 1) + step * (y + 1))[2] * matrix[2, 0] + (copyPtr + nChan * (x) + step * (y + 1))[2] * matrix[2, 1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2] * matrix[2, 2]) / matrixWeight));

                        if (valueR > 255)
                        {
                            valueR = 255;
                        }
                        else
                        {
                            if (valueR < 0)
                                valueR = 0;
                        }

                        (dataPtr + nChan * x + step * y)[2] = (byte)valueR;
                    }



                    //Left and Right Border
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;

                        //Blue Color
                        //valueB = (int)(Math.Round())


                        x = width - 1;



                    }
                }
            }
        }

        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {


            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                MIplImage copy = imgCopy.MIplImage;
                byte* copyPtr = (byte*)copy.imageData.ToPointer();

                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int valor;

                int valorX, valorY;

                if (nChan == 3) // image in RGB
                {

                    //Sobel of Core
                    for (y = 1; y < height - 1; y++)
                    {
                        for (x = 1; x < width - 1; x++)
                        {
                            //Blue Color
                            valorX = ((copyPtr + nChan * (x - 1) + step * (y - 1))[0] + 2 * (copyPtr + nChan * (x - 1) + step * (y))[0] + (copyPtr + nChan * (x - 1) + step * (y + 1))[0]) - ((copyPtr + nChan * (x + 1) + step * (y - 1))[0] + 2 * (copyPtr + nChan * (x + 1) + step * (y))[0] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0]);
                            valorY = ((copyPtr + nChan * (x - 1) + step * (y + 1))[0] + 2 * (copyPtr + nChan * (x) + step * (y + 1))[0] + (copyPtr + nChan * (x + 1) + step * (y + 1))[0]) - ((copyPtr + nChan * (x - 1) + step * (y - 1))[0] + 2 * (copyPtr + nChan * (x) + step * (y - 1))[0] + (copyPtr + nChan * (x + 1) + step * (y - 1))[0]);

                            valor = Math.Abs(valorX) + Math.Abs(valorY);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = Math.Abs(valor);
                            }

                            (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                            //Green Color
                            valorX = ((copyPtr + nChan * (x - 1) + step * (y - 1))[1] + 2 * (copyPtr + nChan * (x - 1) + step * (y))[1] + (copyPtr + nChan * (x - 1) + step * (y + 1))[1]) - ((copyPtr + nChan * (x + 1) + step * (y - 1))[1] + 2 * (copyPtr + nChan * (x + 1) + step * (y))[1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1]);
                            valorY = ((copyPtr + nChan * (x - 1) + step * (y + 1))[1] + 2 * (copyPtr + nChan * (x) + step * (y + 1))[1] + (copyPtr + nChan * (x + 1) + step * (y + 1))[1]) - ((copyPtr + nChan * (x - 1) + step * (y - 1))[1] + 2 * (copyPtr + nChan * (x) + step * (y - 1))[1] + (copyPtr + nChan * (x + 1) + step * (y - 1))[1]);

                            valor = Math.Abs(valorX) + Math.Abs(valorY);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = Math.Abs(valor);
                            }

                            (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                            //Red Color
                            valorX = ((copyPtr + nChan * (x - 1) + step * (y - 1))[2] + 2 * (copyPtr + nChan * (x - 1) + step * (y))[2] + (copyPtr + nChan * (x - 1) + step * (y + 1))[2]) - ((copyPtr + nChan * (x + 1) + step * (y - 1))[2] + 2 * (copyPtr + nChan * (x + 1) + step * (y))[2] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2]);
                            valorY = ((copyPtr + nChan * (x - 1) + step * (y + 1))[2] + 2 * (copyPtr + nChan * (x) + step * (y + 1))[2] + (copyPtr + nChan * (x + 1) + step * (y + 1))[2]) - ((copyPtr + nChan * (x - 1) + step * (y - 1))[2] + 2 * (copyPtr + nChan * (x) + step * (y - 1))[2] + (copyPtr + nChan * (x + 1) + step * (y - 1))[2]);

                            valor = Math.Abs(valorX) + Math.Abs(valorY);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = Math.Abs(valor);
                            }

                            (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                        }
                    }


                    //Mean of Top and Bottom Border
                    for (x = 1; x < width - 1; x++)
                    {
                        y = 0;

                        valorX = 0;
                        valorY = 0;
                        

                        y = height - 1;


                    }



                    //Mean of Left and Right Border
                    for (y = 1; y < height - 1; y++)
                    {
                        x = 0;


                        x = width - 1;

                    }

                    //faltam cantos

                    x = 0;
                    y = 0;





                }
            }

        }


        public static void Diferentiation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                MIplImage copy = imgCopy.MIplImage;
                byte* copyPtr = (byte*)copy.imageData.ToPointer();

                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int valor;

                if (nChan == 3) // image in RGB
                {

                    //Core
                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {
                            //Blue Color
                            valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x + 1) + step * (y))[0]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = 0;
                            }

                            (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                            //Green Color
                            valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x + 1) + step * (y))[1]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = 0;
                            }

                            (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                            //Red Color
                            valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x + 1) + step * (y))[2]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = 0;
                            }

                            (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                        }
                    }


                    //Right Border
                    for (y = 0; y < height - 1; y++)
                    {
                        x = width - 1;

                        //Blue Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y))[0]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                        //Green Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y))[1]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                        //Red Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y))[2]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                    }



                    //Bottom border
                    for (x = 0; x < width - 1; x++)
                    {
                        y = height - 1;

                        //Blue Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x + 1) + step * (y))[0]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y))[0]);
                        
                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                        //Green Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x + 1) + step * (y))[1]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y))[1]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                        //Red Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x + 1) + step * (y))[2]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y))[2]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                    }

                    //Bottom Right Corner
                    x = width - 1;
                    y = height - 1;

                    //Blue Color
                    valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]);

                    if (valor > 255)
                    {
                        valor = 255;
                    }
                    else
                    {
                        if (valor < 0)
                            valor = 0;
                    }

                    (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                }
            }








        }


        public static void Roberts(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {

            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.imageData.ToPointer(); // Pointer to the image

                MIplImage copy = imgCopy.MIplImage;
                byte* copyPtr = (byte*)copy.imageData.ToPointer();

                byte blue, green, red, gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.nChannels; // number of channels - 3
                int padding = m.widthStep - m.nChannels * m.width; // alinhament bytes (padding)
                int x, y;

                int step = m.widthStep;
                int valor;

                if (nChan == 3) // image in RGB
                {

                    //Roberts Filter of Core
                    for (y = 0; y < height - 1; y++)
                    {
                        for (x = 0; x < width - 1; x++)
                        {
                            //Blue Color
                            valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x + 1) + step * (y + 1))[0]) + Math.Abs((copyPtr + nChan * (x + 1) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = 0;
                            }

                            (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                            //Green Color
                            valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x + 1) + step * (y + 1))[1]) + Math.Abs((copyPtr + nChan * (x + 1) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = 0;
                            }

                            (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                            //Red Color
                            valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x + 1) + step * (y + 1))[2]) + Math.Abs((copyPtr + nChan * (x + 1) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]);

                            if (valor > 255)
                            {
                                valor = 255;
                            }
                            else
                            {
                                if (valor < 0)
                                    valor = 0;
                            }

                            (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                        }
                    }


                    //Right Border
                    for (y = 0; y < height - 1; y++)
                    {
                        x = width - 1;

                        //Blue Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                        //Green Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                        //Red Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                    }



                    //Bottom border
                    for (x = 0; x < width - 1; x++)
                    {
                        y = height - 1;

                        //Blue Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x + 1) + step * (y))[0]) + Math.Abs((copyPtr + nChan * (x + 1) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y))[0]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                        //Green Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x + 1) + step * (y))[1]) + Math.Abs((copyPtr + nChan * (x + 1) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y))[1]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                        //Red Color
                        valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x + 1) + step * (y))[2]) + Math.Abs((copyPtr + nChan * (x + 1) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y))[2]);

                        if (valor > 255)
                        {
                            valor = 255;
                        }
                        else
                        {
                            if (valor < 0)
                                valor = 0;
                        }

                        (dataPtr + nChan * x + step * y)[2] = (byte)valor;
                    }

                    //Bottom Right Corner
                    x = width - 1;
                    y = height - 1;

                    //Blue Color
                    valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[0] - (copyPtr + nChan * (x) + step * (y + 1))[0]);

                    if (valor > 255)
                    {
                        valor = 255;
                    }
                    else
                    {
                        if (valor < 0)
                            valor = 0;
                    }

                    (dataPtr + nChan * x + step * y)[0] = (byte)valor;

                    //Green Color
                    valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[1] - (copyPtr + nChan * (x) + step * (y + 1))[1]);

                    if (valor > 255)
                    {
                        valor = 255;
                    }
                    else
                    {
                        if (valor < 0)
                            valor = 0;
                    }

                    (dataPtr + nChan * x + step * y)[1] = (byte)valor;

                    //Red Color
                    valor = Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]) + Math.Abs((copyPtr + nChan * (x) + step * (y))[2] - (copyPtr + nChan * (x) + step * (y + 1))[2]);

                    if (valor > 255)
                    {
                        valor = 255;
                    }
                    else
                    {
                        if (valor < 0)
                            valor = 0;
                    }

                    (dataPtr + nChan * x + step * y)[2] = (byte)valor;



                }
            }





        }


    }
}