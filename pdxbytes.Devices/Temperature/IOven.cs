namespace pdxbytes.Devices.Temperature
{
    public interface IOven
    {
        double Temperature { get; }

        void TurnElementOff();
        void TurnElementOn();
    }
}