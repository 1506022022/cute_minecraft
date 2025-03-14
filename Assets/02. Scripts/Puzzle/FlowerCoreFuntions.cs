using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    public delegate void CoreFunction(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message);

    public static class FlowerCoreFuntions
    {
        private static readonly List<int[]> _crossIndices = new() { new[] { 0, 0 }, new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 } };

        public static void CheckClearFlowerNormalStage(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            message = new();
            var currentStage = (Face)FlowerReader.GetFace(data);
            if (CheckClearFlowerPuzzle(currentStage, cubeMap))
            {
                message.Add(SystemReader.GetClearMessage(currentStage));
            }
        }
        public static void CheckFlowerBossStageClear(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            message = new();
            var bossStage = Face.bottom;
            if (CheckClearFlowerPuzzle(bossStage, cubeMap))
            {
                message.Add(SystemReader.GetClearMessage(bossStage));
            }
        }
        private static bool CheckClearFlowerPuzzle(Face stage, CubeMap<byte> cubeMap)
        {
            var @base = cubeMap.GetElements(stage).Where(FlowerReader.IsFlower);
            return @base.All(x => x == FlowerReader.FLOWER_GREEN) || @base.All(x => x == FlowerReader.FLOWER_RED);
        }
        public static void AttackCross(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            message = new List<byte[]>();
            var flower = cubeMap.GetElements(data[0], data[1], data[2]);
            if (flower != 1 && flower != 2)
            {
                return;
            }

            foreach (var dxdy in _crossIndices)
            {
                if (data[0] + dxdy[0] < 0 || cubeMap.Width <= data[0] + dxdy[0] ||
                    data[1] + dxdy[1] < 0 || cubeMap.Width <= data[1] + dxdy[1])
                {
                    continue;
                }

                var index = new byte[] { (byte)(data[0] + dxdy[0]), (byte)(data[1] + dxdy[1]), data[2], data[3] };
                flower = cubeMap.GetElements(index[0], index[1], index[2]);
                if (flower != 1 && flower != 2)
                {
                    continue;
                }
                AttackDot(index, cubeMap, out var dotMessage);
                message.AddRange(dotMessage);
            }

        }

        public static void AttackCrossLink(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            message = new List<byte[]>();

            var face = data[2];
            if (face is (byte)Face.bottom)
            {
                return;
            }

            var flower = cubeMap.GetElements(data[0], data[1], data[2]);
            if (flower is not (byte)Flower.Type.Red &&
                flower is not (byte)Flower.Type.Green)
            {
                return;
            }

            var x = data[0];
            var y = data[1];
            var attackType = data[3];
            CubeMapReader reader = new CubeMapReader(cubeMap);
            reader.TryGetCrossIndices(x, y, face, out var cross);

            foreach (var index in cross)
            {
                if (reader.GetElement(new byte[] { (byte)index.x, (byte)index.y, (byte)face }) == 0)
                {
                    continue;
                }
                AttackDot(new byte[] { (byte)index.x, (byte)index.y, (byte)Face.bottom, attackType }, cubeMap, out var bossFaceMessage);
                AttackDot(new byte[] { (byte)index.x, (byte)index.y, face, attackType }, cubeMap, out var currentFaceMessage);
                message.AddRange(bossFaceMessage);
                message.AddRange(currentFaceMessage);
            }
        }

        public static void AttackDot(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            message = new List<byte[]>();
            var flower = cubeMap.GetElements(data[0], data[1], data[2]);
            byte[] result;
            switch (flower)
            {
                case 1:
                    result = new[] { data[0], data[1], data[2], (byte)2 };
                    break;
                case 2:
                    result = new[] { data[0], data[1], data[2], (byte)1 };
                    break;
                default:
                    return;
            }
            cubeMap.SetElements(result[0], result[1], result[2], result[3]);
            message.Add(result);
        }

        public static void CreateFlower(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            message = new List<byte[]>();
            var flower = cubeMap.GetElements(data[0], data[1], data[2]);
            byte[] result;
            switch (flower)
            {
                case 0:
                    result = new[] { data[0], data[1], data[2], (byte)Random.Range(1, 2) };
                    break;
                default:
                    return;
            }
            cubeMap.SetElements(result[0], result[1], result[2], result[3]);
            message.Add(result);
        }
        public static void PrintDebugMessage(byte[] data, CubeMap<byte> cubeMap, out List<byte[]> message)
        {
            Debug.LogWarning($"{DM_ERROR.INVALID_FORMAT} {data}");
            message = new List<byte[]>();
        }
    }


}
