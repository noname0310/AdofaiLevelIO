using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using NoName.AdofaiLevelIO.Model.Actions;
using EventType = NoName.AdofaiLevelIO.Model.Actions.EventType;

namespace NoName.AdofaiLevelIO.Model
{
    public class Floor : JObjectMaterializer
    {
        public int Index { get; }
        public char Direction
        {
            get
            {
                if (_floorCacheContainer.TryGetValue(Index, out var floorCache) && floorCache.Direction != null)
                    return floorCache.Direction.Value;
                var direction = LevelReader.GetPathData(JObject)[Index];
				_floorCacheContainer.Caching(Index, new FloorCache() { Direction = direction });
				return direction;
            }
            set
            {
                if (Index == 0)
                    throw new IndexOutOfRangeException("StartFloor cannot be modified");

                var data = LevelReader.GetPathData(JObject).ToCharArray();
                data[Index] = value;
				LevelReader.SetPathData(JObject, new string(data));
                _floorCacheContainer.Caching(Index, new FloorCache() { Direction = value });
                foreach (var item in _floorCacheContainer)
                {
                    item.Value.EntryAngle = null;
                    item.Value.ExitAngle = null;
                }
            }
        }

        public double EntryAngle
        {
            get
            {
                if (Index == 0)
                    return 4.71238899230957;

				if (_floorCacheContainer.TryGetValue(Index, out var floorCache) && floorCache.EntryAngle != null)
                    return floorCache.EntryAngle.Value;
                
                var result = _adofaiLevel.Floors[Index - 1].ExitAngle;
                result = (result + 3.1415927410125732) % 6.2831854820251465;

                _floorCacheContainer.Caching(Index, new FloorCache() { EntryAngle = result });
                return result;
            }
        }
        public double ExitAngle
        {
            get
            {
                if (_floorCacheContainer.TryGetValue(Index, out var floorCache) && floorCache.ExitAngle != null)
                    return floorCache.ExitAngle.Value;

                var result = 0.0;
                var c = Direction;

                if (c <= 'q')
                {
                    if (c != '!')
                    {
                        switch (c)
                        {
                            case '5':
                                result = IncrementAngle(EntryAngle, 1.8849555253982544);
                                break;
                            case '6':
                                result = IncrementAngle(EntryAngle, -1.8849555253982544);
                                break;
                            case '7':
                                result = IncrementAngle(EntryAngle, 2.243994951248169);
                                break;
                            case '8':
                                result = IncrementAngle(EntryAngle, -2.243994951248169);
                                break;
                            case '9':
                                result = IncrementAngle(EntryAngle, 3.665191411972046);
                                break;
                            case 'A':
                                result = 1.832595705986023;
                                break;
                            case 'B':
                                result = 2.6179938316345215;
                                break;
                            case 'C':
                                result = 2.356194496154785;
                                break;
                            case 'D':
                                result = 3.1415927410125732;
                                break;
                            case 'E':
                                result = 0.7853981852531433;
                                break;
                            case 'F':
                                result = 3.665191411972046;
                                break;
                            case 'G':
                                result = 5.759586334228516;
                                break;
                            case 'H':
                                result = 5.235987663269043;
                                break;
                            case 'J':
                                result = 1.0471975803375244;
                                break;
                            case 'L':
                                result = 4.71238899230957;
                                break;
                            case 'M':
                                result = 2.094395160675049;
                                break;
                            case 'N':
                                result = 4.188790321350098;
                                break;
                            case 'Q':
                                result = 5.4977874755859375;
                                break;
                            case 'R':
                                result = 1.5707963705062866;
                                break;
                            case 'T':
                                result = 0.5235987901687622;
                                break;
                            case 'U':
                                result = 0.0;
                                break;
                            case 'V':
                                result = 3.4033920764923096;
                                break;
                            case 'W':
                                result = 4.974188327789307;
                                break;
                            case 'Y':
                                result = 2.879793167114258;
                                break;
                            case 'Z':
                                result = 3.9269909858703613;
                                break;
                            case '[':
                                break;
                            case 'h':
                                result = IncrementAngle(EntryAngle, 2.094395160675049);
                                break;
                            case 'j':
                                result = IncrementAngle(EntryAngle, -2.094395160675049);
                                break;
                            case 'o':
                                result = 0.2617993950843811;
                                break;
                            case 'p':
                                result = 1.3089969158172607;
                                break;
                            case 'q':
                                result = 6.021385669708252;
                                break;
                        }
                    }
                    else //midspin
                        result = EntryAngle;
                }
                else if (c != 't')
                {
                    if (c != 'x')
                    {
                        if (c == 'y')
                            result = IncrementAngle(EntryAngle, (-1.0471976f));
                    }
                    else
                        result = 4.450589656829834;
                }
                else
                    result = IncrementAngle(EntryAngle, (-1.0471976f));

                _floorCacheContainer.Caching(Index, new FloorCache() { ExitAngle = result });
                return result;
            }
        }
        public double RelativeAngle
        {
            get
            {
                double relativeAngle;
                if (EntryAngle < ExitAngle)
                    relativeAngle = ExitAngle - EntryAngle;
                else
                    relativeAngle = Math.PI * 2.0 - EntryAngle + ExitAngle;

                if (!IsClockWise)
                    relativeAngle = Math.PI * 2.0 - relativeAngle;

                var degree = relativeAngle * 360.0 / (Math.PI * 2.0);

                if (-2.0 < degree && degree < 2.0)
                    relativeAngle = Math.PI * 2.0;
                return relativeAngle;
            }
        }
        public float Bpm
        {
            get
            {
                float result;
                var mulitiplyValue = 1f;
				
                for (var i = Index; 0 < i; i--)
                {
                    if (_floorCacheContainer.TryGetValue(i, out var floorCache) && floorCache.Bpm != null)
                    {
                        result = floorCache.Bpm.Value * mulitiplyValue;
						_floorCacheContainer.Caching(Index, new FloorCache() { Bpm = result });
						return result;
                    }

                    var setSpeedAction = (
                        from action 
                        in _adofaiLevel.Floors[i].Actions 
                        where action.EventType == EventType.SetSpeed 
                        select action as SetSpeed
                        ).FirstOrDefault();

					if (setSpeedAction == null)
						continue;

                    var value = setSpeedAction.Value;
                    switch (setSpeedAction.SpeedType)
                    {
                        case SpeedType.Bpm:
                            result = value * mulitiplyValue;
							_floorCacheContainer.Caching(Index, new FloorCache() { Bpm = result });
							return result;
                        case SpeedType.Multiplier:
                            mulitiplyValue *= value;
                            break;
                        case SpeedType.NotAvailable:
                            break;
                        default:
                            goto case SpeedType.NotAvailable;
                    }
                }

                result = _adofaiLevel.LevelInfo.Bpm * mulitiplyValue;
				_floorCacheContainer.Caching(Index, new FloorCache() { Bpm = result });
				return result;
            }
            set
			{
                if (Index == 0)
                {
                    _adofaiLevel.LevelInfo.Bpm = value;
					return;
                }

                if (Mathf.Approximately(_adofaiLevel.Floors[Index - 1].Bpm, value))
                {
					Actions.Remove(EventType.SetSpeed);
                    return;
                }

                Actions.Add(new Data.SetSpeed(SpeedType.Bpm, value));
				_floorCacheContainer.Caching(Index, new FloorCache() { Bpm = value });
			}
        }

        public bool IsMidSpin => Direction == '!';

        public bool IsClockWise
        {
            get
            {
                bool result;
                var twirlCount = 0;

                for (var i = Index; 0 < i; i--)
                {
                    if (_floorCacheContainer.TryGetValue(i, out var floorCache) && floorCache.IsClockWise != null)
                    {
                        if (floorCache.IsClockWise.Value)
                            result = twirlCount % 2 == 0;
                        else
                            result = twirlCount % 2 != 0;
                        _floorCacheContainer.Caching(Index, new FloorCache() { IsClockWise = result });
                        return result;
                    }

                    if (_adofaiLevel.Floors[i].Actions.Any(action => action.EventType == EventType.Twirl))
                        twirlCount += 1;
                }
                
                result = twirlCount % 2 == 0;
                _floorCacheContainer.Caching(Index, new FloorCache() { IsClockWise = result });
                return result;
            }
        }

        public ActionContainer Actions { get; }

        private readonly AdofaiLevel _adofaiLevel;
        private readonly FloorCacheContainer _floorCacheContainer;

		internal Floor(JObject jObject, int floorIndex, AdofaiLevel adofaiLevel, FloorCacheContainer floorCacheContainer) : base(jObject)
        {
            Actions = new ActionContainer(jObject, floorIndex, floorCacheContainer);
            Index = floorIndex;
            _adofaiLevel = adofaiLevel;
            _floorCacheContainer = floorCacheContainer;
        }

        public static float GetAngleFromFloorCharDirectionWithCheck(char direction, out bool exists)
        {
            const float result = 0f;
            exists = true;
            switch (direction)
            {
                case 'A':
                    return 345f;
                case 'B':
                    return 300f;
                case 'C':
                    return 315f;
                case 'D':
                    return 270f;
                case 'E':
                    return 45f;
                case 'F':
                    return 240f;
                case 'G':
                    return 120f;
                case 'H':
                    return 150f;
                case 'I':
                case 'K':
                case 'O':
                case 'P':
                case 'S':
                case 'X':
                    break;
                case 'J':
                    return 30f;
                case 'L':
                    return 180f;
                case 'M':
                    return 330f;
                case 'N':
                    return 210f;
                case 'Q':
                    return 135f;
                case 'R':
                    return 0f;
                case 'T':
                    return 60f;
                case 'U':
                    return 90f;
                case 'V':
                    return 255f;
                case 'W':
                    return 165f;
                case 'Y':
                    return 285f;
                case 'Z':
                    return 225f;
                default:
                    switch (direction)
                    {
                        case 'o':
                            return 75f;
                        case 'p':
                            return 15f;
                        case 'q':
                            return 105f;
                        default:
                            if (direction == 'x')
                                return 195f;
                            break;
                    }
                    break;
            }
            exists = false;
            return result;
        }

        public static float GetAngleFromFloorCharDirection(char direction) => 
            GetAngleFromFloorCharDirectionWithCheck(direction, out _);

        public static double IncrementAngle(double startangle, double increment) => Mod(startangle + increment, 6.2831854820251465);

        public static double Mod(double x, double m) => (x % m + m) % m;
    }
}
