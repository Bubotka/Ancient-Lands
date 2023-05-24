using Codebase.Infrastructure.Services;
using UnityEngine;

namespace Codebase.Infrastructure.AssetManagement
{
    public interface IAssets:IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path,Vector3 at);
    }
}