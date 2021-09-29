using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoSchedule.Core.Models;

namespace AutoSchedule.Core.Helpers
{
    internal static class ClassRoomParser
    {
        public static ClassRoom FromString(string str)
        {
            if (str.Contains("Teaching A")) return ClassRoom.TA;
            if (str.Contains("Teaching B")) return ClassRoom.TB;
            if (str.Contains("Teaching C")) return ClassRoom.TC;
            if (str.Contains("Teaching A")) return ClassRoom.TA;
            if (str.Contains("Zhi Ren")) return ClassRoom.Zhiren;
            if (str.Contains("Zhi Xin")) return ClassRoom.ZhiXin;
            if (str.Contains("Cheng Dao")) return ClassRoom.ChengDao;
            if (str.Contains("Administration Bldg E")) return ClassRoom.ABE;
            if (str.Contains("Administration Bldg W")) return ClassRoom.ABW;

            throw new NotImplementedException($"Parameter {nameof(str)} cannot be parsed to a location.");
        }
    }
}
