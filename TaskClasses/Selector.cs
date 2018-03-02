using System;
using System.Collections.Generic;

namespace TaskClasses
{
    /// <summary>
    /// 2. Задается коллекция целых чисел (Z1, Z2,…) и целое число Х. Надо вывести все сочетания из двух элементов заданной коллекции, такие чтобы сумма 
    /// элементов была равна заданному Х. При этом каждый элемент может быть участником только одного сочетания и не может использоваться повторно в другом. 
    /// Порядок элементов в сочетании не важен.
    /// Например: (1,1,2,1,1,0,1) => (1,1), (1,1), (2, 0)  или(1,1), (1,1), (0, 2)
    /// </summary>
    public class Selector
    {
        public static int[][] Select(int[] array, int sum)
        {
            Array.Sort(array);
            int firstIndex = 0;
            int lastIndex = array.Length - 1;
            var result = new List<int[]>();

            while (firstIndex < lastIndex)
            {
                int s = array[firstIndex] + array[lastIndex];
                if (s == sum)
                {
                    result.Add(new int[] { array[firstIndex],  array[lastIndex] });
                    lastIndex--;
                    firstIndex++;
                }
                else
                {
                    if (s < sum) firstIndex++;
                    else lastIndex--;
                }
            }

            return result.ToArray();
        }
    }
}

