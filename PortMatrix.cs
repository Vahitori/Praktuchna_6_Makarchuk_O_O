namespace StudentManagement;

public class PortMatrix
{
    private readonly Port[,] _matrix = new Port[16, 16];

    public PortMatrix()
    {
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                _matrix[i, j] = new Port(i * 16 + j, $"Device_{i}_{j}");
            }
        }
    }

    public Port GetPort(int row, int col)
    {
        if (row < 0 || row >= 16 || col < 0 || col >= 16)
            throw new IndexOutOfRangeException("Invalid matrix coordinates.");
        return _matrix[row, col];
    }

    public void OpenPort(int row, int col) => GetPort(row, col).Open();
    public void ClosePort(int row, int col) => GetPort(row, col).Close();

    public void WriteToPort(int row, int col, byte[] data) => GetPort(row, col).WriteData(data);
    public byte[] ReadFromPort(int row, int col) => GetPort(row, col).ReadData();

    public string ScanMatrix()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Matrix Scan Status:");
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                if (_matrix[i, j].IsOpen)
                {
                    sb.AppendLine($"[!] Port [{i},{j}] (ID: {_matrix[i,j].PortNumber}) is OPEN.");
                }
            }
        }
        return sb.ToString();
    }
    
    public Port[,] GetRawMatrix() => _matrix;
}
