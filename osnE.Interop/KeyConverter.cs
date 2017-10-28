using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace osnE.Interop
{
    public class KeyTransformer
    {
        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);
        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }

        public static string mapKeyPressToActualCharacter(bool isShiftKey, Key k)
        {
            int characterCode = KeyInterop.VirtualKeyFromKey(k);
            if (characterCode == 27 || characterCode == 8 || characterCode == 9 || characterCode == 20 || characterCode == 16
                || characterCode == 17 || characterCode == 91 || characterCode == 13 || characterCode == 92
                || characterCode == 18)
            {
                return null;
            }

            string[] characterMap = new string[255];
            characterMap[192] = "~";
            characterMap[49] = "!";
            characterMap[50] = "@";
            characterMap[51] = "#";
            characterMap[52] = "$";
            characterMap[53] = "%";
            characterMap[54] = "^";
            characterMap[55] = "&";
            characterMap[56] = "*";
            characterMap[57] = "(";
            characterMap[48] = ")";
            characterMap[109] = "_";
            characterMap[107] = "+";
            characterMap[219] = "{";
            characterMap[221] = "}";
            characterMap[220] = "|";
            characterMap[59] = ":";
            characterMap[222] = "\"";
            characterMap[188] = "<";
            characterMap[190] = ">";
            characterMap[191] = "?";
            characterMap[32] = " ";
            string character = null;
            if (isShiftKey)
            {
                if (characterCode >= 65 && characterCode <= 90)
                {
                    character = KeyTransformer.GetCharFromKey(k).ToString();
                }
                else
                {
                    character = characterMap[characterCode];
                }
            }
            else
            {
                if (characterCode >= 65 && characterCode <= 90)
                {
                    character = KeyTransformer.GetCharFromKey(k).ToString().ToLower();
                }
                else
                {
                    character = KeyTransformer.GetCharFromKey(k).ToString();
                }
            }
            if (string.IsNullOrEmpty(character) || string.IsNullOrWhiteSpace(character))
                return null;

            Console.WriteLine("MappedKey="+character);
            return character;
        }
    }
}
