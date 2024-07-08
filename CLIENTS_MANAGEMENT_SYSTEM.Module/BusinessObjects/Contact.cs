
using CLIENTS_MANAGEMENT_SYSTEM.Module.BusinessObjects;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

[DefaultClassOptions]
[NavigationItem("Clients")]
public class Contact : BaseObject
{
    public Contact(Session session) : base(session) { }

    string name;
    string surname;
    string email;

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Name
    {
        get => name;
        set => SetPropertyValue(nameof(Name), ref name, value);
    }

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Surname
    {
        get => surname;
        set => SetPropertyValue(nameof(Surname), ref surname, value);
    }

    [RuleRequiredField(DefaultContexts.Save)]
    [RuleRegularExpression(DefaultContexts.Save, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", CustomMessageTemplate = "Invalid email format.")]
    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Email
    {
        get => email;
        set => SetPropertyValue(nameof(Email), ref email, value);
    
    }

        [Association("Client-Contacts")]
    public XPCollection<Client> Clients => GetCollection<Client>(nameof(Clients));
}