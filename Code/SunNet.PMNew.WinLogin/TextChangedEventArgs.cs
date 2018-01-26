using System;

namespace SunNet.PMNew.WinLogin
{
    public class TextChangedEventArgs : EventArgs
    {
        public TextChangedEventArgs(string text)
        {
            _text = text;
        }

        string _text;
        public string Text
        {
            get { return _text; }
        }
    }
}
