namespace StudentManagement;

public class Port : ICloneable
{
    public int PortNumber { get; }
    public byte[] DataBuffer { get; } = new byte[64];
    public bool IsOpen { get; private set; }
    public string DeviceName { get; }

    public Port(int portNumber, string deviceName)
    {
        PortNumber = portNumber;
        DeviceName = deviceName;
        IsOpen = false;
    }

    public void Open() => IsOpen = true;
    public void Close() => IsOpen = false;

    public void WriteData(byte[] data)
    {
        if (!IsOpen) throw new InvalidOperationException("Port is closed.");
        int length = Math.Min(data.Length, DataBuffer.Length);
        Array.Copy(data, DataBuffer, length);
    }

    public byte[] ReadData()
    {
        if (!IsOpen) throw new InvalidOperationException("Port is closed.");
        return (byte[])DataBuffer.Clone();
    }

    public object Clone()
    {
        var clone = (Port)this.MemberwiseClone();
        Array.Copy(this.DataBuffer, clone.DataBuffer, this.DataBuffer.Length);
        return clone;
    }

    public override string ToString() => $"Port {PortNumber}: {DeviceName} ({(IsOpen ? "Open" : "Closed")})";
}
