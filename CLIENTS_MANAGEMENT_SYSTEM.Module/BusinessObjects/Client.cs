using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTS_MANAGEMENT_SYSTEM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Clients")]
    public class Client : BaseObject
    {
        public Client(Session session) : base(session) { }

        string name;
        string clientCode;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Persistent("ClientCode")]
        [Size(6)]
        public string ClientCode
        {
            get => clientCode;
            protected set => SetPropertyValue(nameof(ClientCode), ref clientCode, value);
        }

        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(ClientCode))
            {
                ClientCode = GenerateClientCode();
            }
            base.OnSaving();
        }

        private string GenerateClientCode()
        {
            var alphaPart = new string(Name.ToUpper().Take(3).ToArray());
            if (alphaPart.Length < 3)
            {
                alphaPart = alphaPart.PadRight(3, 'A');
            }

            var clientCodes = Session.Query<Client>()
                                     .Where(c => c.ClientCode.StartsWith(alphaPart))
                                     .Select(c => c.ClientCode)
                                     .ToList();

            int maxNumber = 0;
            if (clientCodes.Any())
            {
                maxNumber = clientCodes
                            .Select(code => int.Parse(code.Substring(3)))
                            .Max();
            }

            var numericPart = (maxNumber + 1).ToString("D3");
            return alphaPart + numericPart;
        }

        [PersistentAlias("Contacts.Count()")]
        public int LinkedContactsCount
        {
            get
            {
                object tempObject = EvaluateAlias(nameof(LinkedContactsCount));
                return tempObject != null ? (int)tempObject : 0;
            }
        }

        [Association("Client-Contacts")]
        public XPCollection<Contact> Contacts => GetCollection<Contact>(nameof(Contacts));
    }

}
