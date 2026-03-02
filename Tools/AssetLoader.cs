using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace GoatCamMod.Tools;

//https://github.com/developer9998/GorillaHistoricalTeleporter/blob/main/GorillaHistoricalTeleporter/Tools/AssetLoader.cs (slightly modified)
/*
MIT License

Copyright (c) 2025 Dane "Dev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
public class AssetLoader
{
    private static          AssetBundle                loadedBundle;
    private static readonly Dictionary<string, Object> LoadedAssets = [];

    private static Task bundleLoadTask;

    public static async Task<T> LoadAsset<T>(string assetName) where T : Object
    {
        Object cached;

        if (LoadedAssets.TryGetValue(assetName, out cached) && cached is T)
            return (T)cached;

        if (loadedBundle == null)
        {
            if (bundleLoadTask == null)
                bundleLoadTask = LoadAssetBundle();

            await bundleLoadTask;
        }

        TaskCompletionSource<T> completionSource = new();

        AssetBundleRequest request = loadedBundle.LoadAssetAsync<T>(assetName);
        request.completed += _ =>
                             {
                                 T result = request.asset as T;
                                 completionSource.TrySetResult(result);
                             };

        T loadedAsset = await completionSource.Task;

        if (loadedAsset != null)
            LoadedAssets[assetName] = loadedAsset;

        return loadedAsset;
    }

    private static async Task LoadAssetBundle()
    {
        TaskCompletionSource<AssetBundle> completionSource = new();

        Stream stream = typeof(AssetLoader).Assembly.GetManifestResourceStream("GoatCamMod.Content.goatcam");

        if (stream == null)
            throw new FileNotFoundException("[GSD]: Embedded asset bundle not found.");

        AssetBundleCreateRequest request = AssetBundle.LoadFromStreamAsync(stream);
        request.completed += _ => { completionSource.TrySetResult(request.assetBundle); };

        loadedBundle = await completionSource.Task;
    }
}