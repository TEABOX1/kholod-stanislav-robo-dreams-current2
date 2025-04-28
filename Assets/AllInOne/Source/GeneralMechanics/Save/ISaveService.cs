
namespace AllInOne
{
    public interface ISaveService : IService
    {
        void SaveAll();
        void LoadAll();
        ref SaveData SaveData { get; }
    }
}