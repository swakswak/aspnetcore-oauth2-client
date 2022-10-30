namespace OAuth2Client.Security.Cryptography;

public class EncryptionManagerHolder : IEncryptionManagerHolder
{
    private IDictionary<EncryptionManagerType, IEncryptionManager> Dictionary { get; }

    private EncryptionManagerHolder(IDictionary<EncryptionManagerType, IEncryptionManager> encryptionManagers) 
        => Dictionary = new Dictionary<EncryptionManagerType, IEncryptionManager>();

    public IEncryptionManager Get(EncryptionManagerType name) 
        => Dictionary[name];

    public static EncryptionManagerHolderBuilder Builder()
    {
        return new EncryptionManagerHolderBuilder(new Dictionary<EncryptionManagerType, IEncryptionManager>());
    }

    public class EncryptionManagerHolderBuilder
    {
        public EncryptionManagerHolderBuilder(Dictionary<EncryptionManagerType,IEncryptionManager> dictionary)
        {
            Dict = dictionary;
        }

        private IDictionary<EncryptionManagerType, IEncryptionManager> Dict { get; }

        public EncryptionManagerHolderBuilder EncryptionManager(EncryptionManagerType type, IEncryptionManager manager)
        {
            Dict[type] = manager;
            return this;
        }

        public EncryptionManagerHolder Build()
        {
            return new EncryptionManagerHolder(Dict);
        }
    }
}