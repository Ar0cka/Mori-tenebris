using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Systems.DataLoader.Scripts
{
    public static class DataLoader
    {
        /// <summary>
        /// Метод для асинхронной загрузки конфигов из проекта. Path - путь к дикерктории где находится конфик
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> LoadDataAsync<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return default;
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);

            try
            {
                await handle.Task;
                return handle.Result;
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError(e.Message);
#endif
            }
            finally
            {
                Addressables.Release(handle);
            }

            return default;
        }
    }
}