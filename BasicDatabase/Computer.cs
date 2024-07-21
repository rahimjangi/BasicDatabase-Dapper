namespace BasicDatabase;

public class Computer
{
    public int ComputerId { get; set; }
    public string Motherboard { get; set; } = string.Empty;
    public int CPUcores { get; set; }
    public bool HasWiFi { get; set; }
    public bool HasLTE { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Price { get; set; }
    public string VideoCard { get; set; } = string.Empty;
}
