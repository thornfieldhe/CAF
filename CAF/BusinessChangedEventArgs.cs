using System;

namespace CAF
{
    public class BusinessChangedEventArgs : EventArgs
    {
        public string updateFiled;
        public BusinessChangedEventArgs(string updateFiled)
        {
            this.updateFiled += updateFiled;
        }

        public BusinessChangedEventArgs() { }
    }
}
