namespace CardanoAssignment.Models;

public class LeiRecord
{
    public Links Links { get; set; }
    public Data[] Data { get; set; }
}

public class Links
{
    public string First { get; set; }
    public string Last { get; set; }
}

public class Data
{
    public string Type { get; set; }
    public string Id { get; set; }
    public Attributes Attributes { get; set; }
    public Relationships Relationships { get; set; }
    public Links1 Links { get; set; }
}

public class Attributes
{
    public string Lei { get; set; }
    public Entity Entity { get; set; }
    public Registration Registration { get; set; }
    public string[] Bic { get; set; }
}

public class Entity
{
    public LegalName LegalName { get; set; }
    public object[] OtherNames { get; set; }
    public object[] TransliteratedOtherNames { get; set; }
    public LegalAddress LegalAddress { get; set; }
    public HeadquartersAddress HeadquartersAddress { get; set; }
    public RegisteredAt RegisteredAt { get; set; }
    public string RegisteredAs { get; set; }
    public string Jurisdiction { get; set; }
    public string Category { get; set; }
    public LegalForm LegalForm { get; set; }
    public AssociatedEntity AssociatedEntity { get; set; }
    public string Status { get; set; }
    public Expiration Expiration { get; set; }
    public SuccessorEntity SuccessorEntity { get; set; }
    public object[] SuccessorEntities { get; set; }
    public string CreationDate { get; set; }
    public object SubCategory { get; set; }
    public object[] OtherAddresses { get; set; }
    public object[] EventGroups { get; set; }
}

public class LegalName
{
    public string Name { get; set; }
    public string Language { get; set; }
}

public class LegalAddress
{
    public string Language { get; set; }
    public string[] AddressLines { get; set; }
    public object AddressNumber { get; set; }
    public object AddressNumberWithinBuilding { get; set; }
    public object MailRouting { get; set; }
    public string City { get; set; }
    public object Region { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
}

public class HeadquartersAddress
{
    public string Language { get; set; }
    public string[] AddressLines { get; set; }
    public object AddressNumber { get; set; }
    public object AddressNumberWithinBuilding { get; set; }
    public object MailRouting { get; set; }
    public string City { get; set; }
    public object Region { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
}

public class RegisteredAt
{
    public string Id { get; set; }
    public object Other { get; set; }
}

public class LegalForm
{
    public string Id { get; set; }
    public object Other { get; set; }
}

public class AssociatedEntity
{
    public object Lei { get; set; }
    public object Name { get; set; }
}

public class Expiration
{
    public object Date { get; set; }
    public object Reason { get; set; }
}

public class SuccessorEntity
{
    public object Lei { get; set; }
    public object Name { get; set; }
}

public class Registration
{
    public string InitialRegistrationDate { get; set; }
    public string LastUpdateDate { get; set; }
    public string Status { get; set; }
    public string NextRenewalDate { get; set; }
    public string ManagingLou { get; set; }
    public string CorroborationLevel { get; set; }
    public ValidatedAt ValidatedAt { get; set; }
    public string ValidatedAs { get; set; }
    public object[] OtherValidationAuthorities { get; set; }
}

public class ValidatedAt
{
    public string Id { get; set; }
    public object Other { get; set; }
}

public class Relationships
{
    public ManagingLou ManagingLou { get; set; }
    public LeiIssuer LeiIssuer { get; set; }
    public FieldModifications FieldModifications { get; set; }
    public DirectParent DirectParent { get; set; }
    public UltimateParent UltimateParent { get; set; }
}

public class ManagingLou
{
    public Links2 Links { get; set; }
}

public class Links2
{
    public string Related { get; set; }
}

public class LeiIssuer
{
    public Links3 Links { get; set; }
}

public class Links3
{
    public string Related { get; set; }
}

public class FieldModifications
{
    public Links4 Links { get; set; }
}

public class Links4
{
    public string Related { get; set; }
}

public class DirectParent
{
    public Links5 Links { get; set; }
}

public class Links5
{
    public string ReportingException { get; set; }
}

public class UltimateParent
{
    public Links6 Links { get; set; }
}

public class Links6
{
    public string ReportingException { get; set; }
}

public class Links1
{
    public string Self { get; set; }
}

