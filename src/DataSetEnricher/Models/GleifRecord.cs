namespace CardanoAssignment.Models;

public class GleifRecord
{
    public Data[] Data { get; set; }
}

public class Data
{
    public Attributes Attributes { get; set; }
}

public class Attributes
{
    public Entity Entity { get; set; }
    public string[] Bic { get; set; }
}

public class Entity
{
    public LegalName LegalName { get; set; }
    public LegalAddress LegalAddress { get; set; }

}

public class LegalName
{
    public string Name { get; set; }
}

public class LegalAddress
{
    public string Country { get; set; }
}