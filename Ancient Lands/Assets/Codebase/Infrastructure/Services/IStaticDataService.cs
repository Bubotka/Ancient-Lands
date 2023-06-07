using Codebase.StaticData;

namespace Codebase.Infrastructure.Services
{
    public interface IStaticDataService:IService
    {
        void LoadMonsters();
        MonsterStaticData ForMonster(MonsterTypeID typeID);
    }
} 