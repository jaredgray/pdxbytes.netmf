using pdxbytes.Structures;

namespace pdxbytes.DeviceInterfaces
{
    public interface IGraphicDevice : IDevice
    {
        /// <summary>
        /// initializes a memory block to write to on the display
        /// </summary>
        /// <param name="x">the horizontal location on the display</param>
        /// <param name="y">marks the vertical location on the display</param>
        /// <param name="width">sets the width of the block</param>
        /// <param name="height">sets the height of the block</param>
        void BeginDraw(short x, short y, short width, short height);

        /// <summary>
        /// pushes the visual data to the display
        /// </summary>
        /// <param name="buffer">data to go to the screen</param>
        void WriteBuffer(UInt24Collection buffer);

        int BufferSize { get; }
        int Stride { get; }
    }
}