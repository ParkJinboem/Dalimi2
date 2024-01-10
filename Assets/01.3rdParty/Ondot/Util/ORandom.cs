using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace OnDot.Util
{
    public class OSimpleRandom
    {
        float percent;

        public OSimpleRandom(float percent)
        {
            this.percent = percent;
        }

        public bool Pick()
        {
            float num = UnityEngine.Random.Range(0f, 100f);            
            return num < percent;
        }

        public void DebugPick(int count)
        {
            List<int> pickIndexes = new List<int>();
            for (int i = 0; i < count; i++)
            {
                pickIndexes.Add(Pick() ? 1 : 0);
            }

            HashSet<int> pickIndexHashSets = pickIndexes.OrderBy(x => x).ToHashSet();
            foreach (var pickIndex in pickIndexHashSets)
            {
                Debug.Log($"{pickIndex} : {pickIndexes.FindAll(x => x == pickIndex).Count / (float)count * 100}%");
            }
        }
    }

    public class ORandom
    {
        List<int> indexes;

        public ORandom(float percent) : this(new float[] { percent, 100 - percent })
        {

        }

        public ORandom(float[] percents)
        {
            List<RandomData> randomDatas = new List<RandomData>();
            for (int i = 0; i < percents.Length; i++)
            {
                RandomData randomData = new RandomData();
                randomData.index = i;
                randomData.percent = percents[i];
                randomDatas.Add(randomData);
            }
            randomDatas.Sort((x1, x2) => x1.percent.CompareTo(x2.percent));

            // add index
            indexes = new List<int>();
            int totalPercent = 0;
            foreach (RandomData randomData in randomDatas)
            {
                // 0.00x, 확률은 소수점 세자리까지 사용 가능
                int percent = Mathf.FloorToInt(randomData.percent * 1000);
                for (int i = 0; i < percent; i++)
                {
                    indexes.Add(randomData.index);
                }
                totalPercent += percent;
            }
            //Debug.Log("총 확률 : " + (float)totalPercent / 1000);

            // shuffle
            Random shuffle = new Random();
            int n = indexes.Count;
            while (n > 1)
            {
                n--;
                int k = shuffle.Next(n + 1);
                int value = indexes[k];
                indexes[k] = indexes[n];
                indexes[n] = value;
            }
        }

        /// <summary>
        /// 
        /// ORandom(float percent) 생성 시 사용
        /// 
        /// 성공 / 실패 처리
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Pick()
        {
            return PickIndex() == 0;
        }

        /// <summary>
        ///
        /// ORandom(float[] percents) 생성 시 사용
        ///
        /// percents의 단일 랜덤 index값 반환
        /// 
        /// </summary>
        /// <returns></returns>
        public int PickIndex()
        {
            int num = UnityEngine.Random.Range(0, 100 * 1000);
            return indexes[num];
        }

        /// <summary>
        /// PickIndex() 확률 확인 및 오류 테스트
        /// </summary>
        /// <param name="count"></param>
        public void DebugPickIndex(int count)
        {
            List<int> pickIndexes = new List<int>();
            for (int i = 0; i < count; i++)
            {
                pickIndexes.Add(PickIndex());
            }

            HashSet<int> pickIndexHashSets = pickIndexes.OrderBy(x => x).ToHashSet();
            foreach (var pickIndex in pickIndexHashSets)
            {
                Debug.Log($"{pickIndex} : {pickIndexes.FindAll(x => x == pickIndex).Count / (float)count * 100}%");
            }
        }

        public struct RandomData
        {
            public int index;
            public float percent;
        }
    }
}