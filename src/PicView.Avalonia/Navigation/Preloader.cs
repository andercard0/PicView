﻿using System.Collections.Concurrent;
using System.Diagnostics;
using Avalonia.Media.Imaging;
using PicView.Avalonia.ImageHandling;
using PicView.Core.ImageDecoding;

namespace PicView.Avalonia.Navigation;

public sealed class PreLoader : IDisposable
{
    private readonly Lock _lock = new();
    public static bool IsRunning{ get; private set; }
    
    private readonly ConcurrentDictionary<int, PreLoadValue> _preLoadList = new();
    private const int PositiveIterations = 6;
    private const int NegativeIterations = 4;
    public const int MaxCount = PositiveIterations + NegativeIterations + 2;
    
    public class PreLoadValue(ImageModel? imageModel)
    {
        public ImageModel? ImageModel { get; set; } = imageModel;

        public bool IsLoading = true;
    }

#if DEBUG

    // ReSharper disable once ConvertToConstant.Local
    private static readonly bool ShowAddRemove = true;

#endif
    
    public async Task<bool> AddAsync(int index, List<string> list, ImageModel? imageModel = null)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(AddAsync)} list null \n{index}");
#endif
            return false;
        }
        if (index < 0 || index >= list.Count)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(AddAsync)} invalid index: \n{index}");
#endif
            return false;
        }

        var preLoadValue = new PreLoadValue(imageModel);
        try
        {
            var add = _preLoadList.TryAdd(index, preLoadValue);
            if (add)
            {
                if (imageModel is null)
                {
                    preLoadValue.IsLoading = true;
                    var fileInfo = new FileInfo(list[index]);
                    imageModel = await GetImageModel.GetImageModelAsync(fileInfo).ConfigureAwait(false);
                    preLoadValue.ImageModel = imageModel;
                }
                else
                {
                    preLoadValue.ImageModel = imageModel;
                    if (imageModel.Image is null)
                    {
                        preLoadValue.IsLoading = true;

                        preLoadValue.ImageModel = await GetImageModel.GetImageModelAsync(imageModel.FileInfo).ConfigureAwait(false);
                    }
                }
                
                if (imageModel.EXIFOrientation is null)
                {
                    preLoadValue.ImageModel.EXIFOrientation = EXIFHelper.GetImageOrientation(imageModel.FileInfo);
                }

#if DEBUG
                if (ShowAddRemove)
                    Trace.WriteLine($"{imageModel?.FileInfo?.Name} added at {index}");
#endif
                return true;
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(AddAsync)} exception: \n{ex}");
#endif
            return false;
        }
        finally
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (preLoadValue is not null)
            {
                preLoadValue.IsLoading = false;
            }
        }
        return false;
    }

    public async Task<bool> RefreshFileInfo(int index, List<string> list)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(RefreshFileInfo)} list null \n{index}");
#endif
            return false;
        }
        if (index < 0 || index >= list.Count)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(RefreshFileInfo)} invalid index: \n{index}");
#endif
            return false;
        }

        var removed = _preLoadList.TryRemove(index, out var preLoadValue);
        if (preLoadValue is null)
        {
            return removed;
        }

        if (preLoadValue.ImageModel != null)
        {
            preLoadValue.ImageModel.FileInfo = null;
        }

        await AddAsync(index, list, preLoadValue.ImageModel).ConfigureAwait(false);
        return removed;
    }
    
    public void RefreshAllFileInfo(List<string> list)
    {
        try
        {
            foreach (var item in _preLoadList)
            {
                if (item.Value is null) continue;
                var fileInfo = new FileInfo(list[item.Key]);
                if (item.Value.ImageModel != null)
                {
                    item.Value.ImageModel.FileInfo = fileInfo;
                }
            }
        }
        catch (Exception e)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(RefreshAllFileInfo)} \n{e.Message}");
#endif
        }
    }

    /// <summary>
    /// Removes all keys from the cache.
    /// </summary>
    public void Clear()
    {
        foreach (var item in _preLoadList.Values)
        {
            if (item?.ImageModel?.Image is Bitmap img)
            {
                img.Dispose();
            }
        }
        _preLoadList.Clear();
    }

    public PreLoadValue? Get(int key, List<string> list)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(Get)} list null \n{key}");
#endif
            return null;
        }
        if (key < 0 || key >= list.Count)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(Get)} invalid key: \n{key}");
#endif
            return null;
        }

        return !Contains(key, list) ? null : _preLoadList[key];
    }
    
    public async Task<PreLoadValue?> GetAsync(int key, List<string> list)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(GetAsync)} list null \n{key}");
#endif
            return null;
        }
        if (key < 0 || key >= list.Count)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(GetAsync)} invalid key: \n{key}");
#endif
            return null;
        }

        if (Contains(key, list))
        {
            return _preLoadList[key];
        }

        await AddAsync(key, list);
        return Get(key, list);
    }

    public bool Contains(int key, List<string> list)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(Get)} list null \n{key}");
#endif
            return false;
        }
        if (key < 0 || key >= list.Count)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(Contains)} invalid key: \n{key}");
#endif
            return false;
        }

        return _preLoadList.ContainsKey(key);
    }

    public bool Remove(int key, List<string> list)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(Get)} list null \n{key}");
#endif
            return false;
        }

        if (key < 0 || key >= list.Count)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(Remove)} invalid key: \n{key}");
#endif
            return false;
        }

        if (!Contains(key, list))
        {
            return false;
        }

        try
        {
            var item = _preLoadList[key];
            if (item.ImageModel.Image is Bitmap img)
            {
                img.Dispose();
            }
            item.ImageModel.Image = null;
            item.ImageModel.FileInfo = null;
            var remove = _preLoadList.TryRemove(key, out _);
#if DEBUG
            if (remove && ShowAddRemove)
                Trace.WriteLine($"{list[key]} removed at {list.IndexOf(list[key])}");
#endif
            return remove;
        }
        catch (Exception e)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(Remove)} exception:\n{e.Message}");
#endif
            return false;
        }
    }

    public async Task PreLoadAsync(int currentIndex, int count, bool reverse, List<string> list)
    {
        if (list == null)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoader)}.{nameof(PreLoadAsync)} list null \n{currentIndex}");
#endif
            return;
        }
        
        lock (_lock)
        {
            if (IsRunning) 
                return;
            IsRunning = true;
        }

        int nextStartingIndex, prevStartingIndex;
        if (reverse)
        {
            nextStartingIndex = (currentIndex - 1 + count) % count;
            prevStartingIndex = currentIndex + 1;
        }
        else
        {
            nextStartingIndex = (currentIndex + 1) % count;
            prevStartingIndex = currentIndex - 1;
        }
        var array = new int[MaxCount];

#if DEBUG
        if (ShowAddRemove)
            Trace.WriteLine($"\nPreLoading started at {currentIndex}\n");
#endif

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount - 3 < 1 ? 1 : Environment.ProcessorCount - 3
        };

        try
        {
            if (reverse)
            {
                await NegativeLoop(options);
                await PositiveLoop(options);
            }
            else
            {
                await PositiveLoop(options);
                await NegativeLoop(options);
            }
        }
        catch (Exception exception)
        {
#if DEBUG
            Trace.WriteLine($"{nameof(PreLoadAsync)} exception:\n{exception.Message}");
#endif
        }
        finally
        {
            lock (_lock)
            {
                IsRunning = false;
            }
        }

        RemoveLoop();

        return;

        async Task PositiveLoop(ParallelOptions parallelOptions)
        {
            await Parallel.ForAsync(0, PositiveIterations, parallelOptions, async (i, _) =>
            {
                if (list.Count == 0 || count != list.Count)
                {
                    Clear();
                    return;
                }
                var index = (nextStartingIndex + i) % list.Count;
                var isAdded = await AddAsync(index, list);
                if (isAdded)
                {
                    array[i] = index;
                }
            });
        }

        async Task NegativeLoop(ParallelOptions parallelOptions)
        {
            await Parallel.ForAsync(0, NegativeIterations, parallelOptions, async (i, _) =>
            {
                if (list.Count == 0 || count != list.Count)
                {
                    Clear();
                    return;
                }
                var index = (prevStartingIndex - i + list.Count) % list.Count;
                var isAdded = await AddAsync(index, list);
                if (isAdded)
                {
                    array[i] = index;
                }
            });
        }

        void RemoveLoop()
        {
            // Iterate through the _preLoadList and remove items outside the preload range
            
            if (list.Count <= MaxCount + NegativeIterations || _preLoadList.Count <= MaxCount)
            {
                return;
            }
            var deleteCount = _preLoadList.Count - MaxCount < MaxCount ? MaxCount : _preLoadList.Count - MaxCount;
            for (var i = 0; i < deleteCount; i++)
            {
                var removeIndex = reverse ? _preLoadList.Keys.Max() : _preLoadList.Keys.Min();
                if (i >= array.Length)
                {
                    return;
                }

                if (array[i] == removeIndex || removeIndex == currentIndex)
                {
                    continue;
                }

                if (removeIndex > currentIndex + 2 || removeIndex < currentIndex - 2)
                {
                    Remove(removeIndex, list);
                }
            }

            if (deleteCount > 1)
            {
                // Collect unmanaged memory, prevent memory leak
                GC.Collect(0, GCCollectionMode.Optimized, false);
            }
        }
    }

    #region IDisposable
    
    private bool _disposed;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, false);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            Clear();
        }
        _disposed = true;
    }
    
    ~PreLoader()
    {
        Dispose(false);
    }

    #endregion
}