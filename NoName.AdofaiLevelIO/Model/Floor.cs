using System;
using System.Collections.Generic;
using System.Linq;
//using System.Globalization;
using Newtonsoft.Json.Linq;
using NoName.AdofaiLevelIO.Model.Actions;

//using UnityEngine;

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
                var data = LevelReader.GetPathData(JObject).ToCharArray();
                data[Index] = value;
				LevelReader.SetPathData(JObject, new string(data));
                _floorCacheContainer.Caching(Index, new FloorCache() { Direction = value });
            }
        }

        public float EntryAngle
        {
            get
            {
                if (_floorCacheContainer.TryGetValue(Index, out var floorCache) && floorCache.EntryAngle != null)
                    return floorCache.EntryAngle.Value;
                _floorCacheContainer.Caching(Index, new FloorCache() { EntryAngle = 11111111 });
				throw new NotImplementedException();
            }
            set => _floorCacheContainer.Caching(Index, new FloorCache() { EntryAngle = value });
        }
        public float ExitAngle
        {
            get
            {
                if (_floorCacheContainer.TryGetValue(Index, out var floorCache) && floorCache.ExitAngle != null)
                    return floorCache.ExitAngle.Value;
                _floorCacheContainer.Caching(Index, new FloorCache() { ExitAngle = 11111111 });
				throw new NotImplementedException();
            }
            set => _floorCacheContainer.Caching(Index, new FloorCache() { ExitAngle = value });
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
                Actions.Add(new Data.SetSpeed(SpeedType.Bpm, value));
				_floorCacheContainer.Caching(Index, new FloorCache() { Bpm = value });
			}
        }

        public bool IsMidSpin => Direction == '!';

        public ActionContainer Actions { get; }

        private readonly AdofaiLevel _adofaiLevel;
        private readonly FloorCacheContainer _floorCacheContainer;

		internal Floor(JObject jObject, int floorIndex, AdofaiLevel adofaiLevel, FloorCacheContainer floorCacheContainer) : base(jObject)
        {
            Actions = new ActionContainer(jObject, floorIndex, _floorCacheContainer);
            Index = floorIndex;
            _adofaiLevel = adofaiLevel;
            _floorCacheContainer = floorCacheContainer;
        }

  //      public static double incrementAngle(double startangle, double increment)
  //      {
  //          return mod(startangle + increment, 6.2831854820251465);
  //      }

  //      public static double mod(double x, double m)
  //      {
  //          return (x % m + m) % m;
  //      }

  //      public static Vector3 getVectorFromAngle(double angle, double radius)
  //      {
  //          return new Vector3((float)(Math.Sin(angle) * radius), (float)(Math.Cos(angle) * radius), 0f);
  //      }

		//public List<Floor> MakeLevel()
		//{
		//	var listFloors = new List<Floor>();
		//	string text = LevelReader.GetPathData(JObject);
		//	Vector3 vector = Vector3.zero;
		//	bool flag = true;
		//	float num = 1f;
		//	scrFloor component = UnityEngine.Object.Instantiate<GameObject>(this.floor, Vector3.zero, Quaternion.identity).GetComponent<scrFloor>();
		//	listFloors.Add(component);
		//	component.hasLit = true;
		//	component.entryangle = 4.71238899230957;
		//	component.name = "0/FloorR";
		//	int num2 = 1;
		//	for (int i = 0; i < text.Length; i++)
		//	{
		//		double radius = (double)scrController.instance.startRadius;
		//		double num3 = 0.0;
  //              bool isEditor = true;//Application.isEditor;
		//		char c = text[i];
		//		if (c <= 'q')
		//		{
		//			if (c != '!')
		//			{
		//				switch (c)
		//				{
		//					case '5':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, 1.8849555253982544);
		//						break;
		//					case '6':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, -1.8849555253982544);
		//						break;
		//					case '7':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, 2.243994951248169);
		//						break;
		//					case '8':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, -2.243994951248169);
		//						break;
		//					case '9':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, 3.665191411972046);
		//						break;
		//					case 'A':
		//						num3 = 1.832595705986023;
		//						break;
		//					case 'B':
		//						num3 = 2.6179938316345215;
		//						break;
		//					case 'C':
		//						num3 = 2.356194496154785;
		//						break;
		//					case 'D':
		//						num3 = 3.1415927410125732;
		//						break;
		//					case 'E':
		//						num3 = 0.7853981852531433;
		//						break;
		//					case 'F':
		//						num3 = 3.665191411972046;
		//						break;
		//					case 'G':
		//						num3 = 5.759586334228516;
		//						break;
		//					case 'H':
		//						num3 = 5.235987663269043;
		//						break;
		//					case 'J':
		//						num3 = 1.0471975803375244;
		//						break;
		//					case 'L':
		//						num3 = 4.71238899230957;
		//						break;
		//					case 'M':
		//						num3 = 2.094395160675049;
		//						break;
		//					case 'N':
		//						num3 = 4.188790321350098;
		//						break;
		//					case 'Q':
		//						num3 = 5.4977874755859375;
		//						break;
		//					case 'R':
		//						num3 = 1.5707963705062866;
		//						break;
		//					case 'T':
		//						num3 = 0.5235987901687622;
		//						break;
		//					case 'U':
		//						num3 = 0.0;
		//						break;
		//					case 'V':
		//						num3 = 3.4033920764923096;
		//						break;
		//					case 'W':
		//						num3 = 4.974188327789307;
		//						break;
		//					case 'Y':
		//						num3 = 2.879793167114258;
		//						break;
		//					case 'Z':
		//						num3 = 3.9269909858703613;
		//						break;
		//					case '[':
		//						if (isEditor)
		//						{
		//							var flag2 = false;
  //                                  var num4 = i + 1;
  //                                  var flag3 = false;
		//							if (i + 1 <= text.Length && text[i + 1] == '*')
		//							{
		//								flag3 = true;
		//								num4++;
		//							}
		//							while (i + 1 <= text.Length && !flag2)
		//							{
		//								i++;
		//								if (text[i] == ']')
		//								{
		//									flag2 = true;
		//								}
		//							}
		//							string s = text.Substring(num4, i - num4).Replace(" ", "");
		//							float num5 = 0f;
		//							if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out num5))
		//							{
		//								float num6 = 0.017453292f * num5;
		//								num3 = (flag3 ? incrementAngle(listFloors.Last<Floor>().EntryAngle, (double)num6) : ((double)num6));
		//							}
		//						}
		//						break;
		//					case 'h':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, 2.094395160675049);
		//						break;
		//					case 'j':
		//						num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, -2.094395160675049);
		//						break;
		//					case 'o':
		//						num3 = 0.2617993950843811;
		//						break;
		//					case 'p':
		//						num3 = 1.3089969158172607;
		//						break;
		//					case 'q':
		//						num3 = 6.021385669708252;
		//						break;
		//				}
		//			}
		//			else
		//			{
		//				num3 = listFloors.Last<Floor>().EntryAngle;
		//			}
		//		}
		//		else if (c != 't')
		//		{
		//			if (c != 'x')
		//			{
		//				if (c == 'y')
		//				{
		//					num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, (double)(1.0471976f * (float)(flag ? -1 : 1)));
		//				}
		//			}
		//			else
		//			{
		//				num3 = 4.450589656829834;
		//			}
		//		}
		//		else
		//		{
		//			num3 = incrementAngle(listFloors.Last<Floor>().EntryAngle, (double)(1.0471976f * (float)(flag ? 1 : -1)));
		//		}
		//		Vector3 vectorFromAngle = getVectorFromAngle(num3, radius);
		//		vector += vectorFromAngle;
		//		if (listFloors.Count > 0)
		//		{
		//			listFloors[listFloors.Count - 1].ExitAngle = num3;
		//		}
		//		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.floor, vector, default(Quaternion));
		//		gameObject2.name = (i + 1).ToString() + "/Floor" + text[i].ToString();
		//		scrFloor component2 = gameObject2.GetComponent<scrFloor>();
		//		listFloors.Last<Floor>().nextfloor = component2;
		//		listFloors.Add(component2);
		//		component2.direction = text[i];
		//		component2.seqID = num2;
		//		component2.entryangle = (num3 + 3.1415927410125732) % 6.2831854820251465;
		//		if (text[i] == '!')
		//		{
		//			listFloors[num2 - 1].IsMidSpin = true;
		//		}
		//		var flag4 = true;
				
		//		component2.isCCW = !flag;
		//		component2.speed = num;
		//		num2++;
		//	}
		//}
	}
}
