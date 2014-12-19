namespace TheHUtil.Extensions
{
    using System;

    public static class ExceptionFactory
    {
        public static NotImplementedException NewFutureImplemetationException
        {
            get
            {
                return new NotImplementedException("This has yet to be implemented, but will be. Check for a newer version.");
            }                
        }

        public static NotImplementedException NeverToBeImplementedException
        {
            get
            {
                return new NotImplementedException("This method is not to be called.");
            }
        }
    }
}
