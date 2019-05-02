using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Text;


namespace Siegrain.Common
{
    public class CHNToPinyin
    {
        /// <summary> 
        /// 将字符串转换成拼音 
        /// </summary> 
        /// <param name="chineseStr">拼音字符串</param> 
        /// <param name="includeTone">是否包含音调</param> 
        /// <returns></returns> 
        public static string ConvertToPinYin(string chineseStr, bool includeTone)
        {
            if (chineseStr == null)
                throw new ArgumentNullException(nameof(chineseStr));

            var charArray = chineseStr.ToCharArray();
            var sb = new StringBuilder();
            foreach (var c in charArray)
            {
                if (!IsValidChar(c)) {
                    sb.Append(c);
                    continue;
                }
                var chineseChar = new ChineseChar(c);
                var pyColl = chineseChar.Pinyins;
                if (pyColl.Length == 0) continue;
                sb.Append(pyColl[0] + " ");
            }

            if (!includeTone)
            {
                var sb2 = new StringBuilder();
                foreach (var c in sb.ToString())
                {
                    if (!char.IsNumber(c))
                        sb2.Append(c);
                }
                return sb2.ToString();
            }

            return sb.ToString();
        }

        public static string ConvertToPinYin(string chineseStr)
        {
            return ConvertToPinYin(chineseStr, false);
        }

        public static bool IsValidChar(char ch)
        {
            return ChineseChar.IsValidChar(ch);
        }

        /// <summary> 
        /// 是否为有效的中文字 
        /// </summary> 
        /// <param name="chn"></param> 
        /// <returns></returns> 
        public static bool IsValidChinese(string chn)
        {
            if (chn == null)
                throw new ArgumentNullException(nameof(chn));

            foreach (var c in chn)
            {
                if (!IsValidChar(c))
                    return false;
            }

            return true;
        }
    }

}
