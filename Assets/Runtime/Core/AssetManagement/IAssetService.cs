using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using System;

public interface IAssetService
{
    Task<T> LoadAsync<T>(string assetName, CancellationToken cancellationToken = default);
    Task<T> InstantiateAsync<T>(Transform container = null, string assetName = null, CancellationToken cancellationToken = default);
    Task<GameObject> InstantiateAsync(string assetName, Transform container = null, CancellationToken cancellationToken = default);
    void DisposeAsset(GameObject gameObject);
    event Action<string> LoadError;
}
