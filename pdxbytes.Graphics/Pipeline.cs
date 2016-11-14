using System.Collections;
using pdxbytes.DeviceInterfaces;
using pdxbytes.Structures;

namespace pdxbytes.Graphics
{
    public class Pipeline
    {
        public Pipeline(IGraphicDevice display)
        {
            this.Display = display;
            this.DisplayBuffers = new ArrayList();
        }

        public IGraphicDevice Display { get; set; }

        private ArrayList DisplayBuffers { get; set; }
        public void AddDisplay(DisplayBuffer buffer)
        {
            this.DisplayBuffers.Add(buffer);
        }

        public void Flush()
        {
            // TODO: sort the display buffers in zindex order
            foreach (DisplayBuffer buffer in this.DisplayBuffers)
            {
                UInt24Collection bytes = null;
                //var vec2 = buffer.GetCurrentPosition();
                if(!buffer.ApplyPositionUpdates)
                    this.Display.BeginDraw(buffer.X, buffer.Y, buffer.Width, buffer.Height);

                // make the buffer size evenly divisible by the stride
                var buffersize = ((int)(this.Display.BufferSize / buffer.Width));
                while (null != (bytes = buffer.Read(buffersize)))
                {
                    if(buffer.ApplyPositionUpdates)
                    {
                        var vec2 = buffer.GetCurrentPosition();
                        this.Display.BeginDraw(vec2.X, vec2.Y, buffer.Width, buffer.Height);
                    }
                    this.Display.WriteBuffer(bytes);
                }
                buffer.Cleanup();
            }

            this.DisplayBuffers.Clear();
        }
    }
}
