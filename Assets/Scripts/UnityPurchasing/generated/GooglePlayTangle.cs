// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("YCMNSK5pTmcGuTTb+EgfT/rz/Z1r2Vp5a1ZdUnHdE92sVlpaWl5bWBu2Bf+A1vD0WfpTdD/KYaNYy5HHGKyzNK9RNpl5RVefNkbG1pAvlfbZWlRba9laUVnZWlpbxzcRcM/a7xoMQginm+6uf77oJKb4VV5+4h7AwtUsbbssfzUm6pyB/JR6+239d/VZZqINTigomYhO1ghbai5hmfMvA0Cd/VdhoZCxkQzs4qjER1Pl8RzeUZMGll04h002/VkSML1WS21HO4M77YWUHUXWxKnpX/MJKuOJOh+KcwiZ0UI0DDA8LUqiqc77R2iELaRfYLyluCmKYd/C14+6zHqv/Q8aMcXyeFwy8/G9GnCXbG2hvv9MT+mFbEDtIWhVeykBqFlYWlta");
        private static int[] order = new int[] { 4,4,6,5,4,10,7,12,12,10,12,11,12,13,14 };
        private static int key = 91;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
