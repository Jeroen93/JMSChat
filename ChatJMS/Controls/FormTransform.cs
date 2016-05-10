//====================================================
//| Downloaded From                                  |
//| Visual C# Kicks - http://www.vcskicks.com/       |
//| License - http://www.vcskicks.com/license.php    |
//====================================================

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ChatJMS.Controls
{
    internal static class FormTransform
    {
        public static void TransformSize(Form frm, int newWidth, int newHeight)
        {
            TransformSize(frm, new Size(newWidth, newHeight));
        }

        public static void TransformSize(Form frm, Size newSize)
        {
            ParameterizedThreadStart threadStart = RunTransformation;
            var transformThread = new Thread(threadStart);

            transformThread.Start(new object[] { frm, newSize });
        }

        private delegate void RunTransformationDelegate(object paramaters);
        private static void RunTransformation(object parameters)
        {
            var frm = (Form)((object[])parameters)[0];
            if (frm.InvokeRequired)
            {
                var del = new RunTransformationDelegate(RunTransformation);
                frm.Invoke(del, parameters);
            }
            else
            {
                //Animation variables
                const double fps = 300.0;
                var interval = (long)(Stopwatch.Frequency / fps);
                long ticks1 = 0;

                //Dimension transform variables
                var size = (Size)((object[])parameters)[1];

                const int step = 10;

                var xDirection = frm.Width < size.Width ? 1 : -1;
                var yDirection = frm.Height < size.Height ? 1 : -1;

                var xStep = step * xDirection;
                var yStep = step * yDirection;

                var widthOff = IsOff(frm.Width, size.Width, xStep);
                var heightOff = IsOff(frm.Height, size.Height, yStep);


                while (widthOff || heightOff)
                {
                    //Get current timestamp
                    var ticks2 = Stopwatch.GetTimestamp();

                    if (ticks2 >= ticks1 + interval) //only run logic if enough time has passed "between frames"
                    {
                        //Adjust the Form dimensions
                        if (widthOff)
                            frm.Width += xStep;

                        if (heightOff)
                            frm.Height += yStep;

                        widthOff = IsOff(frm.Width, size.Width, xStep);
                        heightOff = IsOff(frm.Height, size.Height, yStep);

                        //Allows the Form to refresh
                        Application.DoEvents();

                        //Save current timestamp
                        ticks1 = Stopwatch.GetTimestamp();
                    }

                    Thread.Sleep(1);
                }

            }
        }

        private static bool IsOff(int current, int target, int step)
        {
            //Do avoid uneven jumps, do not change the width if it is
            //within the step amount
            if (Math.Abs(current - target) < Math.Abs(step)) return false;

            return (step > 0 && current < target) || //increasing direction - keep going if still too small
                   (step < 0 && current > target); //decreasing direction - keep going if still too large
        }
    }
}
