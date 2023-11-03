using UnityEngine;
using System;
namespace TMPro
{
    // Input validator accepts date input in MM/DD/YYY format
    [Serializable]
    [CreateAssetMenu(fileName = "InputValidator - Date.asset", menuName = "TextMeshPro/Input Validators/Date", order = 100)]
    public class DateInput : TMP_InputValidator
    {
        // Custom text input validation function
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (pos > 9)
            {
                return (char)0;
            }
            else if (pos != 2 && pos != 5 && ch >= '0' && ch <= '9')
            {
                text += ch;
                pos += 1;
                return ch;
            } 
            else if ((pos == 2 || pos == 5) && ch == '/')
            {
                text += ch;
                pos += 1;
                return ch;
            }
            return (char)0;
        }
    }
}
