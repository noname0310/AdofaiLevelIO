using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model;

namespace NoName.AdofaiLevelIO
{
    public class FloorContainer : IEnumerable<Floor>
    {
        public int Count => _floorList.Count;
        public string PathData
        {
            get
            {
                var stringBuilder = new StringBuilder(_floorList.Count);
                foreach (var item in _floorList)
                    stringBuilder.Append(item.Direction);
                return stringBuilder.ToString();
            }
        }
        public LinkedListNode<Floor> this[int index]
        {
            get
            {
                if (_floorList.Count <= index)
                    throw new IndexOutOfRangeException();

                if (index < _cachedfloorList.Count)
                    return _cachedfloorList[index];

                if (_cachedfloorList.Count == 0)
                    _cachedfloorList.Add(_floorList.First);

                var currentNode = _cachedfloorList[_cachedfloorList.Count - 1];

                for (var i = _cachedfloorList.Count - 1; i < index; i++)
                {
                    if (currentNode.Next == null)
                        break;
                    currentNode = currentNode.Next;
                    _cachedfloorList.Add(currentNode);
                }
                return currentNode;
            }
        }
        
        private readonly FloorCacheContainer _floorCache;
        private readonly LinkedList<Floor> _floorList;
        private readonly List<LinkedListNode<Floor>> _cachedfloorList;

        internal FloorContainer(string pathData, AdofaiLevel adofaiLevel)
        {
            _floorCache = new FloorCacheContainer();
            _floorList = new LinkedList<Floor>();
            _cachedfloorList = new List<LinkedListNode<Floor>>();

            var actions = new List<JToken>[pathData.Length];
            for (var i = 0; i < pathData.Length; i++)
                actions[i] = new List<JToken>();
            foreach (var action in LevelReader.GetActions(adofaiLevel.RawData))
                actions[action["floor"]?.ToObject<int>() ?? throw new Exception("json parse error : floor is not int")].Add(action);

            for (var i = 0; i < pathData.Length; i++)
            {
                var item = new Floor(i, pathData[i], actions[i], adofaiLevel, _floorCache);
                _floorList.AddLast(item);
                item.SelfPosition = _floorList.Last;
            }
        }

        public void ResetCache()
        {
            _floorCache.Clear();
            _cachedfloorList.Clear();
        }

        public IEnumerator<Floor> GetEnumerator() => _floorList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
